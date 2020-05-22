using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Samples.Sample_Hero.Commands
{
    public class RemoveByIdInputData : ICommandInputData
    {
        [NotNull] public string Id { get; }
        public int AmountWantRemove { get; }
        public DataInventory DataInventory { get; }

        public RemoveByIdInputData(DataInventory dataInventory, [NotNull] string id, int amountWantRemove)
        {
            DataInventory = dataInventory;
            Id = id;
            AmountWantRemove = amountWantRemove;
        }
    }
}
