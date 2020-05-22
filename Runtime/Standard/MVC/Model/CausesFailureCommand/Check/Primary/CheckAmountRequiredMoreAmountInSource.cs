using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckAmountRequiredMoreAmountInSource : CheckCauses
    {
        [NotNull] public readonly DataEntity Source;
        public readonly int AmountRequired;
        public int AmountCanTake;

        public CheckAmountRequiredMoreAmountInSource([NotNull] DataEntity source, int amountRequired)
        {
            Source = source;
            AmountRequired = amountRequired;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            AmountCanTake = Source.Amount;
            if (AmountRequired > AmountCanTake)
            {
                var cause = new CauseAmountRequiredMoreAmountInSource(Source, AmountRequired, AmountCanTake);
                result.Add(cause);

            }
            return result;
        }
    }
}
