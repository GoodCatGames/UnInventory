using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample_Hero.Commands
{
    public class CheckNotEnoughAmount : CheckCauses
    {
        public readonly DataInventory DataInventory;
        public readonly string Id;
        public readonly int AmountWantRemove;

        public CheckNotEnoughAmount(DataInventory dataInventory, string id, int amountWantRemove)
        {
            DataInventory = dataInventory;
            Id = id;
            AmountWantRemove = amountWantRemove;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var entitiesWithId = DatabaseReadOnly.GetEntitiesWithId(DataInventory, Id);
            var sumInInventory = entitiesWithId.Sum(entity => entity.Amount);
            if (sumInInventory < AmountWantRemove)
            {
                result.Add(new CauseNotEnoughAmount(DataInventory, Id, AmountWantRemove));
            }
            return result;
        }
    }
}
