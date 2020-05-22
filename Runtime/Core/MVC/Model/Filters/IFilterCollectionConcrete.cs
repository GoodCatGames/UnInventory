using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Model.Filters
{
    public interface IFilterCollectionConcrete<T> : IList<IFilterConcrete<T>>
        where T : class, ICommandInputData
    {
        bool Validate(T data, out List<IFilterNoValid> noValidFiltersResponses);
    }
}