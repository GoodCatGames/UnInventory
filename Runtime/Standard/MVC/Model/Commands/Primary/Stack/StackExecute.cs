using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Stack
{
    public class StackExecute : ExecutorPrimaryCommand<StackCommand, StackInputData, StackDataAfterExecute>
    {
        protected override Action<StackDataAfterExecute> ActionAfterEvent => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            NotifierPrimaryEvents.MoveInStackAfterEvent.Invoke;

        public StackExecute(StackCommand command) : base(command)
        {
        }
        
        protected override void Execute()
        {
            if (Command.InputData.EntitySource == null) throw new ArgumentNullException(nameof(Command.InputData.EntitySource));
            if (Command.InputData.ToDataEntity == null) throw new ArgumentNullException(nameof(Command.InputData.ToDataEntity));

            if (Command.InputData.EntitySource.Id != Command.InputData.ToDataEntity.Id) { throw new Exception(); }

            int maxAmount = Command.InputData.EntitySource.AmountMax;

            int excessAmountStack = (Command.InputData.ToDataEntity.Amount + Command.InputData.AmountWantPut) - maxAmount;

            int amountPutInOtherEntity = excessAmountStack > 0
                ? maxAmount - Command.InputData.ToDataEntity.Amount
                : Command.InputData.AmountWantPut;

            if (amountPutInOtherEntity <= 0)
            {
                throw new Exception();
            }

            Command.InputData.ToDataEntity.Amount = Command.InputData.ToDataEntity.Amount + amountPutInOtherEntity;
            Command.InputData.EntitySource.Amount -= amountPutInOtherEntity;

            var entityFrom = Command.InputData.EntitySource;
            var isSourceDestroy = Command.InputData.EntitySource.Amount == 0;
            
            if (isSourceDestroy)
            {
                DatabaseCommands.RemoveEntity(entityFrom);
            }
        }

        protected override StackDataAfterExecute GetDataEvent() => new StackDataAfterExecute(Command.InputData);
    }
}
