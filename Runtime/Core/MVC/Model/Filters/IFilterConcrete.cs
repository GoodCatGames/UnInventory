using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.Filters
{
    public interface IFilterConcrete<in T> : IFilter
        where T : ICommandInputData
    {
        bool Validate(T data);
    }
}
