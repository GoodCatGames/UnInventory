﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.Configuration;
using UnInventory.Core.MVC.Controller.Hand;
using UnInventory.Core.MVC.Model;

namespace UnInventory.Core.Manager
{
    [AddComponentMenu("UI/ Inventory Manager", 30)]
    [DefaultExecutionOrder(-20000)]
    public class InventoryManager : MonoBehaviour, IInventoryManager, IDebug, ISetRootCanvas
    {
        #region Static
        public static RectTransform CanvasRootRectTransform
        {
            get
            {
                var inventoryManager = Get();
                inventoryManager.CheckRootCanvasExist();
                return inventoryManager._rootCanvasRectTransform;
            }
        }

        public static Transform CanvasRoot
        {
            get
            {
                var inventoryManager = Get();
                inventoryManager.CheckRootCanvasExist();
                return inventoryManager._rootCanvasCurrentScene;
            }
        }

        /// <summary>
        /// API
        /// </summary>
        public static IContainerDi ContainerDi => Get()._containerDi;

        /// <summary>
        /// For Override s assemblies (No API)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ContainerDiOverride<T>() where T : ContainerDi => (T)(Get()._containerDi);
                    
        internal static IContainerDiCoreInternal ContainerDiInternal => Get()._containerDi;

        public static IHand Hand => Get()._containerDi.Hand;
        public static InventoryManager Get() => InventoryManagerLazy.Value;

        private static readonly Lazy<InventoryManager> InventoryManagerLazy = new Lazy<InventoryManager>(() =>
        {
            var inventoryManager = FindObjectOfType<InventoryManager>();
            return inventoryManager;
        });
        #endregion

        GameObject IDebug.PointForDebug => BindPrefabs.PointForDebug;

        #region Pre-made
        public float RatioSizeEntityToSlot => ratioSizeEntityToSlot;
        [SerializeField] private float ratioSizeEntityToSlot = 0.85f;

        [SerializeField] public BindPrefabs BindPrefabs = new BindPrefabs();

        public bool DebugMode => _debugMode;
        [UsedImplicitly] [SerializeField] private bool _debugMode = default;
        #endregion

        public IFiltersManager FiltersManager => _containerDi.FiltersManager;
       

        private IContainerDiCoreInternal _containerDi { get; set; }

        private RectTransform _rootCanvasRectTransform;
        private Transform _rootCanvasCurrentScene; 
        
        void ISetRootCanvas.SetRootCanvas(Canvas canvas)
        {
            _rootCanvasCurrentScene = canvas.transform;
            _rootCanvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        private void CheckRootCanvasExist()
        {
            Contract.Assert(_rootCanvasCurrentScene != null,
                $"Canvas not found with {nameof(IsRootCanvasOnScene)} on scene!");
        }
        
        [UsedImplicitly]
        private void Awake()
        {
            var managers = FindObjectsOfType<InventoryManager>();
            if (managers.Count() > 1)
            {
                Destroy(gameObject);
                return;
            }
            
            _containerDi = GetComponent<IContainerDiCoreInternal>();
            _containerDi.Init(BindPrefabs.EntityPrefabStandard);

            DontDestroyOnLoad(this);
        }
    }
}
