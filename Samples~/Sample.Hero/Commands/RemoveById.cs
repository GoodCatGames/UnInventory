using System;
using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;

namespace UnInventory.Samples.Sample_Hero.Commands
{
    public class RemoveById : CommandComposite<RemoveByIdInputData>
    {
        protected override List<ICommand> GetCommandsConsidered()
        {
            var commands = new List<ICommand>();

            var checkNotEnoughAmount = new CheckNotEnoughAmount(InputData.DataInventory, InputData.Id, InputData.AmountWantRemove);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNotEnoughAmount);

            if (checkNotEnoughAmount.IsActual())
            {
                return commands;
            }

            var entitiesWithId = DatabaseReadOnly.GetEntitiesWithId(InputData.DataInventory, InputData.Id);
            var lostRemove = InputData.AmountWantRemove;
            foreach (var dataEntity in entitiesWithId)
            {
                var amountInEntity = dataEntity.Amount;
                var remove = Math.Min(lostRemove, amountInEntity);
                var removeCommand = CreateCommand<RemoveCommand>().EnterData(new RemoveInputData(dataEntity, remove));
                commands.Add(removeCommand);
                lostRemove -= remove;
                if(lostRemove == 0) { break; }
            }
            return commands;
        }
    }
}
