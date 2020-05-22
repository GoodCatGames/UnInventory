using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Core.MVC.Model.Filters.Response;
using UnInventory.Standard.MVC.Model.Filters.Response;

namespace UnInventory.Standard.MVC.Model.Filters
{
    public class FilterCollectionConcrete<T> : Collection<IFilterConcrete<T>>, 
        IFilterCollectionConcrete<T> where T : class, ICommandInputData
    {
        public FilterCollectionConcrete()
        {
        }

        public FilterCollectionConcrete([NotNull] IList<IFilterConcrete<T>> list) : base(list)
        {
        }
        
        public bool Validate(T data, out List<IFilterNoValid> noValidFiltersResponses) 
        {
            var noValidFilters = this.Where(filter => !filter.Validate(data)).ToList();
            noValidFiltersResponses = noValidFilters.Select(concrete => new FilterNoValid(data, concrete)).Cast<IFilterNoValid>().ToList();
            return !noValidFiltersResponses.Any();
        }
    }
}
