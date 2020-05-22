using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnityEngine.Events;

namespace UnInventory.Core.InventoryBindings
{
    public interface IInventoryBinding : IDataInventoryContainer
    {
        State State { get; }
        
        IInventoryBinding Bind([NotNull] InventoryComponent inventoryComponent);
        IInventoryBinding UnBind();
        UnityEvent<State> ChangeState { get; }
    }
}