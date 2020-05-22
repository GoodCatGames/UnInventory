using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Create
{
    public class CreateCommand : CommandPrimary<CreateInputData>
    {
        protected override void CausesCheckAdd()
        {
            var dataEntity = InputData.DataEntity;
            var slot = InputData.Slot;

            var checkOutBorder = new CheckOutBorder(dataEntity, slot);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkOutBorder);

            var checkNoValidEntitiesInTargetSlots = new CheckNoValidEntitiesInTargetSlots(dataEntity, slot);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoValidEntitiesInTargetSlots);
            
            // filters
            CheckAndAddCauseNoValidFilters();
        }
    }
}
