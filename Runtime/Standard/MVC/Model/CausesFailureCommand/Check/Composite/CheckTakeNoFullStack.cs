using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Composite;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Composite
{
    public class CheckTakeNoFullStack : CheckCauses
    {
        public readonly DataEntity Entity;
        public readonly int AmountTake;

        public CheckTakeNoFullStack(DataEntity entity, int amountTake)
        {
            Entity = entity;
            AmountTake = amountTake;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var isTakeFullStack = Entity.Amount == AmountTake;
            if (!isTakeFullStack)
            {
                result.Add(new CauseTakeNoFullStack(Entity, AmountTake));
            }
            return result;
        }
    }
}
