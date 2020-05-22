using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Composite;
using UnInventory.Standard.MVC.Model.Commands.Composite.InputData;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;

namespace UnInventory.Standard.MVC.Model.Commands.Composite
{
    public class PutCommand : CommandComposite<PutInputData>
    {
        protected override List<ICommand> GetCommandsConsidered()
        {
            var entitySource = InputData.EntitySource;
            var slot = InputData.SlotLeftTop;
            var amountWantPut = InputData.AmountWantPut;

            var resultList = new List<ICommand>();
            
            // MoveInEmptySlots
            var moveInEmptySlots = (MoveCommand)CreateCommand<MoveCommand>().EnterData(new MoveInputData(entitySource, slot, amountWantPut));
            resultList.Add(moveInEmptySlots);
            if (!moveInEmptySlots.IsCanTryStackOrSwap)
            {
                return resultList;
            }
            
            if(moveInEmptySlots.CausesFailure.IsCause<CauseIsMoreOneNoValidEntityInSlots>())
            {
                return resultList;
            }

            // There is only one entity in slots
            var otherEntity = moveInEmptySlots.CausesFailure.GetCauseFirstOrDefault<CauseIsSingleNoValidEntityInSlots>().Entity; 
            var moveInStack = (StackCommand) CreateCommand<StackCommand>(FilterCollection)
                .EnterData(new StackInputData(entitySource, otherEntity, amountWantPut));
            resultList.Add(moveInStack);

            if (!moveInStack.IsNeedOtherPrimaryAction)
            {
                return resultList;
            }
            
            var checkTakeNoFullStack = new CheckTakeNoFullStack(entitySource, amountWantPut);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkTakeNoFullStack);
            if (checkTakeNoFullStack.IsActual())
            {
                return resultList;
            }

            // Swap
            var swap = CreateCommand<SwapCommand>(FilterCollection).EnterData(new SwapInputData(entitySource, slot, otherEntity));
            resultList.Add(swap);
            return resultList;
        }
    }
}
