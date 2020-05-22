using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.Filters.Response;
using UnInventory.Standard.MVC.Model.Commands.Composite;

namespace UnInventory.Standard.MVC.Model.Commands.FiltersResponse
{
    internal class CommandNoValidOnlyForFiltersPut : CommandNoValidOnlyForFilters<PutCommand>
    {
        public CommandNoValidOnlyForFiltersPut(PutCommand command) : base(command)
        {
        }

        protected override FilterNoValidCollection GetFilters()
        {
            if (!Command.CausesFailure.IsContainsOnly<CauseNoCommandsForExecute>())
            {
                return new FilterNoValidCollection();
            }

            var commandNoValidOnlyForFilters = Command.CommandsConsidered.Select(command => Factory.Create(command));
            var noValidOnlyForFilters = commandNoValidOnlyForFilters.FirstOrDefault(filters => filters.IsCommandNoValidOnlyForFilters);

            return noValidOnlyForFilters == null ? new FilterNoValidCollection() :
                noValidOnlyForFilters.GetImpossibleOnlyForThisFilters();
        }
    }
}
