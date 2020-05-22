using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    // Dummy
    public class FilterDummyBodyPart :
        IFilterCreate,
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        [NotNull] private readonly IDataInventoryContainer _dummy;

        public FilterDummyBodyPart([NotNull] IDataInventoryContainer dummy)
        {
            _dummy = dummy ?? throw new ArgumentNullException(nameof(dummy));
        }

        public bool Validate(CreateInputData data) =>
            ValidateBodyPart(data.Slot.DataInventory, data.DataEntity, data.Slot);

        public bool Validate(MoveInputData data) =>
            ValidateBodyPart(data.ToInventory, data.EntitySource, data.SlotTo);

        // stack denied in Dummy
        public bool Validate(StackInputData data) => data.ToInventory != _dummy.DataInventory;

        public bool Validate(SwapPrimaryInputData data) =>
            ValidateBodyPart(data.ToInventory, data.EntitySource, data.SlotTo)
            && ValidateBodyPart(data.FromInventory, data.EntityTarget, data.SlotNewPositionEntityTarget);

        private bool ValidateBodyPart(DataInventory inventory, DataEntity dataEntity, DataSlot dataSlot)
        {
            if (inventory != _dummy.DataInventory) return true;
            var dataEntityEquipment = dataEntity as DataEntityEquipment;
            var dataSlotDummy = dataSlot as DataSlotDummy;
            var result = dataEntityEquipment?.BodyPart == dataSlotDummy?.BodyPart;
            return result;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}