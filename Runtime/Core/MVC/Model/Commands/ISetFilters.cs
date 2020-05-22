using UnInventory.Core.MVC.Model.Filters;

namespace UnInventory.Core.MVC.Model.Commands
{
    public interface ISetFilters
    {
        void SetFilters(IFilterCollection filterCollectionNew);
    }
}