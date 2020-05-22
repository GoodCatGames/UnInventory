using System;

namespace UnInventory.Core.MVC.Model.Commands.Executors
{
    public interface IExecutorCommandFactory
    {
        IExecutorPrimaryCommand Create<TCommand>(Type typeExecutor, TCommand command)
            where TCommand : ICommand;
    }
}