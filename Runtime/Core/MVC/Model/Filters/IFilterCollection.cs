using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.Filters
{
    public interface IFilterCollection : IList<IFilter>
    {
        IFilterCollectionConcrete<T> GetFilterCollectionConcrete<T>() where T : class, ICommandInputData;
    }
}