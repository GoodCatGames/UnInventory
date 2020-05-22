using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Core.MVC.Controller.BindComponentToData
{
    public interface IBindComponentToDataDbWrite
    {
        /// <summary>
        /// Single use
        /// No need invoke BindInventoryComponentToDatabase after this
        /// </summary>
        /// <param name="inventoryStructure"></param>
        /// <param name="inventoryComponent"></param>
        void RegisterInventoryInDatabase(DataInventoryStructure inventoryStructure, InventoryComponent inventoryComponent);

        /// <summary>
        /// Invoke every time you create an inventory transform
        /// </summary>
        /// <param name="inventoryComponent"></param>
        /// <param name="dataInventoryInDatabase"></param>
        void BindInventory(InventoryComponent inventoryComponent, DataInventory dataInventoryInDatabase);

        void UnBindInventory(InventoryComponent inventoryComponent);

        void BindEntity(IEntityRootComponent entityComponent);

        void UnBindEntity(IEntityRootComponent entityComponent);
        
    }
}