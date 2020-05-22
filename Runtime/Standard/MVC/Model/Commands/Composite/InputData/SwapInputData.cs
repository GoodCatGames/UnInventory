using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary;

namespace UnInventory.Standard.MVC.Model.Commands.Composite.InputData
{
    public class SwapInputData : ChangeInputData
    {
        public DataSlot SlotOldPositionEntityDisplacing { get; }
        public DataEntity EntityTarget { get; }
        [NotNull] public IComparer<DataSlot> Comparer { get; }
        
        public SwapInputData([NotNull] DataEntity entityDisplacing,
            [NotNull] DataSlot slotNewPositionEntityDisplacing, [NotNull] DataEntity entityTarget, [CanBeNull] IComparer<DataSlot> comparer = null)
        : base(entityDisplacing, slotNewPositionEntityDisplacing)
        {
            SlotOldPositionEntityDisplacing = DatabaseReadOnly.GetSlotOrNull(entityDisplacing);
            EntityTarget = entityTarget;
            Comparer = comparer ?? DatabaseReadOnly.ComparerSlotsDefault;
        }
    }
}
