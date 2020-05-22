using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class CheckTargetInventoryIsOverflow : CheckCauses
    {
        public readonly DataInventory Inventory;
        public readonly string IdEntity;
        public readonly int AmountWantPut;
        public int AmountLeftPut => _totalLeftToPut;
        private readonly int _totalLeftToPut;


        public CheckTargetInventoryIsOverflow(DataInventory inventory, string idEntity, int amountWantPut, int totalLeftToPut)
        {
            Inventory = inventory;
            IdEntity = idEntity;
            AmountWantPut = amountWantPut;
            _totalLeftToPut = totalLeftToPut;
        }
        
        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            if (_totalLeftToPut == 0) return result;
            var causeTargetInventoryIsOverflow = new CauseTargetInventoryIsOverflow(Inventory, IdEntity, AmountWantPut, AmountWantPut - _totalLeftToPut);
            result.Add(causeTargetInventoryIsOverflow);
            return result;
        }
    }
}
