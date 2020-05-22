using UnInventory.Core.MVC.Model.Data;
using UnityEngine;

namespace UnInventory.Standard
{
    public interface IInventoryOpenCloseObject : IDataInventoryContainer
    {
        bool IsOpen { get; }
        Transform Transform { get; }
        void OpenClose(Vector2? position = null);
        void Open(Vector2? position = null);
        void Close();
    }
}