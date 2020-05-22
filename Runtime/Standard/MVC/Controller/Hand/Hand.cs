using System;
using JetBrains.Annotations;
using UnInventory.Core.Extensions;
using UnInventory.Core.InventoryBindings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Controller.Hand;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.Model.Filters.Response;
using UnInventory.Core.MVC.View;
using UnInventory.Standard.Configuration;
using UnInventory.Standard.MVC.Model.Commands.Composite;
using UnInventory.Standard.MVC.Model.Commands.Composite.InputData;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Events;
using Object = UnityEngine.Object;

namespace UnInventory.Standard.MVC.Controller.Hand
{
    /// <summary>
    /// Virtual "Hand" with which you drag and drop a certain amount of Entity. Provides visual display including.
    /// Use this in the SlotInputComponent and/or other Input modules you implement.
    /// </summary>
    internal class Hand : IHand
    {
        public UnityEvent StatusWasChanged { get; } = new UnityEvent();
        public UnityEvent<IReadOnlyFilterNoValidCollection> NoValidFiltersEvent { get; } = new NoValidFiltersEvent();
        public bool IsEmpty => HandData.IsEmpty;
        public bool IsAreaEntity => HandData.EntitySource.IsAreaEntity;

        public int AmountInHand => HandData.AmountInHand;
        
        public DataEntity DataEntityHand => HandData.EntitySource;

        private ICommandsFactory Commands => InventoryManager.ContainerDi.Commands;

        private ICommandNoValidOnlyForFiltersFactory FactoryCommandNoValidOnlyForFilters =>
            InventoryManager.ContainerDiOverride<ContainerDiStandard>().FactoryCommandNoValidOnlyForFilters;
        private IDebug Debug => InventoryManager.Get();
        private IBindComponentToDataDbRead _bindComponentToDataDbRead => InventoryManager.ContainerDi.BindComponentToDataDbRead;
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;

        private IPositionsManager _positionsManager => InventoryManager.ContainerDiOverride<ContainerDiStandard>().PositionsManager;

        private HandData HandData { get; } = new HandData();
        private IHandView _handView;

        // debug
        private Transform _point;
        
        public bool TakeEntityOnPositionInHandTry(DataSlot slot,  Vector2 positionCreateHand)
        {
            if (slot == null) throw new ArgumentNullException(nameof(slot));
            var entitySource = DatabaseReadOnly.GetEntityOrNull(slot);

            if (entitySource == null) return false;
            if (Debug.DebugMode)
            {
                _point = Debug.PointForDebug.InstantiateOnRootCanvas().transform;
                _point.SetAsLastSibling();
            }

            TakeEntityInHand(entitySource);
            var entityComponent = _bindComponentToDataDbRead.GetEntityComponent(entitySource);
            _handView = new HandView(entityComponent, HandData.DataEntityHand); 
            _handView.PositionSet(positionCreateHand);
            return true;
        }
        
        public void AddAmountInHandSourcePercent(float percent)
        {
            var totalAmount = HandData.EntitySource.Amount + HandData.AmountInHand;
            var amountAdd = (int)Math.Round(totalAmount * percent/100);
            AddAmountInHand(amountAdd);
        }
        
        public void AddAmountInHand(int amountAdd)
        {
            if (amountAdd <= 0) throw new ArgumentOutOfRangeException(nameof(amountAdd));
            if (HandData.IsEmpty) { throw new Exception("Hand is empty!"); }
            if (HandData.EntitySource.Amount == 0) { return;}

            var canTake = Math.Min(HandData.EntitySource.Amount, amountAdd);

            HandData.EntitySource.Amount -= canTake;
            HandData.DataEntityHand.Amount += canTake;
            
            StatusWasChanged.Invoke();
        }

        public void PutHandInSlotTry(DataSlot slotLeftTop)
        {
            PutHandInSlotTryData(slotLeftTop);
            DestroyView();
        }

        public void UndoTakeEntityInHand()
        {
            UndoTakeEntityInHandData();
            DestroyView();
            StatusWasChanged.Invoke();
        }

        public void RemoveEntityInHand()
        {
            var entity = HandData.EntitySource;
            var amount = HandData.AmountInHand;
            UndoTakeEntityInHand();
            var removeCommand = Commands.CreateForHand<RemoveCommand>().EnterData(new RemoveInputData(entity, amount));
            removeCommand.ExecuteTry();
            DestroyView();
            StatusWasChanged.Invoke();
        }

        public void PositionSet(Vector2 position)
        {
            _handView?.PositionSet(position);

            if (Debug.DebugMode && !IsEmpty)
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                Vector2 delta = Vector2.zero;
                if (IsAreaEntity)
                {
                    delta = _positionsManager.GetDeltaEntityAreaCenterToLeftTopWithScaleFactor(DataEntityHand);
                }
                pointerEventData.position = position + delta;
                _point.position = pointerEventData.position;
                _point.SetAsLastSibling();
            }
        }

        public bool IsCanPutHandInSlot(DataSlot slotLeftTop)
        {
            if(IsEmpty) throw new Exception("Hand is empty!");
            var entitySource = HandData.EntitySource;
            var amountWantPut = HandData.DataEntityHand.Amount;
            entitySource.Amount += amountWantPut;
            var command = Commands.CreateForHand<PutCommand>()
                .EnterData(new PutInputData(entitySource, slotLeftTop, amountWantPut));
            var isCanExecute = command.IsCanExecute;
            entitySource.Amount -= amountWantPut;
            return isCanExecute;
        }

        private void DestroyView()
        {
            _handView?.Destroy();
            if (Debug.DebugMode) { Object.Destroy(_point.gameObject); }
        }

        private void TakeEntityInHand([NotNull] DataEntity entity, int amount = 1)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if(!HandData.IsEmpty) { throw new Exception("Hand already full!");}
            HandData.EntitySource = entity;
            HandData.DataEntityHand = entity.Copy();
            HandData.DataEntityHand.Amount = 0;
            AddAmountInHand(amount);
        }

        private void UndoTakeEntityInHandData()
        {
            HandData.ReturnAmountInSource();
            HandData.Clear();
            StatusWasChanged.Invoke();
        }

        private void PutHandInSlotTryData([NotNull] DataSlot slotLeftTop)
        {
            if (slotLeftTop.DataInventory == null)
            {
                throw GetExceptionUnBindInventoryComponent();
            }
            
            var entitySource = HandData.EntitySource;
            var amountWantPut = HandData.DataEntityHand.Amount;
            HandData.ReturnAmountInSource();
            var slotLeftTopPrevious = DatabaseReadOnly.GetSlotOrNull(entitySource);
            if (slotLeftTopPrevious != slotLeftTop)
            {
                var command = Commands.CreateForHand<PutCommand>()
                    .EnterData(new PutInputData(entitySource, slotLeftTop, amountWantPut));
                var success = command.ExecuteTry();
                
                if (!success)
                {
                    var noValidOnlyForFilters = FactoryCommandNoValidOnlyForFilters.Create(command);

                    // event
                    if (noValidOnlyForFilters.IsCommandNoValidOnlyForFilters)
                    {
                        NoValidFiltersEvent.Invoke(noValidOnlyForFilters.GetImpossibleOnlyForThisFilters());
                    }
                }
            }
            HandData.Clear();
            StatusWasChanged.Invoke();
        }

        private Exception GetExceptionUnBindInventoryComponent()
        {
            return new Exception($"InventoryComponent is unbind! " +
                                 $"Use {nameof(IInventoryBinding)} to bind before using Inventory!");
        }
    }
}