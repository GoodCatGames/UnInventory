using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Core.MVC.View.Components
{
    public class InventoryComponent : MonoBehaviour, IDataInventoryContainer, IInventoryStructureContainer
    {
        /// <summary>
        /// Get Inventory SingleOrDefault from root and children transform (Recursive)
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static InventoryComponent GetInventoryComponent(Transform transform)
        {
            var inventoryComponents = transform.GetComponentsRootAndChildren<InventoryComponent>().ToArray();
            if (inventoryComponents.Count() > 1) { throw new Exception("Must be only one Inventory!"); }
            return inventoryComponents.FirstOrDefault();
        }

        public UnityEvent DestroyEvent { get; } = new UnityEvent();

        public Canvas CanvasInventory { get; private set; }
        public RectTransform RectTransform { get; private set; }
        [CanBeNull] public GridLayoutGroup GridLayoutGroup { get; private set; }

        public DataInventory DataInventory => _dataInventory;
        [SerializeField] private DataInventory _dataInventory;
        private IBindComponentToDataDbRead BindComponentToDataDbRead => InventoryManager.ContainerDiInternal.BindComponentToDataDbRead;
        private IBindComponentToDataDbWrite BindComponentToDataDbWrite => InventoryManager.ContainerDiInternal.BindComponentToDataDbWrite;

        public void Init(DataInventory dataInventory)
        {
            SetCanvasInventory();
            SetDataInventory(dataInventory);
        }

        [UsedImplicitly]
        private void Awake()
        {
            SetCanvasInventory();
            _dataInventory.GenerateIdIfNecessary();
            RectTransform = GetComponent<RectTransform>();

            if (_dataInventory.TypeInventory != DataInventory.TypeInventoryEnum.FreeSlots)
            {
                GridLayoutGroup = GetComponent<GridLayoutGroup>();
            }
        }

        [UsedImplicitly]
        private void OnDestroy() => DestroyEvent.Invoke();

        public IEnumerable<ISlotRootComponent> GetSlots() => transform.GetComponentsInChildren<ISlotRootComponent>();

        public DataInventoryStructure GetInventoryStructure()
        {
            var components = GetSlots();
            var dataSlots = components.Select(component => component.Data);
            return new DataInventoryStructure(DataInventory, dataSlots);
        }

        public void SetInventoryStructure(DataInventoryStructure inventoryStructure)
        {
            CheckValidStructure(inventoryStructure);
            SetDataSlots(inventoryStructure.Slots);
            SetDataInventory(inventoryStructure.DataInventory);
        }

        private void CheckValidStructure(DataInventoryStructure inventoryStructure) =>
            Contract.Assert(IsValidStructureForThisInventory(inventoryStructure));


        private bool IsValidStructureForThisInventory(DataInventoryStructure inventoryStructure)
        {
            var inventoryStructureSource = GetInventoryStructure();
            if (!inventoryStructureSource.DataInventory.Equals(inventoryStructure.DataInventory)
                || inventoryStructureSource.Slots.Count != inventoryStructure.Slots.Count)
            {
                return false;
            }
            
            var sourceCoords = GetCoords(inventoryStructureSource);
            var targetCoords = GetCoords(inventoryStructure);

            return  SameSlotsType(inventoryStructureSource, inventoryStructure) &&
                    new HashSet<Vector2Int>(sourceCoords).SetEquals(targetCoords);

            IEnumerable<Vector2Int> GetCoords(DataInventoryStructure structure) => structure.Slots.Select(slot => slot.Vector2Int);

            bool SameSlotsType(DataInventoryStructure first, DataInventoryStructure second) =>
                first.Slots.First().GetType() == second.Slots.First().GetType();
        }

        private ISlotRootComponent GetSlotComponent(IEnumerable<ISlotRootComponent> components, DataSlot dataSlot) =>
            components.SingleOrDefault(component =>
                component.Data.Vector2Int == dataSlot.Vector2Int);

        private void SetCanvasInventory()
        {
            CanvasInventory = GetComponentInParent<Canvas>();
        }

        private void SetDataInventory(DataInventory dataInventory)
        {
            _dataInventory = dataInventory;
            var slotRootComponents = GetSlots();
            foreach (var slotRootComponent in slotRootComponents)
            {
                slotRootComponent.SetInventory(this, dataInventory);
            }
        }

        private void SetDataSlots(IEnumerable<DataSlot> structureSlots)
        {
            var components = GetSlots().ToArray();
            foreach (var dataSlot in structureSlots)
            {
                var slotRootComponent = GetSlotComponent(components, dataSlot);
                slotRootComponent.SetData(dataSlot);
            }
        }

        public override string ToString()
        {
            return DataInventory.Id;  
        }
    }
}