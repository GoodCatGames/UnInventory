using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Check
{
    public class CheckNoCommandsForExecute : CheckCauses
    {
        private readonly IEnumerable<ICommand> _commandsChainForExecute;

        public CheckNoCommandsForExecute(IEnumerable<ICommand> commandsChainForExecute)
        {
            _commandsChainForExecute = commandsChainForExecute;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            if(!_commandsChainForExecute.Any()) { result.Add(new CauseNoCommandsForExecute());}
            return result;
        }
    }
}
