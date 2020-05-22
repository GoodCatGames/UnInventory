using UnityEngine.Events;

namespace UnInventory.Core.InventoryBindings
{
    public enum State
    {
        Bind,
        UnBind
    }

    public class ChangeStateEvent : UnityEvent<State>
    {
    }
}