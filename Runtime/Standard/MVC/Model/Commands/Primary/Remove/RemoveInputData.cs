using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Remove
{
    public class RemoveInputData : ICommandInputData
    {
        [NotNull] public readonly DataInventory Inventory;
        [NotNull] public DataEntity DataEntity { get; }
        public int AmountRemove { get; }

        public RemoveInputData([NotNull] DataEntity dataEntity, int amountRemove)
        {
            if (amountRemove <= 0) throw new ArgumentOutOfRangeException(nameof(amountRemove));

            DataEntity = dataEntity;
            Inventory = DataEntity.DataInventory;
            AmountRemove = amountRemove;
        }
    }
}
