using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckStackOverflow : CheckCauses
    {
        public readonly DataEntity Source, Target;
        public readonly int AmountWantPut;
        public int AmountCanPut { get; private set; }

        public CheckStackOverflow(DataEntity source, DataEntity target, int amountWantPut)
        {
            Source = source;
            Target = target;
            AmountWantPut = amountWantPut;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var maxAmount = Source.AmountMax;
            int excessAmountStack = (Target.Amount + AmountWantPut) - maxAmount;

            AmountCanPut = excessAmountStack > 0
                ? maxAmount - Target.Amount
                : AmountWantPut;

            if (AmountCanPut < AmountWantPut)
            {
                result.Add(new CauseStackOverflow(Source, Target, AmountWantPut, AmountCanPut));
            }
            return result;
        }
    }
}
