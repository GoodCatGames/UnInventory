using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class MoveBetweenInventoriesInputData : ICommandInputData
    {
        public DataInventory[] Inventories => new[] {FromInventory, ToInventory};
        public DataInventory FromInventory { get; }
        public DataInventory ToInventory { get; }
        public string IdEntity { get; }
        public int Amount { get; }
        
        public MoveBetweenInventoriesInputData([NotNull] DataInventory fromInventory,
            [NotNull] DataInventory inventory, string idEntity, int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
            FromInventory = fromInventory ?? throw new ArgumentNullException(nameof(fromInventory));
            ToInventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            IdEntity = idEntity;
            Amount = amount;

            if (string.IsNullOrEmpty(idEntity))
            {
                throw new ArgumentException("message", nameof(idEntity));
            }

            if (FromInventory == ToInventory) { throw new Exception("FromInventory == ToInventory!"); }
        }
    }
}
