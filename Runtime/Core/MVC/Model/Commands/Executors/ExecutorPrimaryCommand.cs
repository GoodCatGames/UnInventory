using System;
using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.DataBase;

namespace UnInventory.Core.MVC.Model.Commands.Executors
{
    public abstract class ExecutorPrimaryCommand<TCommand, TInputData, TDataAfterExecuteEvent> : IExecutorPrimaryCommand
        where TCommand : Command<TInputData>
        where TInputData : ICommandInputData
        where TDataAfterExecuteEvent : AfterExecuteData<TInputData>
    {
        [NotNull] protected IDatabaseCommands DatabaseCommands => InventoryManager.ContainerDiInternal.DatabaseCommands;
        [NotNull] protected IDatabaseReadOnly ReadOnlyDatabase => InventoryManager.ContainerDi.DatabaseReadOnly;

        protected TCommand Command { get; private set; }
        protected abstract Action<TDataAfterExecuteEvent> ActionAfterEvent { get; }

        protected ExecutorPrimaryCommand(TCommand command)
        {
            Command = command;
        }

        public void Init(TCommand command)
        {
            Command = command;
        }
        
        public bool ExecuteTry()
        {
            if (!Command.IsCanExecute) return false;
            Execute();
            var dataEvent = GetDataEvent();
            ActionAfterEvent.Invoke(dataEvent);
            return true;
        }

        protected abstract void Execute();

        protected abstract TDataAfterExecuteEvent GetDataEvent();
    }
}
