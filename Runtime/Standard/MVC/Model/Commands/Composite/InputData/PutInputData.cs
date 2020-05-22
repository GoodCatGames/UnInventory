using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Composite.InputData
{
    public class PutInputData : ICommandInputData
    {
        public DataEntity EntitySource { get; }
        public DataSlot SlotLeftTop { get; }
        public int AmountWantPut { get; }

        public PutInputData([NotNull] DataEntity entitySource, [NotNull] DataSlot slotLeftTop, int amountWantPut)
        {
            if (amountWantPut <= 0) throw new ArgumentOutOfRangeException(nameof(amountWantPut));
            EntitySource = entitySource;
            SlotLeftTop = slotLeftTop;
            AmountWantPut = amountWantPut;
        }
    }
}
