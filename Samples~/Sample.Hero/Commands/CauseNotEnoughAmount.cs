using System;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample_Hero.Commands
{
    public class CauseNotEnoughAmount : CauseFailureCommand
    {
        public readonly DataInventory DataInventory;
        public readonly string Id;
        public readonly int AmountWantRemove;

        public CauseNotEnoughAmount(DataInventory dataInventory, string id, int amountWantRemove)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            AmountWantRemove = amountWantRemove;
            DataInventory = dataInventory ?? throw new ArgumentNullException(nameof(dataInventory));
        }
    }
}
