using System.Collections.Generic;

namespace UnInventory.Core.MVC.Model.Filters.Response
{
    public interface IReadOnlyFilterNoValidCollection : IReadOnlyCollection<IFilterNoValid>
    {
        IReadOnlyList<FilterConcreteNoValidStruct<TFilter>> Get<TFilter>()
            where TFilter : class, IFilter;
    }
}