using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Stack
{
    public class StackInputData : ChangeInputData
    {
        public readonly bool IsFullStackMove;

        [NotNull] public DataEntity ToDataEntity { get; }
        public int AmountWantPut { get; }

        public StackInputData([NotNull] DataEntity source, [NotNull] DataEntity dataEntity, int amountWantPut)
        : base(source, dataEntity)
        {
            if (amountWantPut <= 0) throw new ArgumentOutOfRangeException(nameof(amountWantPut));
            ToDataEntity = dataEntity;
            AmountWantPut = amountWantPut;
            IsFullStackMove = AmountSource == AmountWantPut;
        }
    }
}
