using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Causes
{
    public class CauseNoValidFilters : CauseFailureCommand
    {
        public FilterNoValidCollection FilterNoValidNewCollection { get; }

        public CauseNoValidFilters([NotNull] FilterNoValidCollection filterNoValidNewCollection)
        {
            FilterNoValidNewCollection = filterNoValidNewCollection ??
                                         throw new ArgumentNullException(nameof(filterNoValidNewCollection));
        }
    }
}
