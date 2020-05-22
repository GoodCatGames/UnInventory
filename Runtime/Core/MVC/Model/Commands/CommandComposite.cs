using System;
using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Check;

namespace UnInventory.Core.MVC.Model.Commands
{
    public abstract class CommandComposite<TInputData> : Command<TInputData>
        where TInputData : ICommandInputData
    {
        public IReadOnlyList<ICommand> CommandsChainForExecute { get; protected set; } = new List<ICommand>();
        public IReadOnlyList<ICommand> CommandsConsidered { get; protected set; } = new List<ICommand>();

        public override IReadOnlyCausesCollection CausesFailureIncludedNested
        {
            get
            {
                var causesCollection = new CausesCollection(CausesFailure);
                var causeFailureCommands = CommandsConsidered.SelectMany(command => command.CausesFailureIncludedNested);
                causesCollection.AddRange(causeFailureCommands);
                return causesCollection;
            }
        }

        public sealed override bool ExecuteTry()
        {
            if (!IsCanExecute) return false;
            if (CommandsChainForExecute.Select(commandAction => commandAction.ExecuteTry()).Any(executeTry => !executeTry))
            {
                throw new Exception();
            }
            return true;
        }

        public sealed override void Update()
        {
            base.Update();
            CommandsConsidered = GetCommandsConsidered();
            CommandsChainForExecute = GetCommandsForExecute();
            var checkNoCommandsForExecute = new CheckNoCommandsForExecute(CommandsChainForExecute);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoCommandsForExecute);
        }
        
        /// <summary>
        /// Function must return the sequence of Commands generated
        /// When calling the ExecuteTry method, all Commands that can be executed will be executed in the order of this sequence
        /// </summary>
        /// <returns></returns>
        protected abstract List<ICommand> GetCommandsConsidered();

        private List<ICommand> GetCommandsForExecute() => CommandsConsidered.Where(command => command.IsCanExecute).ToList();
    }
}
