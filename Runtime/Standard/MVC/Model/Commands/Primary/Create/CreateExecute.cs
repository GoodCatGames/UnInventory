using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Create
{
    public class CreateExecute : ExecutorPrimaryCommand<CreateCommand, CreateInputData, CreateDataAfterExecute>
    {
        protected override Action<CreateDataAfterExecute> ActionAfterEvent => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            NotifierPrimaryEvents.CreateEvent.Invoke;

        public CreateExecute(CreateCommand command) : base(command)
        {
        }

        protected override void Execute() => DatabaseCommands.CreateEntityInSlot(Command.InputData.DataEntity, Command.InputData.Slot);
        protected override CreateDataAfterExecute GetDataEvent() => new CreateDataAfterExecute(Command.InputData);
    }
}
