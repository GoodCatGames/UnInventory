using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace UnInventory.Core.MVC.Model.Filters.Response
{
    public class FilterNoValidCollection : Collection<IFilterNoValid>, IReadOnlyFilterNoValidCollection
    {
        public FilterNoValidCollection()
        {
        }

        public FilterNoValidCollection([NotNull] IList<IFilterNoValid> list) : base(list)
        {
        }
        
        public IReadOnlyList<FilterConcreteNoValidStruct<TFilter>> Get<TFilter>()
            where TFilter : class, IFilter
        {
            return this.Where(noValidFilter => noValidFilter.Filter is TFilter)
                .Select(concrete =>
                    new FilterConcreteNoValidStruct<TFilter>((TFilter) concrete.Filter, concrete.FilterData)).ToList();
        }
        
        public override string ToString()
        {
            string result = "";
            foreach (var filterNoValid in this)
            {
                result += filterNoValid.ToString();
            }
            return result;
        }
    }
}
