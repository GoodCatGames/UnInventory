using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.View.Components.Slot
{
    public interface ISlotRootComponent : IComponentUnInventory<DataSlot>
    {
        new DataSlot Data { get; }
        bool IsEmpty { get; }

        void SetInventory(InventoryComponent inventoryComponent, DataInventory dataInventory);
    }
}