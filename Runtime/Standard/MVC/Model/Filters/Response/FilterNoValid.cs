using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Standard.MVC.Model.Filters.Response
{
    public class FilterNoValid : IFilterNoValid 
    {
        [NotNull]
        public ICommandInputData FilterData { get; }

        [NotNull]
        public IFilter Filter { get; }
        
        public FilterNoValid([NotNull] ICommandInputData filterData, [NotNull] IFilter noValidFilter)
        {
            FilterData = filterData ?? throw new ArgumentNullException(nameof(filterData));
            Filter = noValidFilter ?? throw new ArgumentNullException(nameof(noValidFilter));
        }
    }
}
