using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary
{
    public class SwapPrimaryExecute : ExecutorPrimaryCommand<SwapPrimaryCommand, SwapPrimaryInputData, SwapPrimaryDataAfterExecute>
    {
        protected override Action<SwapPrimaryDataAfterExecute> ActionAfterEvent => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            NotifierPrimaryEvents.MoveSwapAfterEvent.Invoke;

        public SwapPrimaryExecute(SwapPrimaryCommand command) : base(command)
        {
        }
        
        protected override void Execute()
        {
            DatabaseCommands.UnbindEntityFromSlots(Command.InputData.EntitySource);
            DatabaseCommands.UnbindEntityFromSlots(Command.InputData.EntityTarget);
            DatabaseCommands.MoveEntityInSlot(Command.InputData.EntitySource, Command.InputData.SlotTo);
            DatabaseCommands.MoveEntityInSlot(Command.InputData.EntityTarget, Command.InputData.SlotNewPositionEntityTarget);
        }

        protected override SwapPrimaryDataAfterExecute GetDataEvent()
        {
            var dataMoveSwapAfter = new SwapPrimaryDataAfterExecute(Command.InputData);
            return dataMoveSwapAfter;
        }
    }
}
