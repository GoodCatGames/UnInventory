using System.Linq;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.Filters.Response;
using UnInventory.Standard.MVC.Model.Commands.Composite;

namespace UnInventory.Standard.MVC.Model.Commands.FiltersResponse
{
    public class CommandNoValidOnlyForFiltersSwap : CommandNoValidOnlyForFilters<SwapCommand>
    {
        public CommandNoValidOnlyForFiltersSwap(SwapCommand command) : base(command)
        {
        }

        protected override FilterNoValidCollection GetFilters()
        {
            var commandNoValidOnlyForFilters = Command.CommandsConsidered.Select(command => Factory.Create(command));
            var noValidOnlyForFilters = commandNoValidOnlyForFilters.FirstOrDefault(filters => filters.IsCommandNoValidOnlyForFilters);

            return noValidOnlyForFilters == null ? new FilterNoValidCollection() : 
                noValidOnlyForFilters.GetImpossibleOnlyForThisFilters();
        }
    }
}
