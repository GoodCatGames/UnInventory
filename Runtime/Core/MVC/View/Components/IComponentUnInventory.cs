using UnInventory.Core.MVC.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace UnInventory.Core.MVC.View.Components
{
    public interface IComponentUnInventory<TData>
        where TData : class, IData
    {
        InventoryComponent InventoryComponent { get; }
        Transform Transform { get; }
        RectTransform RectTransform { get; }
        
        UnityEvent<TData, TData> SetNewDataEvent { get; }

        TData Data { get; }
        void SetData(TData data);
    }
}
