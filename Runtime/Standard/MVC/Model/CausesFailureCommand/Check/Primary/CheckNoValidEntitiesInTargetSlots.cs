using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckNoValidEntitiesInTargetSlots : CheckCauses
    {
        public readonly DataEntity Entity;
        public readonly DataSlot Slot;
        public readonly int AmountWantPut;
        public readonly IEnumerable<DataEntity> ValidEntities;

        public CheckNoValidEntitiesInTargetSlots(DataEntity entitySource, DataSlot slotLeftTop, IEnumerable<DataEntity> validEntities = null)
        : this(entitySource, slotLeftTop, entitySource.Amount, validEntities)
        {
        }

        public CheckNoValidEntitiesInTargetSlots(DataEntity entitySource, DataSlot slotLeftTop, int amountWantPut, 
            IEnumerable<DataEntity> validEntities = null)
        {
            Entity = entitySource;
            Slot = slotLeftTop;
            AmountWantPut = amountWantPut;
            ValidEntities = validEntities ?? new List<DataEntity>();
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();

            var checkOutBorder = new CheckOutBorder(Entity, Slot);
            result.AddRange(checkOutBorder.GetActualCauses());

            // cant get TargetSlots if IsOutBorder
            if (checkOutBorder.IsActual()) return result;

            var targetSlots = DatabaseReadOnly.GetSlotsForEntityInInventory(Slot.DataInventory, Slot.Vector2Int, Entity).ToList();
            var noValidEntities = DatabaseReadOnly.EntitiesInTargetSlotsWithoutExcluded(Entity, AmountWantPut, targetSlots,
                ValidEntities).ToList();
            
            if (noValidEntities.Count == 1)
            {
                var entity = noValidEntities.Single();
                var cause = new CauseIsSingleNoValidEntityInSlots(entity, targetSlots);
                result.Add(cause);
            }

            if (noValidEntities.Count > 1)
            {
                var cause = new CauseIsMoreOneNoValidEntityInSlots(noValidEntities, targetSlots);
                result.Add(cause);
            }
            return result;
        }
    }
}
