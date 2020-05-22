using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Stack
{
    public class StackCommand : CommandPrimary<StackInputData>
    {
        public bool IsNeedOtherPrimaryAction => CausesFailure.IsCause<CauseEntitiesCannotStack>();

        public (bool sucess, int amountCanPut) IsCanTryWithOtherAmount()
        {
            if (CausesFailure.IsContainsOnly<CauseStackOverflow>(out var causeStackOverflow) 
                && causeStackOverflow.AmountCanPut != 0)
            {
                
                return (true, causeStackOverflow.AmountCanPut);
            }
            return (false, 0);
        }
        
        protected override void CausesCheckAdd()
        {
            var source = InputData.EntitySource;
            var toDataEntity = InputData.ToDataEntity;
            var amountWantPut = InputData.AmountWantPut;

            CausesCheckAndAdd.AddInCausesIfNecessary(new CheckAmountRequiredMoreAmountInSource(source, amountWantPut));
            CausesCheckAndAdd.AddInCausesIfNecessary(new CheckCannotStack(source, toDataEntity));
            CausesCheckAndAdd.AddInCausesIfNecessary(new CheckStackOverflow(source, toDataEntity, amountWantPut));
            
            CheckAndAddCauseNoValidFilters();
        }
    }
}