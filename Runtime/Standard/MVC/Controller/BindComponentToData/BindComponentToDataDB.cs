using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Standard.MVC.Controller.BindComponentToData
{
    internal class BindComponentToDataDb : IBindComponentToDataDbWrite, IBindComponentToDataDbRead
    {
        private readonly IDatabaseCommands _databaseCommands;
        private readonly IDatabaseReadOnly _databaseReadOnly;

        private readonly BindComponentToData<ISlotRootComponent, DataSlot> _bindSlots = new BindComponentToData<ISlotRootComponent, DataSlot>();

        private readonly BindComponentToData<IEntityRootComponent, DataEntity> _bindEntityComponentToData =
            new BindComponentToData<IEntityRootComponent, DataEntity>();

        public BindComponentToDataDb(IDatabaseCommands databaseCommands, IDatabaseReadOnly databaseReadOnly)
        {
            _databaseCommands = databaseCommands;
            _databaseReadOnly = databaseReadOnly;
        }

        #region Write
        public void RegisterInventoryInDatabase(DataInventoryStructure inventoryStructure,
            InventoryComponent inventoryComponent)
        {
            RegisterInventoryStructureInDatabase(inventoryStructure);
            inventoryComponent.SetInventoryStructure(inventoryStructure);
            BindSlotsComponentsToData(inventoryComponent);
        }
        
        public void BindInventory(InventoryComponent inventoryComponent, DataInventory dataInventoryInDatabase)
        {
            var inventoryStructure = _databaseReadOnly.GetInventoryStructure(dataInventoryInDatabase);
            inventoryComponent.SetInventoryStructure(inventoryStructure);
            UnBindSlotsComponentsToData(inventoryComponent);
            BindSlotsComponentsToData(inventoryComponent);
        }
        
        public void UnBindInventory(InventoryComponent inventoryComponent)
        {
            UnBindEntities(inventoryComponent);
            UnBindSlotsComponentsToData(inventoryComponent);
        }

        public void BindEntity(IEntityRootComponent entityComponent) => _bindEntityComponentToData.Add(entityComponent);
        public void UnBindEntity(IEntityRootComponent entityComponent) => _bindEntityComponentToData.Remove(entityComponent);

        private void UnBindEntities(InventoryComponent inventoryComponent)
        {
            var entities = GetEntityComponents(inventoryComponent.DataInventory);
            foreach (var entity in entities)
            {
                UnBindEntity(entity);
            }
        }

        private void RegisterInventoryStructureInDatabase(DataInventoryStructure inventoryStructure)
        {
            foreach (var dataSlot in inventoryStructure.Slots)
            {
                _databaseCommands.RegisterSlot(dataSlot);
            }
        }

        private void BindSlotsComponentsToData(InventoryComponent inventoryComponent) =>
            ProcessSlotComponents(inventoryComponent, component => _bindSlots.Add(component));

        private void UnBindSlotsComponentsToData(InventoryComponent inventoryComponent) =>
            ProcessSlotComponents(inventoryComponent, component => _bindSlots.Remove(component));

        private void ProcessSlotComponents(InventoryComponent inventoryComponent, Action<ISlotRootComponent> action)
        {
            var slotRootComponents = inventoryComponent.GetSlots();
            foreach (var slotRootComponent in slotRootComponents)
            {
                action.Invoke(slotRootComponent);
            }
        }
        #endregion

        #region Read
        public InventoryComponent GetInventoryComponent(DataInventory dataInventory)
        {
            var tryGetInventoryComponent = TryGetInventoryComponent(dataInventory, out var componentResult);
            if(!tryGetInventoryComponent) { throw new Exception(); }
            return componentResult;
        }

        public bool TryGetInventoryComponent(DataInventory dataInventory, out InventoryComponent componentResult)
        {
            var slotRootComponent = _bindSlots.Components.FirstOrDefault(component => component.Data.DataInventory == dataInventory);
            componentResult = slotRootComponent?.InventoryComponent;
            return componentResult != null;
        }

        public bool TryGetSlotComponent(DataSlot dataSlot, out ISlotRootComponent componentResult) =>
            _bindSlots.TryGetComponent(dataSlot, out componentResult);
        
        public ISlotRootComponent GetSlotComponent(DataSlot dataSlot) => _bindSlots.GetComponent(dataSlot);

        public IEnumerable<IEntityRootComponent> GetEntityComponents(DataInventory dataInventory)
        {
            var entities = _databaseReadOnly.GetDataEntitiesInventory(dataInventory);
            var components = entities.Select(dataEntity => TryGetEntityComponent(dataEntity, out var result) ? result : null).Where(component => component != null);
            return components;
        }

        public IEntityRootComponent GetEntityComponent(DataEntity dataEntity) => _bindEntityComponentToData.GetComponent(dataEntity);

        public bool TryGetEntityComponent(DataEntity dataEntity, out IEntityRootComponent componentResult) =>
            _bindEntityComponentToData.TryGetComponent(dataEntity, out componentResult);
        
        private DataSlot GetDataSlot(DataInventory dataInventory, Vector2Int coordinate) =>
            _databaseReadOnly.GetSlot(dataInventory, coordinate);
        #endregion
    }
}