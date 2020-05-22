using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Standard.MVC.Model.Filters;

namespace UnInventory.Standard.MVC.Model
{
    internal class FiltersManager : IFiltersManager
    {
        public IFilterCollection FiltersForHandOnly { get; } = new FilterCollection();
        public IFilterCollection FiltersForAll { get; } = new FilterCollection();
    }
}
