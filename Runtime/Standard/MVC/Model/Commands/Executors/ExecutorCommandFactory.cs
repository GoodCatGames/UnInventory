using System;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Commands.Executors;

namespace UnInventory.Standard.MVC.Model.Commands.Executors
{
    internal class ExecutorCommandFactory : IExecutorCommandFactory
    {
        public IExecutorPrimaryCommand Create<TCommand>(Type typeExecutor, TCommand command)
            where TCommand : ICommand
        {
            var instance = Activator.CreateInstance(typeExecutor, command);
            var execute = (IExecutorPrimaryCommand)instance;
            return execute;
        }
    }
}
