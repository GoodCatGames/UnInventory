using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;

namespace UnInventory.Standard.MVC.Model.Commands.Primary
{
    public abstract class ChangeInputData : ICommandInputData
    {
        public bool IsInsideSomeInventory => FromInventory == ToInventory;

        public DataInventory[] Inventories => new[] { FromInventory, ToInventory };
        [NotNull] public DataInventory FromInventory => SlotFrom.DataInventory;
        [NotNull] public DataInventory ToInventory => SlotTo.DataInventory;

        public DataSlot[] Slots => new[] { SlotFrom, SlotTo };
        [NotNull] public readonly DataSlot SlotFrom;
        [NotNull] public readonly DataSlot SlotTo;

        public int AmountSource { get; }

        [NotNull] public readonly DataEntity EntitySource;

        protected IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;
        
        /// <summary>
        /// Only for Stack!
        /// </summary>
        /// <param name="dataEntitySource"></param>
        /// <param name="entityTo"></param>
        protected ChangeInputData([NotNull] DataEntity dataEntitySource, [NotNull] DataEntity entityTo)
        {
            SlotTo = DatabaseReadOnly.GetSlotOrNull(entityTo); 
            EntitySource = dataEntitySource;
            AmountSource = EntitySource.Amount;
            SlotFrom = DatabaseReadOnly.GetSlotOrNull(EntitySource);
        }

        protected ChangeInputData([NotNull] DataEntity dataEntitySource, [NotNull] DataSlot slotTo)
        {
            SlotTo = slotTo;
            EntitySource = dataEntitySource;
            AmountSource = EntitySource.Amount;
            SlotFrom = DatabaseReadOnly.GetSlotOrNull(EntitySource);
        }
        
        public override string ToString()
        {
            return $"Change: ";
        }
    }
}
