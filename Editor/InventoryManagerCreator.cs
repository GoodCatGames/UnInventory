﻿using System;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.Configuration;
using UnInventory.Core.Extensions;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.View.Components.Slot;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnInventory.Editor
{
    public static class InventoryManagerCreator
    {
        [UsedImplicitly]
        [MenuItem("GameObject/UnInventory/Create InventoryManager", false, 0)]
        public static void CreateInventoryManager()
        {
            var prefab = (GameObject) Resources.Load("Prefabs/InventoryManager", typeof(GameObject));
            var instantiate = Object.Instantiate(prefab);
            var inventoryManager = instantiate.GetComponent<IInventoryManager>();
            try
            {
                CheckSingleton();
                instantiate.name = "InventoryManager";
                Selection.activeObject = instantiate;
                var isRootCanvasOnScene = GetOrAssignCanvasInventoryManagerIfNecessary();
                isRootCanvasOnScene.SetInInventoryManager(inventoryManager);
            }
            catch (Exception e)
            {
                Object.DestroyImmediate(instantiate);
                Console.WriteLine(e);
                throw;
            }
        }

        public static Canvas GetCanvasInventoryManager()
        {
            var canvases = Object.FindObjectsOfType<IsRootCanvasOnScene>();
            if (canvases.Length > 1)
            {
                throw new Exception("There should be only one Root Canvas!");
            }
            return canvases.Select(go => go.Canvas).FirstOrDefault();
        }

        private static void CheckSingleton()
        {
            var managers = Object.FindObjectsOfType<InventoryManager>();
            if (managers.Length > 1)
            {
                throw new Exception("There should be only one Inventory Manager!");
            }
        }

        private static IsRootCanvasOnScene GetOrAssignCanvasInventoryManagerIfNecessary()
        {
            var canvasInventoryManager = GetCanvasInventoryManager();
            if (canvasInventoryManager == null)
            {
                var canvases = CanvasExt.GetRootSceneCanvases().ToArray();
                if (canvases.Count() != 0)
                {
                    return canvases.First().gameObject.AddComponent<IsRootCanvasOnScene>();
                }
                else
                {
                    throw new Exception("Canvas not found on scene!");
                }
            }
            return canvasInventoryManager.GetComponent<IsRootCanvasOnScene>();
        }
    }
}