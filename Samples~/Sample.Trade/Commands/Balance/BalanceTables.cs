using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample.Trade.Commands.Balance
{
    public class BalanceTables : CommandComposite<BalanceTablesInputData>
    {
        
        protected override List<ICommand> GetCommandsConsidered()
        {
            var heroTable = InputData.HeroTable.DataInventory;
            var heroBag = InputData.HeroBag.DataInventory;
            var traderTable = InputData.TraderTable.DataInventory;
            var traderBag = InputData.TraderBag.DataInventory;

            // Coins on tables need to be returned to bags
            var sumCoinsHeroTableToBag = GetSumCoins(heroTable);
            var coinsHeroTableToBag = GetInputDataCommandMoveCoins(heroTable, heroBag, sumCoinsHeroTableToBag);

            var sumCoinsTraderTableToBag = GetSumCoins(traderTable);
            var coinsTraderTableToBag = GetInputDataCommandMoveCoins(traderTable, traderBag, sumCoinsTraderTableToBag);

            // The difference between the value of items on tables excluding coins on them
            var differenceTablesHeroTrader = DifferenceTablesHeroTrader(sumCoinsHeroTableToBag, sumCoinsTraderTableToBag);

            MoveBetweenInventoriesInputData coinsBagToTable = null;
            if (differenceTablesHeroTrader > 0)
            {
                coinsBagToTable = GetInputDataCommandMoveCoins(traderBag, traderTable, differenceTablesHeroTrader);
            }
            
            if(differenceTablesHeroTrader < 0)
            {
                coinsBagToTable = GetInputDataCommandMoveCoins(heroBag, heroTable, -differenceTablesHeroTrader);
            }

            // summarize and get the final collection InputData
            var inputDates = (new List<MoveBetweenInventoriesInputData>() {coinsHeroTableToBag, coinsTraderTableToBag, coinsBagToTable} ).Optimize();
            return inputDates.Select(inputData => CreateCommand<MoveBetweenInventoriesCommand>().EnterData(inputData)).Cast<ICommand>().ToList();
        }

        private int DifferenceTablesHeroTrader(int sumCoinsHeroTableToBag, int sumCoinsTraderTableToBag)
        {
            var sumPriceHeroTable = ApplicationTrade.GetSumPrice(InputData.HeroTable);
            var sumPriceTraderTable = ApplicationTrade.GetSumPrice(InputData.TraderTable);

            // the cost of all items on the table without coins
            var sumPriceHeroTableWithoutCoins = sumPriceHeroTable - sumCoinsHeroTableToBag;
            var sumPriceTraderTableWithoutCoins = sumPriceTraderTable - sumCoinsTraderTableToBag;
            
            var differenceTablesHeroTrader = sumPriceHeroTableWithoutCoins - sumPriceTraderTableWithoutCoins;
            return differenceTablesHeroTrader;
        }

        [CanBeNull]
        private MoveBetweenInventoriesInputData GetInputDataCommandMoveCoins(DataInventory fromInventory, DataInventory toInventory, int sumCoins)
        {
            MoveBetweenInventoriesInputData result = null;
            if (sumCoins != 0)
            {
                result = new MoveBetweenInventoriesInputData(fromInventory, toInventory, 
                    InputData.IdCoins, sumCoins);
            }
            return result;
        }

        private int GetSumCoins(DataInventory inventory) => DatabaseReadOnly.GetEntitiesWithId(inventory, InputData.IdCoins).Sum(data => data.Amount);
    }
}
