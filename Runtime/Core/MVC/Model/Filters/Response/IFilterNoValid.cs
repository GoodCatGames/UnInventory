using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.Filters.Response
{
    public interface IFilterNoValid
    {
        ICommandInputData FilterData { get; }
        IFilter Filter { get; }
    }
}
