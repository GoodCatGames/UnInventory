using UnityEngine.Events;
using UnInventory.Core.MVC.Model.Filters.Response;

namespace UnInventory.Standard.MVC.Model.Events
{
    public class NoValidFiltersEvent : UnityEvent<IReadOnlyFilterNoValidCollection>
    {
    }
}
