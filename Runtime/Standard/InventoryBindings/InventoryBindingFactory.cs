using System.Collections.Generic;
using UnInventory.Core.InventoryBindings;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;

namespace UnInventory.Standard.InventoryBindings
{
    internal class InventoryBindingFactory : IInventoryBindingFactory
    {
        public IInventoryBinding Create(IInventoryStructureContainer inventoryStructureContainer,
            IEnumerable<DataEntity> dataEntitiesForLoad = null)
        {
            return new InventoryBinding(inventoryStructureContainer, dataEntitiesForLoad);
        }
    }
}
