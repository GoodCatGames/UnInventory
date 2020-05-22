using System;
using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;

namespace UnInventory.Plugins.Standard.Commands.MoveBetweenInventoriesCommand
{
    public class MoveBetweenInventoriesCommand : CommandComposite<MoveBetweenInventoriesInputData>
    {
        protected override List<ICommand> GetCommandsConsidered()
        {
            var idEntity = InputData.IdEntity;
            var fromInventory = InputData.FromInventory;
            var toInventory = InputData.ToInventory;
            var amount = InputData.Amount;

            var resultList = new List<ICommand>();

            var checkNoRequiredAmountInSourceInventory = new CheckNoRequiredAmountInSourceInventory(fromInventory, idEntity, amount);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoRequiredAmountInSourceInventory);
            if (checkNoRequiredAmountInSourceInventory.IsActual())
            {
                return resultList;
            }

            var entitiesWithId = DatabaseReadOnly.GetEntitiesWithId(fromInventory, idEntity);
            var totalLeftToPut = amount;
            var slotsToInventory = DatabaseReadOnly.GetInventoryStructure(toInventory).Slots;

            var busySlots = new List<DataSlot>();

            foreach (var entityFrom in entitiesWithId)
            {
                var takeFromThisEntity = Math.Min(totalLeftToPut, entityFrom.Amount);
                var leftToPutFromThisEntity = takeFromThisEntity;
                foreach (var slot in slotsToInventory)
                {
                    if (busySlots.Contains(slot)) { continue; }

                    var wasPut = 0;

                    // empty slots try
                    var moveCommand = (MoveCommand) CreateCommand<MoveCommand>().EnterData(new MoveInputData(entityFrom, slot, leftToPutFromThisEntity));
                    ICommand command = moveCommand;
                    if (moveCommand.IsCanExecute)
                    {
                        wasPut = moveCommand.InputData.AmountWantPut;
                        busySlots = BusySlotsAdd(busySlots, entityFrom, slot);
                    }
                    else
                    {
                        if (moveCommand.IsCanTryStackOrSwap)
                        {
                            // stack
                            var entityInSlot = moveCommand.CausesFailure.GetCauseFirstOrDefault<CauseIsSingleNoValidEntityInSlots>().Entity;
                            var stackCommand = CreateCommand<StackCommand>(FilterCollection);
                            stackCommand.EnterData(new StackInputData(entityFrom, entityInSlot, leftToPutFromThisEntity));
                            command = stackCommand;
                            if (stackCommand.IsCanExecute)
                            {
                                wasPut = stackCommand.InputData.AmountWantPut;
                                busySlots = BusySlotsAdd(busySlots, entityFrom, slot);
                            }
                            else
                            {
                                var isCanTryWithOtherAmount = stackCommand.IsCanTryWithOtherAmount();
                                if (isCanTryWithOtherAmount.sucess)
                                {
                                    var amountCanPut = isCanTryWithOtherAmount.amountCanPut;
                                    var stackCommandOtherAmount = stackCommand.EnterData(new StackInputData(entityFrom, entityInSlot, amountCanPut));

                                    if (stackCommandOtherAmount.IsCanExecute)
                                    {
                                        wasPut = stackCommandOtherAmount.InputData.AmountWantPut;
                                        busySlots = BusySlotsAdd(busySlots, entityFrom, slot);
                                        command = stackCommandOtherAmount;
                                    }
                                }
                            }
                        }
                    }

                    resultList.Add(command);
                    leftToPutFromThisEntity -= wasPut;
                    totalLeftToPut -= wasPut;

                    if(leftToPutFromThisEntity == 0) { break; }
                }
                if (totalLeftToPut == 0) { break;}
            }

            var checkTargetInventoryIsOverflow = new CheckTargetInventoryIsOverflow(InputData.ToInventory,
                InputData.IdEntity, InputData.Amount, totalLeftToPut);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkTargetInventoryIsOverflow);

            return resultList;
        }
        
        private List<DataSlot> BusySlotsAdd(List<DataSlot> busySlots, DataEntity entity, DataSlot slot)
        {
            var slotsBusy = DatabaseReadOnly.GetSlotsForEntityInInventory(entity, slot);
            busySlots.AddRange(slotsBusy);
            return busySlots;
        }
    }
}
