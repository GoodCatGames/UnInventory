using System.Collections.Generic;
using System.Linq;
using UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample.Trade.Commands.Balance
{
    public class DisplayCauses
    {
        public readonly DataInventory HeroBag;
        public readonly DataInventory TraderBag;

        public DisplayCauses(DataInventory heroBag, DataInventory traderBag)
        {
            HeroBag = heroBag;
            TraderBag = traderBag;
        }

        public void Display(BalanceTables balanceTables)
        {
            var message = GetMessage(balanceTables);
            ShowMessage(message);
        }

        private void ShowMessage(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                Label.Show(messages, message);
            }
        }
        
        private IEnumerable<string> GetMessage(BalanceTables balanceTables)
        {
            var result = new List<string>();

            var causesCollection = balanceTables.CausesFailureIncludedNested;
            if (!causesCollection.Any()) { return result; }
            
            if (causesCollection.IsContainsOnly<CauseNoCommandsForExecute>())
            {
                result.Add("Already is balanced!");
            }

            result.AddRange(causesCollection.OfType<CauseNoRequiredAmountInSourceInventory>().
                Select(cause => $"In the inventory of {cause.InventoryFrom.NameInstance} there are not enough ({cause.AmountWantTake} - {cause.AmountCanTake})" 
            + $" {cause.AmountWantTake - cause.AmountCanTake} coins!"));
            
            result.AddRange(causesCollection.OfType<CauseTargetInventoryIsOverflow>().Select(cause =>
                $"Inventory {cause.Inventory} is full!"));

            return result;
        }
    }
}
