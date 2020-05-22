using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Slot;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnInventory.Samples.Sample_Hero.Components
{
    [RequireComponent(typeof(ISlotRootComponent))]
    public class SlotHotBarInputComponent : SlotInputComponent, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [UsedImplicitly]
        public void OnPointerDown(PointerEventData eventData)
        {
            var success = Hand.TakeEntityOnPositionInHandTry(SlotComponent.Data, eventData.position);
            if (success)
            {
                Hand.AddAmountInHandSourcePercent(100);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PutInSlot(eventData, PutOnlyInHotBarSpot, () => Hand.RemoveEntityInHand());
        }

        [UsedImplicitly]
        public void OnDrag(PointerEventData eventData)
        {
            InventoryManager.Hand.PositionSet(eventData.position);
        }

        private void PutOnlyInHotBarSpot(DataSlot slot)
        {
            if (slot.DataInventory == SlotComponent.InventoryComponent.DataInventory)
            {
                Hand.PutHandInSlotTry(slot);
            }
            else
            {
                Hand.RemoveEntityInHand();
            }
        }
    }
}
