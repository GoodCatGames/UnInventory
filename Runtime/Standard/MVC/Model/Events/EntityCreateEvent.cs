using UnityEngine.Events;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;

namespace UnInventory.Standard.MVC.Model.Events
{
    public class EntityCreateEvent : UnityEvent<CreateDataAfterExecute>
    {
    }
}
