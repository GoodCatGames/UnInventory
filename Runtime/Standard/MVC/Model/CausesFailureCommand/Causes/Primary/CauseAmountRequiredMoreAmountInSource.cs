using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary
{
    public class CauseAmountRequiredMoreAmountInSource : CauseFailureCommand
    {
        [NotNull] public readonly DataEntity Source;
        public readonly int AmountRequired;
        public readonly int AmountCanTake;

        public CauseAmountRequiredMoreAmountInSource([NotNull] DataEntity source, int amountRequired, int amountCanTake)
        {
            if (amountRequired <= 0) throw new ArgumentOutOfRangeException(nameof(amountRequired));
            if (amountCanTake < 0) throw new ArgumentOutOfRangeException(nameof(amountCanTake));
            AmountRequired = amountRequired;
            AmountCanTake = amountCanTake;
            Source = source;
        }
    }
}
