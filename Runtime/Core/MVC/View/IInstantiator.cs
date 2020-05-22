using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Core.MVC.View
{
    public interface IInstantiator
    {
        IEntityRootComponent AddEntity(DataEntity dataEntity, DataSlot slotLeftTop);
    }
}