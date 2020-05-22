using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample.Trade.Commands.Balance.Causes
{
    public class CheckBalanceCauses : CheckCauses
    {
        public readonly IDataInventoryContainer InventoryContainer;
        public readonly int CoinsSumRequired;

        private readonly Func<IDataInventoryContainer, int> _getSumCoinsFunc;

        public CheckBalanceCauses([NotNull] IDataInventoryContainer inventoryContainer, int coinsSumRequired, 
            Func<IDataInventoryContainer, int> sumCoinsFunc)
        {
            InventoryContainer = inventoryContainer ?? throw new ArgumentNullException(nameof(inventoryContainer));
            CoinsSumRequired = coinsSumRequired;
            _getSumCoinsFunc = sumCoinsFunc;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();

            var sumCoinsInInventory = _getSumCoinsFunc.Invoke(InventoryContainer);
            if (CoinsSumRequired > sumCoinsInInventory)
            {
                var causeNotEnoughCoins = new CauseNotEnoughCoins(InventoryContainer.DataInventory, CoinsSumRequired, sumCoinsInInventory);
                result.Add(causeNotEnoughCoins);
            }

            if (CoinsSumRequired == 0)
            {
                result.Add(new CauseAlreadyBalanced());
            }
            return result;
        }
    }
}
