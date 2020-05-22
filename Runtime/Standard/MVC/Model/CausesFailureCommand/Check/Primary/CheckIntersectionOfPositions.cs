using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckIntersectionOfPositions : CheckCauses
    {
        public readonly DataEntity First, Second;
        public readonly DataSlot SlotFirst, SlotSecond;

        public CheckIntersectionOfPositions(DataEntity first, DataSlot slotFirst, DataEntity second, DataSlot slotSecond)
        {
            First = first;
            Second = second;
            SlotFirst = slotFirst;
            SlotSecond = slotSecond;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();

            var checkOutBorderFirst = new CheckOutBorder(First, SlotFirst);
            var checkOutBorderSecond = new CheckOutBorder(Second, SlotSecond);

            result.AddRange(checkOutBorderFirst.GetActualCauses());
            result.AddRange(checkOutBorderSecond.GetActualCauses());

            if (checkOutBorderFirst.IsActual() || checkOutBorderSecond.IsActual()) return result;

            var slotsNewPositionEntity = DatabaseReadOnly.GetSlotsForEntityInInventory(First, SlotFirst).ToArray();
            var slotsNewPositionOtherEntity = DatabaseReadOnly.GetSlotsForEntityInInventory(Second, SlotSecond).ToArray();
            var isIntersection = slotsNewPositionEntity.Intersect(slotsNewPositionOtherEntity).Any();
            if (isIntersection)
            {
                result.Add(new CauseIntersectionOfPositions(First, SlotFirst, Second, SlotSecond));
            }
            return result;
        }
    }
}
