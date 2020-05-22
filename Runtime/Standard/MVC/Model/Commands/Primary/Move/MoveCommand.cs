using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Move
{
    public class MoveCommand : CommandPrimary<MoveInputData>
    {
        public bool IsCanTryStackOrSwap => !IsImpossibleTryOtherPrimaryAction() && IdSingleOtherEntity(out var otherEntity);
        
        protected override void CausesCheckAdd()
        {
            var source = InputData.EntitySource;
            var slot = InputData.SlotTo;
            var amountWantPut = InputData.AmountWantPut;

            CausesCheckAndAdd.AddInCausesIfNecessary(new CheckAmountRequiredMoreAmountInSource(source, amountWantPut));

            var checkNoValidEntitiesInTargetSlots = new CheckNoValidEntitiesInTargetSlots(source, slot, amountWantPut);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoValidEntitiesInTargetSlots);

            CheckAndAddCauseNoValidFilters();
        }

        private bool IdSingleOtherEntity(out DataEntity entity)
        {
            var isSingleOtherEntityCause = CausesFailure.IsCause<CauseIsSingleNoValidEntityInSlots>(out var cause);
            if (!isSingleOtherEntityCause)
            {
                entity = null;
                return false;
            }
            entity = cause.Entity;
            return true;
        }

        private bool IsImpossibleTryOtherPrimaryAction()
        {
            return CausesFailure.IsCause<CauseIsOutBorder>()
                   || CausesFailure.IsCause<CauseIsMoreOneNoValidEntityInSlots>();
        }
    }
}