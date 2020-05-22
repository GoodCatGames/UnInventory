using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Commands.Executors;

namespace UnInventory.Standard.MVC.Model.Commands.Executors
{
    internal class CommandPrimaryExtension : ICommandPrimaryExecuteTry
    {
        private readonly IBindCommandToExecutor _bindCommandToExecutor;
        private readonly IExecutorCommandFactory _executorFactory;

        public CommandPrimaryExtension(IBindCommandToExecutor bindCommandToExecutor, IExecutorCommandFactory executorFactory)
        {
            _bindCommandToExecutor = bindCommandToExecutor;
            _executorFactory = executorFactory;
        }

        public bool ExecuteTry(ICommand command)
        {
            var typeExecutorForTypeCommand = _bindCommandToExecutor.GetTypeExecutorForTypeCommand(command);
            var executor = _executorFactory.Create(typeExecutorForTypeCommand, command);
            return executor.ExecuteTry();
        }
    }
}
