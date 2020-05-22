using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Standard.MVC.Model.Commands.FiltersResponse
{
    public class CommandNoValidOnlyForFiltersPrimary : CommandNoValidOnlyForFilters<ICommand>
    {
        public CommandNoValidOnlyForFiltersPrimary(ICommand command) : base(command)
        {
        }

        protected override FilterNoValidCollection GetFilters()
        {
            var isOneSingleCauseFilters = Command.CausesFailure.IsContainsOnly<CauseNoValidFilters>();
            if (!isOneSingleCauseFilters)
            {
                return new FilterNoValidCollection();
            }

            var causeNoValidFilters = Command.CausesFailure.GetCauseFirstOrDefault<CauseNoValidFilters>();
            return causeNoValidFilters.FilterNoValidNewCollection;
        }
    }
}
