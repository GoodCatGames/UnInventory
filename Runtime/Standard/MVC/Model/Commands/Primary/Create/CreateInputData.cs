using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Create
{
    public class CreateInputData : ICommandInputData
    {
        public DataInventory Inventory => DataEntity.DataInventory;
        [NotNull] public DataEntity DataEntity { get; }
        public int Amount { get; }
        [NotNull] public DataSlot Slot { get; }
        [NotNull] public readonly IEnumerable<DataSlot> Slots;

        public CreateInputData([NotNull] DataEntity dataEntity, [NotNull] DataSlot slot)
        {
            Amount = dataEntity.Amount;
            if (Amount <= 0) throw new ArgumentOutOfRangeException(nameof(Amount));

            DataEntity = dataEntity;
            Slot = slot;
            var slots = InventoryManager.ContainerDi.DatabaseReadOnly.GetSlotsForEntityInInventory(DataEntity, slot);
            Slots = slots;
        }

        public override string ToString() => $"Create: {DataEntity}: {DataEntity.Amount}, {Slot}";
    }
}
