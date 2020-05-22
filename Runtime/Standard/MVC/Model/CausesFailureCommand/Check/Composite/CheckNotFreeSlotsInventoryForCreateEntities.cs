using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Composite;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Composite
{
    public class CheckNotFreeSlotsInventoryForCreateEntities : CheckCauses
    {
        public readonly DataInventory Inventory;
        public readonly IEnumerable<DataEntity> Entities;
        private readonly IEnumerable<ICommand> _commandsChainForExecute;

        public CheckNotFreeSlotsInventoryForCreateEntities(DataInventory inventory, IEnumerable<DataEntity> entities, IEnumerable<ICommand> commandsChainForExecute)
        {
            Inventory = inventory;
            Entities = entities;
            _commandsChainForExecute = commandsChainForExecute;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var countSuccessEntities = _commandsChainForExecute.OfType<CreateCommand>()
                .Select(command => command.InputData.DataEntity).Distinct().Count();
            if (countSuccessEntities < Entities.Count())
            {
                result.Add(new CauseNotFreeSlotsInventoryForCreateEntities(Inventory, Entities));
            }
            return result;
        }
    }
}
