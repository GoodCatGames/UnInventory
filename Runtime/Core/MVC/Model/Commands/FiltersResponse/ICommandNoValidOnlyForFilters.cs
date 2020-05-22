using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Model.Commands.FiltersResponse
{
    public interface ICommandNoValidOnlyForFilters
    {
        FilterNoValidCollection GetImpossibleOnlyForThisFilters();
        bool IsCommandNoValidOnlyForFilters { get; }
    }
}