using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary
{
    public class SwapPrimaryInputData : ChangeInputData
    {
        public DataSlot SlotNewPositionEntityTarget { get; }
        public DataSlot SlotOldPositionEntityDisplacing { get; }
        public DataEntity EntityTarget { get; }
        
        public SwapPrimaryInputData([NotNull] DataEntity entityDisplacing, [NotNull] DataSlot slotNewPositionEntityDisplacing,
            [NotNull] DataEntity entityTarget, [NotNull] DataSlot slotNewPositionEntityTarget) : base(entityDisplacing, slotNewPositionEntityDisplacing)
        {
            SlotNewPositionEntityTarget = slotNewPositionEntityTarget;
            SlotOldPositionEntityDisplacing = DatabaseReadOnly.GetSlotOrNull(entityDisplacing);
            EntityTarget = entityTarget;
        }
    }
}
