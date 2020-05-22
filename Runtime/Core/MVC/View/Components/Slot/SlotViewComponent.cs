using UnityEngine;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.View.Components.Slot
{
    [RequireComponent(typeof(ISlotRootComponent))]
    public abstract class SlotViewComponent : ViewComponent<DataSlot, ISlotRootComponent>
    {
    }
}
