using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.Model.DataBase.Events;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Standard.MVC.Model.DataBase
{
    internal class DataBase : IDatabaseNotifier, IDatabaseReadOnly, IDatabaseCommands
    {
        public IComparer<DataEntity> ComparerEntitiesDefault { get; }
        public IComparer<DataSlot> ComparerSlotsDefault { get; } = new ComparerSlots();

        private readonly Dictionary<DataSlot, DataEntity> _dictionaryBindSlotToEntity =
            new Dictionary<DataSlot, DataEntity>();

        /// <summary>
        /// Sorted used ComparerSlotsDefault
        /// </summary>
        public IReadOnlyCollection<DataSlot> Slots => _slots;
        private readonly HashSet<DataSlot> _slots = new HashSet<DataSlot>();

        /// <summary>
        /// Sorted used ComparerEntitiesDefault
        /// </summary>
        public IReadOnlyCollection<DataEntity> Entities => _entities;
        private readonly HashSet<DataEntity> _entities = new HashSet<DataEntity>();
        
        public DataBase()
        {
            ComparerEntitiesDefault = new ComparerEntitiesForComparerSlots(this, ComparerSlotsDefault);
        }
        
        // Events
        public UnityEvent<DataEntity> CreateEvent { get; } = new DatabaseCreateEvent();
        public UnityEvent<DataEntity> ChangePositionEvent { get; } = new DatabaseChangePositionEvent();
        public UnityEvent<DataEntity> RemoveEvent { get; } = new DatabaseRemoveEvent();

        #region Commands

        /// <inheritdoc />
        /// <summary>
        /// Create DataEntity in Database on slot
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="toSlotLeftTop"></param>
        public void CreateEntityInSlot(DataEntity entity, DataSlot toSlotLeftTop)
        {
            if (!SlotIsFree(toSlotLeftTop) && GetEntityOrNull(toSlotLeftTop) != entity)
            {
                throw new Exception("Slot must be empty!");
            }
            RegisterEntity(entity);
            BindEntityToSlots(toSlotLeftTop.DataInventory, entity, toSlotLeftTop);

            CreateEvent.Invoke(entity);
        }

        public void RemoveEntity(DataEntity entity)
        {
            UnRegisterEntity(entity);
            UnbindEntityFromSlots(entity);
            RemoveEvent.Invoke(entity);
        }
        
        public void MoveEntityInSlot(DataEntity entity, DataSlot toSlotLeftTop)
        {
            var inventory = toSlotLeftTop.DataInventory;
           
            if (GetSlotOrNull(entity) == toSlotLeftTop)
            {
                return;
            }
            
            var slotIsFreeForEntity = SlotIsFreeForEntity(toSlotLeftTop, entity, entity.Amount);
            if (!slotIsFreeForEntity)
            {
                throw new Exception("The slot should be free!");
            }

            UnbindEntityFromSlots(entity);
            BindEntityToSlots(inventory, entity, toSlotLeftTop);
            ChangePositionEvent.Invoke(entity);
        }

        #endregion

        public void RegisterSlot(DataSlot slot) => _slots.Add(slot);

        #region Requests
        public DataInventoryStructure GetInventoryStructure(DataInventory inventory)
        {
            var slotsInventory = GetSlotsInventory(inventory);
            return new DataInventoryStructure(inventory, slotsInventory);
        }

        public IEnumerable<DataSlot> GetSlotsForEntityInInventory(DataEntity dataEntity, DataSlot slot)
            => GetSlotsForEntityInInventory(slot.DataInventory, slot.Vector2Int, dataEntity);
        
        public IEnumerable<DataSlot> GetSlotsForEntityInInventory(DataInventory inventory, Vector2Int position,
            DataEntity dataEntity)
        {
            var result = new List<DataSlot>();
            var isOutBorderInventory = IsOutBorderInventory(inventory, dataEntity, position);
            Contract.Assert(!isOutBorderInventory, "IsOutBorderInventory!");
           if (inventory.TypeInventory == DataInventory.TypeInventoryEnum.GridSupportMultislotEntity)
            {
                result = GetSlots(inventory, dataEntity.GetArea(position)).ToList();
            }
            else
            {
                result.Add(GetSlotOrNull(inventory, position));
            }
            return result;
        }

        public IEnumerable<DataSlot> GetSlots(DataInventory inventory, IEnumerable<Vector2Int> coordinates)
        {
            return coordinates.Select(coordinate => GetSlotOrNull(inventory, coordinate)).Distinct().Where(slot => slot != null);
        }

        public DataSlot GetSlot(DataInventory dataInventory, Vector2 position) => _slots.Single(slot =>
            slot.DataInventory == dataInventory && slot.Vector2Int == position);
        
        public DataSlot GetSlotOrNull(DataEntity entity) => _dictionaryBindSlotToEntity
            .OrderBy(pair => pair.Key, ComparerSlotsDefault)
            .FirstOrDefault(pair => pair.Value == entity).Key;
        
        public DataSlot GetSlotOrNull(DataInventory inventory,
            Vector2Int position)
        {
            return _slots.FirstOrDefault(sl =>
                sl.DataInventory == inventory &&
                sl.Column == position.x &&
                sl.Row == position.y);
        }

        public IReadOnlyDictionary<DataEntity, Vector2Int> GetDataEntitiesWithPositions(DataInventory inventory)
        {
            var dataEntities = GetDataEntitiesInventory(inventory);
            return dataEntities.ToDictionary(dataEntity => dataEntity, dataEntity => GetSlotOrNull(dataEntity).Vector2Int);
        }

        public IEnumerable<DataEntity> GetDataEntitiesInventory(DataInventory inventory)
        {
            return new List<DataEntity>(_entities.Where(entity => entity.DataInventory == inventory).Select(ent => ent));
        }

        public bool SlotIsFree(DataSlot slot) => GetEntityOrNull(slot) == null;
        
        public IEnumerable<DataSlot> SlotsFree(DataInventory inventory) => SlotsFree().Where(slot => slot.DataInventory == inventory);

        public IEnumerable<DataEntity> GetEntitiesWithId(DataInventory inventory, string id) =>
            _entities.Where(entity => entity.DataInventory == inventory && entity.Id == id)
                .OrderBy(entity => entity, ComparerEntitiesDefault);

        public IEnumerable<DataEntity> GetEntities(IEnumerable<DataSlot> slots) =>
            slots.Where(slot => slot != null)
                .Select(GetEntityOrNull).Distinct()
                .Where(entity => entity != null);

        public DataEntity GetEntityOrNull(DataSlot slot) => _dictionaryBindSlotToEntity.FirstOrDefault(pair => pair.Key == slot).Value;
        
        public IEnumerable<DataEntity> EntitiesInTargetSlotsWithoutExcluded(DataEntity entitySource,  int amountTransferred,
            IEnumerable<DataSlot> targetSlots, IEnumerable<DataEntity> excludingDataEntities = null)
        {
            var targetSlotsWithoutFree = targetSlots.Where((slot => !SlotIsFreeForEntity(slot, entitySource, amountTransferred)));
            var entitiesInTargetSlotsWithoutSource = GetEntities(targetSlotsWithoutFree);
            if (excludingDataEntities != null)
            {
                entitiesInTargetSlotsWithoutSource = entitiesInTargetSlotsWithoutSource.Where(entity => !excludingDataEntities.Contains(entity));
            }
            return entitiesInTargetSlotsWithoutSource;
        }
        
        public bool IsAreaEntityForInventory(IEntityRootComponent entitySource, DataInventory inventoryTo)
        {
            return entitySource.Data.IsAreaEntity &&
                   inventoryTo.TypeInventory == DataInventory.TypeInventoryEnum.GridSupportMultislotEntity;
        }

        public bool IsEntitiesMayBeStack(DataEntity entitySource, DataEntity otherEntity)
        {
            return otherEntity.IsMayBeStack && entitySource.IsMayBeStack &&
                   otherEntity.Id == entitySource.Id;
        }

        public bool IsOutBorderInventory(DataInventory inventory, DataEntity dataEntity, Vector2Int coordinateSlotLeftTop)
        {
            if (inventory.TypeInventory != DataInventory.TypeInventoryEnum.GridSupportMultislotEntity)
            {
                return false;
            }
            var targetSlots = GetSlots(inventory, dataEntity.GetArea(coordinateSlotLeftTop));
            int countSlotsArea = dataEntity.Dimensions.x * dataEntity.Dimensions.y;
            return countSlotsArea > targetSlots.Count();
        }

        private List<DataSlot> GetSlotsInventory(DataInventory dataInventory) =>
            _slots.Where(slot => slot.DataInventory == dataInventory).ToList();

        /// <summary>
        /// free slots sorted accordingly ComparerSlots
        /// </summary>
        /// <returns></returns>
        private IEnumerable<DataSlot> SlotsFree()
        {
            var freeSlots = _slots.Where(SlotIsFree).OrderBy(slot => slot, ComparerSlotsDefault).ToArray();
            if (!freeSlots.Any())
            {
                throw new Exception("No free slot!");
            }
            return freeSlots;
        }

        /// <summary>
        /// The slot is free for DataEntity, taking into account the portable number.
        /// If any number should remain, then the current position is not considered free
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="dataEntity"></param>
        /// <param name="amountTransferred">Кол-во переносимое</param>
        /// <returns></returns>
        private bool SlotIsFreeForEntity([NotNull] DataSlot slot, [NotNull] DataEntity dataEntity, int amountTransferred)
        {
            if (amountTransferred <= 0) throw new ArgumentOutOfRangeException(nameof(amountTransferred));
            
            if (amountTransferred > dataEntity.AmountMax)
            {
                throw new Exception("AmountTransferred can t more AmountMax");
            }
            var entityInSlot = GetEntityOrNull(slot);
            return SlotIsFree(slot) || (entityInSlot == dataEntity && entityInSlot.Amount == amountTransferred);
        }

        #endregion

        private void BindEntityToSlots(DataInventory inventory, DataEntity entity, DataSlot slotLeftTop)
        {
            var slots = GetSlotsForEntityInInventory(inventory, slotLeftTop.Vector2Int, entity);

            UnbindEntityFromSlots(entity);

            foreach (var slot in slots)
            {
                _dictionaryBindSlotToEntity.Add(slot, entity);
            }
            ((IDataInventorySetter)entity).SetDataInventory(slotLeftTop.DataInventory);
        }

        public void UnbindEntityFromSlots(DataEntity entity)
        {
            var slotsForDelete = _dictionaryBindSlotToEntity.Where(pair => pair.Value == entity).Select(pair => pair.Key).ToList();
            slotsForDelete.ForEach(slot => _dictionaryBindSlotToEntity.Remove(slot));
            ((IDataInventorySetter)entity).SetDataInventory(null);
        }

        private void RegisterEntity(DataEntity item) => _entities.Add(item);

        private void UnRegisterEntity(DataEntity item) => _entities.Remove(item);
    }
}