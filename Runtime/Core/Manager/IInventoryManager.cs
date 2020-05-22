using UnInventory.Core.MVC.Model;

namespace UnInventory.Core.Manager
{
    public interface IInventoryManager
    {
        IFiltersManager FiltersManager { get; }
    }
}
