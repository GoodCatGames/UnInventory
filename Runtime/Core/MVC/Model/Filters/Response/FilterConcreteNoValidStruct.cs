using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.Filters.Response
{
    public struct FilterConcreteNoValidStruct<TFilter> 
        where TFilter : class, IFilter
    {
        [NotNull] public readonly TFilter Filter;
        [NotNull] public readonly ICommandInputData FilterData;

        public FilterConcreteNoValidStruct([NotNull] TFilter filter, [NotNull] ICommandInputData filterData)
        {
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
            FilterData = filterData ?? throw new ArgumentNullException(nameof(filterData));
        }
    }
}