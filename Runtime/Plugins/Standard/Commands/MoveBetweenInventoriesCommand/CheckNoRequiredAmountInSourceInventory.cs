using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class CheckNoRequiredAmountInSourceInventory : CheckCauses
    {
        public readonly DataInventory InventoryFrom;
        public readonly string IdEntity;
        public readonly int AmountWantTake;

        public CheckNoRequiredAmountInSourceInventory(DataInventory inventoryFrom, string idEntity, int amountWantTake)
        {
            InventoryFrom = inventoryFrom;
            IdEntity = idEntity;
            AmountWantTake = amountWantTake;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var amountCanTake = DatabaseReadOnly.GetEntitiesWithId(InventoryFrom, IdEntity).Sum(entity => entity.Amount);
            if (amountCanTake < AmountWantTake)
            {
                result.Add(new CauseNoRequiredAmountInSourceInventory(InventoryFrom, IdEntity, AmountWantTake, amountCanTake));
            }
            return result;
        }
    }
}
