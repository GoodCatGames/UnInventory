using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnInventory.Core.Extensions;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Core.MVC.Controller
{
    public abstract class SlotInputComponent : MonoBehaviour
    {
        protected ISlotRootComponent SlotComponent;
        protected Hand.IHand Hand => InventoryManager.Hand;
        private IPositionsManager _positionsManager => InventoryManager.ContainerDiInternal.PositionsManager;
        
        [UsedImplicitly]
        private void Awake()
        {
            SlotComponent = GetComponent<ISlotRootComponent>();
        }

        protected void PutInSlotOrUndoTakeHand(PointerEventData eventData)
        {
            PutInSlot(eventData, slot => Hand.PutHandInSlotTry(slot), () => Hand.UndoTakeEntityInHand());
        }
        
        protected void PutInSlot(PointerEventData eventData, Action<DataSlot>  endDragInSlot, Action endDragOutSlot)
        {
            if (Hand.IsEmpty) { return; }
            
            var positionPutHand = GetPositionHand(eventData);
            var slotOnEndDrag = SlotOrNullInPosition(positionPutHand);
            var isEndDragOnSlot = slotOnEndDrag != null;

            if (isEndDragOnSlot)
            {
                endDragInSlot.Invoke(slotOnEndDrag.Data);
            }
            else
            {
                endDragOutSlot.Invoke();
            }
        }

        private static ISlotRootComponent SlotOrNullInPosition(Vector2 positionPutHand)
        {
            var slotOnEndDrag = GraphicRaycasterExtension.GetComponentsInPositionAllCanvas<ISlotRootComponent>(positionPutHand)
                .FirstOrDefault();
            return slotOnEndDrag;
        }

        private Vector2 GetPositionHand(PointerEventData eventData)
        {
            var delta = Vector2.zero;
            if (Hand.IsAreaEntity)
            {
                delta = _positionsManager.GetDeltaEntityAreaCenterToLeftTopWithScaleFactor(InventoryManager.Hand.DataEntityHand);
            }

            var positionPutHand = eventData.position + delta;
            return positionPutHand;
        }
    }
}
