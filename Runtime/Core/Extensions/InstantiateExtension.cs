using System.Diagnostics.Contracts;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace UnInventory.Core.Extensions
{
    public static class InstantiateExtension
    {
        public static GameObject InstantiateOnRootCanvas(this GameObject prefabInventory)
        {
            Contract.Assert(Manager.InventoryManager.CanvasRoot != null, "Manager.InventoryManager.CanvasRoot == null");
            var canvasMain = Manager.InventoryManager.CanvasRoot;
            var gameObject = Object.Instantiate(prefabInventory, canvasMain);

            return gameObject;
        }
    }
}
