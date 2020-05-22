using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Move
{
    public class MoveInputData : ChangeInputData
    {
        public bool IsFullStackMove => !IsCreatedNewEntity;
        protected readonly bool IsCreatedNewEntity;
        public readonly int AmountWantPut;

        public MoveInputData([NotNull] DataEntity entitySource, [NotNull] DataSlot slotLeftTop, int amountWantPut)
            : base(entitySource, slotLeftTop)
        {
            if (amountWantPut <= 0) throw new ArgumentOutOfRangeException(nameof(amountWantPut));
            AmountWantPut = amountWantPut;
            IsCreatedNewEntity = AmountSource != AmountWantPut;
        }
    }
}
