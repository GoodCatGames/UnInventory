using System.Linq;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Standard.MVC.Model.Filters;

namespace UnInventory.Standard.MVC.Model
{
    internal class CommandsFactory : ICommandsFactory
    {
        private readonly IFiltersManager _filtersManager;

        public CommandsFactory(IFiltersManager filtersManager)
        {
            _filtersManager = filtersManager;
        }
        
        public T CreateForHand<T>()
            where T : ICommand, ISetFilters, new()
        {
            var filtersUiAndModel = new FilterCollection(_filtersManager.FiltersForAll.Concat(_filtersManager.FiltersForHandOnly).ToList());
            return Create<T>(filtersUiAndModel);
        }
        
        public T Create<T>()
            where T : ICommand, ISetFilters, new() => Create<T>(_filtersManager.FiltersForAll);

        private T Create<T>(IFilterCollection filterCollection)
            where T : ICommand, ISetFilters, new()
        {
            var command = Command<ICommandInputData>.CreateCommand<T>(filterCollection);
            return command;
        }
    }
}
