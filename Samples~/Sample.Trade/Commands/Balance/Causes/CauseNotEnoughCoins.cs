using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample.Trade.Commands.Balance.Causes
{
    public class CauseNotEnoughCoins : CauseFailureCommand
    {
        public readonly DataInventory Inventory;
        public readonly int CoinsSumRequired;
        public readonly int CoinsSumExist;

        public CauseNotEnoughCoins(DataInventory inventory, int coinsSumRequired, int coinsSumExist)
        {
            Inventory = inventory;
            CoinsSumRequired = coinsSumRequired;
            CoinsSumExist = coinsSumExist;
        }
    }
}
