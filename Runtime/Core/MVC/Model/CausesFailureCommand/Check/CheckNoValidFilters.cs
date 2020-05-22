using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Check
{
    public class CheckNoValidFilters : CheckCauses
    {
        public FilterNoValidCollection FilterNoValidNewCollection { get; }

        public CheckNoValidFilters([NotNull] FilterNoValidCollection filterNoValidNewCollection)
        {
            FilterNoValidNewCollection = filterNoValidNewCollection ??
                                         throw new ArgumentNullException(nameof(filterNoValidNewCollection));
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            if (FilterNoValidNewCollection.Any())
            {
                result.Add(new CauseNoValidFilters(FilterNoValidNewCollection));
            }
            return result;
        }
    }
}
