using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Move
{
    public class MoveExecute : ExecutorPrimaryCommand<MoveCommand, MoveInputData, MoveDataAfterExecute>
    {
        protected override Action<MoveDataAfterExecute> ActionAfterEvent => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            NotifierPrimaryEvents.MoveToEmptyAfterEvent.Invoke;

        private DataEntity _dataEntityNewCreate;

        public MoveExecute(MoveCommand command) : base(command)
        {
        }

        /// <summary>
        /// Relocates the DataEntity to the empty slot (s).
        /// When the remainder creates a new one in the slot with the specified number. The amount is subtracted from the source.
        /// When exhausted, moves the source to a new slot.
        /// </summary>
        /// <returns></returns>
        protected override void Execute()
        {
            if (Command.InputData.AmountWantPut  > Command.InputData.EntitySource.Amount) { throw new Exception(); }
            
            bool takeFullStack = Command.InputData.AmountWantPut == Command.InputData.EntitySource.Amount;

            _dataEntityNewCreate = null;
            
            if (takeFullStack)
            {
                var toInventory = Command.InputData.SlotTo.DataInventory;
                DatabaseCommands.MoveEntityInSlot(Command.InputData.EntitySource, Command.InputData.SlotTo);
                ((IDataInventorySetter)Command.InputData.EntitySource).SetDataInventory(toInventory);

            }
            else
            {
                var data = Command.InputData.EntitySource.Copy();
                data.Amount = Command.InputData.AmountWantPut;
                DatabaseCommands.CreateEntityInSlot(data, Command.InputData.SlotTo);
                Command.InputData.EntitySource.Amount -= Command.InputData.AmountWantPut;
                _dataEntityNewCreate = data;
            }
        }

        protected override MoveDataAfterExecute GetDataEvent()
        {
            return new MoveDataAfterExecute(Command.InputData, _dataEntityNewCreate);
        }
    }
}
