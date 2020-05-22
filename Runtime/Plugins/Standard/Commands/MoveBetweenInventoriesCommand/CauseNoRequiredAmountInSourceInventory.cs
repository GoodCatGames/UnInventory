using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class CauseNoRequiredAmountInSourceInventory : CauseFailureCommand
    {
        [NotNull] public readonly DataInventory InventoryFrom;
        public readonly string IdEntity;
        public readonly int AmountWantTake;
        public readonly int AmountCanTake;

        public CauseNoRequiredAmountInSourceInventory([NotNull] DataInventory inventoryFrom, string idEntity, int amountWantTake, int amountCanTake)
        {
            if (string.IsNullOrEmpty(idEntity))
            {
                throw new System.ArgumentException("message", nameof(idEntity));
            }

            if (amountWantTake <= 0) throw new ArgumentOutOfRangeException(nameof(amountWantTake));
            if (amountCanTake < 0) throw new ArgumentOutOfRangeException(nameof(amountCanTake));

            IdEntity = idEntity;
            InventoryFrom = inventoryFrom;
            AmountWantTake = amountWantTake;
            AmountCanTake = amountCanTake;
        }
    }
}
