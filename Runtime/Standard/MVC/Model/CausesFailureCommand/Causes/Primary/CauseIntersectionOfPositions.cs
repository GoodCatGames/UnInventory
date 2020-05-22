using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary
{
    public class CauseIntersectionOfPositions : CauseFailureCommand
    {
        [NotNull] public readonly DataEntity First, Second;
        [NotNull] public readonly DataSlot SlotFirst, SlotSecond;

        public CauseIntersectionOfPositions([NotNull] DataEntity first,
            [NotNull] DataSlot slotFirst, [NotNull] DataEntity second, [NotNull] DataSlot slotSecond)
        {
            First = first;
            Second = second;
            SlotFirst = slotFirst;
            SlotSecond = slotSecond;
        }
    }
}
