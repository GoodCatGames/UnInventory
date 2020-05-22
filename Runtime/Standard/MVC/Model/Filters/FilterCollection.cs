using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Filters;

namespace UnInventory.Standard.MVC.Model.Filters
{
    public class FilterCollection : Collection<IFilter>, IFilterCollection
    {
        public FilterCollection()
        {
        }

        public FilterCollection([NotNull] IList<IFilter> list) : base(list)
        {
        }

        public IFilterCollectionConcrete<T> GetFilterCollectionConcrete<T>() where T : class, ICommandInputData
        {
            var filtersTType = this.OfType<IFilterConcrete<T>>().ToList();
            return new FilterCollectionConcrete<T>(filtersTType);
        }
    }
}
