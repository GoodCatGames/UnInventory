using System;

namespace UnInventory.Core.MVC.Model.Commands.Executors
{
    public interface IBindCommandToExecutor
    {
        void Add<TCommandPrimary, TExecutor>()
            where TCommandPrimary : ICommand 
            where TExecutor : IExecutorPrimaryCommand;

        Type GetTypeExecutorForTypeCommand<TCommandPrimary>(TCommandPrimary commandPrimary)
            where TCommandPrimary : ICommand 
        ;
    }
}