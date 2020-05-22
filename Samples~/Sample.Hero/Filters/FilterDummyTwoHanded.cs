using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    public class FilterDummyTwoHanded :
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;
        [NotNull] private readonly IDataInventoryContainer _dummy;

        public FilterDummyTwoHanded([NotNull] IDataInventoryContainer dummy)
        {
            _dummy = dummy;
        }

        public bool Validate(MoveInputData data)
        {
            return ValidateEntityTwoHanded(data.SlotTo, data.EntitySource)
                && ValidateEntityOneHanded(data.SlotTo, data.EntitySource);
        }

        public bool Validate(StackInputData data)
        {
            return ValidateEntityTwoHanded(data.SlotTo, data.EntitySource)
                   && ValidateEntityOneHanded(data.SlotTo, data.EntitySource);

        }

        public bool Validate(SwapPrimaryInputData data)
        {
            return ValidateEntityTwoHanded(data.SlotTo, data.EntitySource)
                   && ValidateEntityOneHanded(data.SlotTo, data.EntitySource)
                   && ValidateEntityTwoHanded(data.SlotFrom, data.EntityTarget)
                   && ValidateEntityOneHanded(data.SlotFrom, data.EntityTarget);
        }

        /// <summary>
        /// If we move a two-handed item and there is something in the second hand - refusal
        /// </summary>
        /// <param name="slotTarget"></param>
        /// <param name="entityTwoHanded"></param>
        /// <returns></returns>
        private bool ValidateEntityTwoHanded(DataSlot slotTarget, DataEntity entityTwoHanded)
        {
            if (slotTarget.DataInventory == _dummy.DataInventory 
                && entityTwoHanded is DataEntityEquipment equipment
                && equipment.BodyPart == DataSlotDummy.BodyPartEnum.Hand
                && equipment.IsTwoHanded)
            {
                var slotSource = DatabaseReadOnly.GetSlotOrNull(entityTwoHanded);

                // исключаем SourceSlot и TargetSlot
                var slotsHand = GetSlotsHand();
                slotsHand = slotsHand.Except(new List<DataSlot>() { slotSource, slotTarget });
                
                var secondHandIsFull = slotsHand.Any(slot => !DatabaseReadOnly.SlotIsFree(slot));
                return !secondHandIsFull;
            }
            return true;
        }

        /// <summary>
        /// If we move a one-handed item and there is a two-handed item in the second hand - refusal 
        /// </summary>
        /// <param name="slotTarget"></param>
        /// <param name="entityOneHanded"></param>
        /// <returns></returns>
        private bool ValidateEntityOneHanded(DataSlot slotTarget, DataEntity entityOneHanded)
        {
            if (slotTarget.DataInventory == _dummy.DataInventory
                && entityOneHanded is DataEntityEquipment equipment
                && equipment.BodyPart == DataSlotDummy.BodyPartEnum.Hand
                && !equipment.IsTwoHanded
                )
            {
                var slotsHand = GetSlotsHand();
                var slotSource = DatabaseReadOnly.GetSlotOrNull(entityOneHanded);

                // исключаем SourceSlot и TargetSlot
                slotsHand = slotsHand.Except(new List<DataSlot>() {slotSource, slotTarget});

                var secondHandIsTwoHandedEntity = slotsHand.
                    Any(slot => 
                        (DatabaseReadOnly.GetEntityOrNull(slot) is DataEntityEquipment equipmentInSlot && equipmentInSlot.IsTwoHanded)
                    );
                return !secondHandIsTwoHandedEntity;
            }
            return true;
        }

        private IEnumerable<DataSlot> GetSlotsHand()
        {
            return DatabaseReadOnly.GetInventoryStructure(_dummy.DataInventory).Slots.OfType<DataSlotDummy>()
                .Where(slot => slot.BodyPart == DataSlotDummy.BodyPartEnum.Hand);
        }
    }
}
