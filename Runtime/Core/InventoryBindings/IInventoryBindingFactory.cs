using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;

namespace UnInventory.Core.InventoryBindings
{
    public interface IInventoryBindingFactory
    {
        IInventoryBinding Create(IInventoryStructureContainer inventoryStructureContainer,
            IEnumerable<DataEntity> dataEntitiesForLoad = null);
    }
}