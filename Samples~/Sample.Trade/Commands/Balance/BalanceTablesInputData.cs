using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Samples.Sample.Trade.Data;

namespace UnInventory.Samples.Sample.Trade.Commands.Balance
{
    public class BalanceTablesInputData : ICommandInputData
    {
        [NotNull] public IDataInventoryContainer HeroBag => Hero.Bag;
        [NotNull] public IDataInventoryContainer TraderBag => Trader.Bag;
        [NotNull] public IDataInventoryContainer HeroTable => Hero.Table;
        [NotNull] public IDataInventoryContainer TraderTable => Trader.Table;

        public readonly BagTableStruct Hero;
        public readonly BagTableStruct Trader;

        [NotNull] public readonly string IdCoins;

        public BalanceTablesInputData(BagTableStruct hero, BagTableStruct trader, [NotNull] string idCoins)
        {
            Hero = hero;
            Trader = trader;
            IdCoins = idCoins ?? throw new ArgumentNullException(nameof(idCoins));
        }
    }
}
