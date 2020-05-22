using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Composite
{
    public class CauseTakeNoFullStack : CauseFailureCommand
    {
        public readonly DataEntity Entity;
        public readonly int AmountTake;

        public CauseTakeNoFullStack(DataEntity entity, int amountWantPut)
        {
            Entity = entity;
            AmountTake = amountWantPut;
        }
    }
}
