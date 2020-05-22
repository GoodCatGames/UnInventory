using System;
using System.Linq;
using UnInventory.Core.Manager;
using UnityEngine;

namespace UnInventory.Core.Configuration
{
    [RequireComponent(typeof(Canvas))]
    public class IsRootCanvasOnScene : MonoBehaviour
    {
        public Canvas Canvas => GetComponent<Canvas>();

        public void SetInInventoryManager(IInventoryManager inventoryManager)
        {
            var setRootCanvas = (ISetRootCanvas)inventoryManager;
            setRootCanvas.SetRootCanvas(Canvas);
        }

        private void Awake()
        {
            CheckSingle();
            CheckInventoryManagerExist();
            SetInInventoryManager(InventoryManager.Get());
        }

        private void CheckSingle()
        {
            var rootCanvasOnScenes = FindObjectsOfType<IsRootCanvasOnScene>();
            if (rootCanvasOnScenes.Count() > 1) throw new Exception("Should be only one Root Canvas!"); 
        }

        private void CheckInventoryManagerExist()
        {
            if (InventoryManager.Get() == null)
            {
                throw new Exception("InventoryManager not found!");
            }
        }
    }
}