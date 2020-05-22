using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Composite;
using UnInventory.Standard.MVC.Model.Commands.Composite.InputData;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;

namespace UnInventory.Standard.MVC.Model.Commands.Composite
{
    public class CreateEntitiesCommand : CommandComposite<CreateEntitiesInputData>
    {
        protected override List<ICommand> GetCommandsConsidered()
        {
            var resultList = new List<ICommand>();

            var slotsFree = DatabaseReadOnly.SlotsFree(InputData.Inventory).ToArray();
            var busySlots = new List<DataSlot>();
            
            foreach (var entity in InputData.Entities)
            {
                foreach (var slot in slotsFree)
                {
                    if (busySlots.Contains(slot)) { continue; }

                    var command = CreateCommand<CreateCommand>();
                    command.EnterData(new CreateInputData(entity, slot));
                    if (command.IsCanExecute)
                    {
                        var slotsBusy = DatabaseReadOnly.GetSlotsForEntityInInventory(entity, slot);
                        busySlots.AddRange(slotsBusy);
                    }

                    resultList.Add(command);

                    if (command.IsCanExecute)
                    {
                        break;
                    }
                }
            }

            var checkNotFreeSlotsInventoryForCreateEntities = new CheckNotFreeSlotsInventoryForCreateEntities(InputData.Inventory, InputData.Entities
                , resultList);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNotFreeSlotsInventoryForCreateEntities);

            return resultList;
        }
    }
}
