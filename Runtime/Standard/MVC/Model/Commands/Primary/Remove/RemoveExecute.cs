using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Remove
{
    public class RemoveExecute : ExecutorPrimaryCommand<RemoveCommand, RemoveInputData, RemoveDataAfterExecute>
    {
        protected override Action<RemoveDataAfterExecute> ActionAfterEvent => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            NotifierPrimaryEvents.EntityRemoveEvent.Invoke;

        public RemoveExecute(RemoveCommand command) : base(command)
        {
        }

        /// <summary>
        /// Create Entity in Inventory and Invoke CreateEvent
        /// </summary>
        /// <returns></returns>
        protected override void Execute()
        {
            Command.InputData.DataEntity.Amount -= Command.InputData.AmountRemove;
            if (Command.InputData.DataEntity.Amount == 0) { DatabaseCommands.RemoveEntity(Command.InputData.DataEntity); }
        }

        protected override RemoveDataAfterExecute GetDataEvent() => new RemoveDataAfterExecute(Command.InputData);
    }
}
