using UnityEngine.Events;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.Model.DataBase
{
    public interface IDatabaseNotifier
    {
        UnityEvent<DataEntity> CreateEvent { get; }
        UnityEvent<DataEntity> RemoveEvent { get; }
        UnityEvent<DataEntity> ChangePositionEvent { get; }
    }
}
