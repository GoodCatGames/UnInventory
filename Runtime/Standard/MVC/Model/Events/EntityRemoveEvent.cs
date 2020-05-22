using UnityEngine.Events;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;

namespace UnInventory.Standard.MVC.Model.Events
{
    public class EntityRemoveEvent : UnityEvent<RemoveDataAfterExecute>
    {
    }
}
