using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class CauseTargetInventoryIsOverflow : CauseFailureCommand
    {
        public readonly DataInventory Inventory;
        public readonly string IdEntity;
        public readonly int AmountWantPut;
        public readonly int AmountCanPut;

        public CauseTargetInventoryIsOverflow(DataInventory inventory, string idEntity, int amountWantPut, int amountCanPut)
        {
            Inventory = inventory;
            IdEntity = idEntity;
            AmountWantPut = amountWantPut;
            AmountCanPut = amountCanPut;
        }
    }
}
