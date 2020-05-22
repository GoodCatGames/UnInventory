using UnityEngine.Events;

namespace UnInventory.Core.Extensions
{
    public static class RxSimple
    {
        public static void SetValueInvokeChangeEvent<T>(ref T field, T newValue, UnityEvent dataChangeEvent)
        {
            var oldValue = field;
            if (Equals(oldValue, newValue)) return;
            field = newValue;
            dataChangeEvent.Invoke();
        }
    }
}
