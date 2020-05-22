using UnInventory.Core.MVC.Model.Filters;

namespace UnInventory.Core.MVC.Model
{
    public interface IFiltersManager
    {
        IFilterCollection FiltersForHandOnly { get; }
        IFilterCollection FiltersForAll { get; }
    }
}