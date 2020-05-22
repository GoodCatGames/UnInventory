namespace UnInventory.Core.MVC.Model.Commands.FiltersResponse
{
    public interface ICommandNoValidOnlyForFiltersFactory
    {
        ICommandNoValidOnlyForFilters Create<T>(T command)
            where T : ICommand;
    }
}