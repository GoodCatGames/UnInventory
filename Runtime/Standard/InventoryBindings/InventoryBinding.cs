using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.Configuration;
using UnInventory.Core.InventoryBindings;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Standard.Configuration;
using UnInventory.Standard.InventoryBindings.States;
using UnityEngine.Events;

namespace UnInventory.Standard.InventoryBindings
{
    internal class InventoryBinding : IInventoryBinding
    {
        private static readonly HashSet<InventoryComponent> BoundInventoryComponents = new HashSet<InventoryComponent>();

        public State State { get; set; }
        public UnityEvent<State> ChangeState { get; } = new ChangeStateEvent();

        public DataInventory DataInventory => _stateClass.DataInventory;
        private ContainerDi ContainerDi => InventoryManager.ContainerDiOverride<ContainerDiStandard>();

        private InventoryComponent _inventoryComponent;
        private StateInventoryBind _stateClass;


        public InventoryBinding(IInventoryStructureContainer inventoryStructureContainer, IEnumerable<DataEntity> dataEntitiesForLoad = null)
        {
            var inventoryStructure = inventoryStructureContainer.GetInventoryStructure().Clone();
            _stateClass = new NoInitState(this, inventoryStructure, dataEntitiesForLoad);
        }

        public IInventoryBinding Bind(InventoryComponent inventoryComponent)
        {
            _stateClass.Bind(inventoryComponent);
            ContainerDi.Viewer.UpdateEntitiesInventory(DataInventory);
            inventoryComponent.DestroyEvent.AddListener(() => UnBind());
            ChangeState.Invoke(State.Bind);
            return this;
        }

        public IInventoryBinding UnBind()
        {
            if (State != State.UnBind)
            {
                _inventoryComponent.DestroyEvent.RemoveListener(() => UnBind());
                _stateClass.Unbind(_inventoryComponent);
            }
            return this;
        }

        private void SetState([NotNull] StateInventoryBind state)
        {
            _stateClass = state;
            State = state is BindState ? State.Bind : State.UnBind;
            ChangeState.Invoke(State);
        }

        // Nested class to access private members
        internal abstract class StateInventoryBind
        {
            internal abstract DataInventory DataInventory { get; }
            protected readonly InventoryBinding InventoryBinding;

            protected IBindComponentToDataDbWrite BindComponentToDataDbWrite => ContainerDi.BindComponentToDataDbWrite;
            protected IBindComponentToDataDbRead BindComponentToDataDbRead => ContainerDi.BindComponentToDataDbRead;

            private ContainerDi ContainerDi => InventoryManager.ContainerDiOverride<ContainerDiStandard>();
            
            protected StateInventoryBind(InventoryBinding inventoryDataBind) => InventoryBinding = inventoryDataBind;

            public abstract void Bind([NotNull] InventoryComponent inventoryComponent);
            public abstract void Unbind([NotNull] InventoryComponent inventoryComponent);

            protected void SetState(StateInventoryBind state) => InventoryBinding.SetState(state);

            protected void BindInventoryComponent(InventoryComponent inventoryComponent)
            {
                InventoryBinding._inventoryComponent = inventoryComponent;
                CheckAlreadyBindInventoryComponent(inventoryComponent);
                BoundInventoryComponents.Add(inventoryComponent);
            }

            protected void UnBindInventoryComponent(InventoryComponent inventoryComponent)
            {
                BoundInventoryComponents.Remove(inventoryComponent);
                InventoryBinding._inventoryComponent = null;
            }

            private void CheckAlreadyBindInventoryComponent(InventoryComponent inventoryComponent)
            {
                var alreadyBind = BoundInventoryComponents.Contains(inventoryComponent);
                if (alreadyBind)
                {
                    throw new Exception($"{inventoryComponent} already bind!");
                }
            }
        }
    }
}