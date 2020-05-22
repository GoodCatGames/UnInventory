using System.Collections.Generic;
using UnityEngine;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Core.MVC.Model.DataBase
{
    public interface IDatabaseReadOnly
    {
        IReadOnlyCollection<DataSlot> Slots { get; }
        IReadOnlyCollection<DataEntity> Entities { get; }
        IComparer<DataSlot> ComparerSlotsDefault { get; }

        /// <summary>
        /// Returns the slots for the DataEntity position given the type InventoryComponent
        /// (If Areas Entity is not supported, one slot will always be returned)
        /// </summary>
        /// <param name="dataEntity"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        IEnumerable<DataSlot> GetSlotsForEntityInInventory(DataEntity dataEntity, DataSlot slot);

        /// <summary>
        /// Returns the slots for the DataEntity position given the type InventoryComponent
        /// (If Areas Entity is not supported, one slot will always be returned)
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="position"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        IEnumerable<DataSlot> GetSlotsForEntityInInventory(DataInventory inventory, Vector2Int position, DataEntity dataEntity);

        IEnumerable<DataSlot> GetSlots(DataInventory inventory, IEnumerable<Vector2Int> coordinates);

        DataSlot GetSlot(DataInventory dataInventory, Vector2 position);

        DataSlot GetSlotOrNull(DataEntity entity);

        DataSlot GetSlotOrNull(DataInventory inventory,
            Vector2Int position);

        IReadOnlyDictionary<DataEntity, Vector2Int> GetDataEntitiesWithPositions(DataInventory inventory);

        IEnumerable<DataEntity> GetDataEntitiesInventory(DataInventory inventory);

        IEnumerable<DataSlot> SlotsFree(DataInventory inventory);

        IEnumerable<DataEntity> GetEntitiesWithId(DataInventory inventory, string id);

        IEnumerable<DataEntity> GetEntities(IEnumerable<DataSlot> slots);

        DataEntity GetEntityOrNull(DataSlot slot);

        bool IsAreaEntityForInventory(IEntityRootComponent entitySource, DataInventory inventoryTo);

        bool IsEntitiesMayBeStack(DataEntity entitySource, DataEntity otherEntity);

        bool IsOutBorderInventory(DataInventory inventory, DataEntity dataEntity, Vector2Int coordinateSlotLeftTop);
        
        /// <summary>
        /// Returns a DataEntity without considering the entitySource given the taken number
        /// see SlotIsFreeForEntity ()
        /// </summary>
        /// <param name="entitySource"></param>
        /// <param name="amountTransferred"></param>
        /// <param name="targetSlots"></param>
        /// <param name="excludingDataEntities"></param>
        /// <returns></returns>
        IEnumerable<DataEntity> EntitiesInTargetSlotsWithoutExcluded(DataEntity entitySource,  int amountTransferred,
            IEnumerable<DataSlot> targetSlots, IEnumerable<DataEntity> excludingDataEntities = null);

        bool SlotIsFree(DataSlot slot);

        DataInventoryStructure GetInventoryStructure(DataInventory inventory);
    }
}