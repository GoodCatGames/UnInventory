using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Core.MVC.Controller.BindComponentToData
{
    public interface IBindComponentToDataDbRead
    {
        InventoryComponent GetInventoryComponent(DataInventory dataInventory);
        ISlotRootComponent GetSlotComponent(DataSlot dataSlot);
        IEntityRootComponent GetEntityComponent(DataEntity dataEntity);
        bool TryGetEntityComponent(DataEntity dataEntity, out IEntityRootComponent componentResult);
        bool TryGetSlotComponent(DataSlot dataSlot, out ISlotRootComponent componentResult);
        bool TryGetInventoryComponent(DataInventory dataInventory, out InventoryComponent componentResult);
        IEnumerable<IEntityRootComponent> GetEntityComponents(DataInventory dataInventory);
    }
}