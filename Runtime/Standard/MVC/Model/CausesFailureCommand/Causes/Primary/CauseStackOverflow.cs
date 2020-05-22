using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary
{
    public class CauseStackOverflow : CauseFailureCommand
    {
        [NotNull] public readonly DataEntity Source, Target;
        public readonly int AmountWantPut;
        public readonly int AmountCanPut;

        public CauseStackOverflow(DataEntity source, DataEntity target, int amountWantPut, int amountCanPut)
        {
            Source = source;
            Target = target;
            AmountWantPut = amountWantPut;
            AmountCanPut = amountCanPut;
        }
    }
}
