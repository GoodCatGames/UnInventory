using System.Linq;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Model.Commands.FiltersResponse
{
    /// <summary>
    /// Have only one CauseFailure - NoValidFilters
    /// </summary>
    public abstract class CommandNoValidOnlyForFilters<T> : ICommandNoValidOnlyForFilters 
        where T : ICommand
    {
        public bool IsCommandNoValidOnlyForFilters => GetImpossibleOnlyForThisFilters().Any();

        protected readonly T Command;

        protected ICommandNoValidOnlyForFiltersFactory Factory => InventoryManager.ContainerDiInternal.FactoryCommandNoValidOnlyForFilters;

        protected CommandNoValidOnlyForFilters(T command)
        {
            Command = command;
        }
        
        public FilterNoValidCollection GetImpossibleOnlyForThisFilters()
        {
            Command.Update();
            return Command.IsCanExecute ? new FilterNoValidCollection() : GetFilters();
        }

        protected abstract FilterNoValidCollection GetFilters();
    }
}
