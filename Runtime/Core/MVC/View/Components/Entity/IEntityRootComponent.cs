using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.View.Components.Entity
{
    public interface IEntityRootComponent : IComponentUnInventory<DataEntity>
    {
        void Init(InventoryComponent inventoryComponent, DataEntity dataEntity);
    }
}
