using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary
{
    public class SwapPrimaryCommand : CommandPrimary<SwapPrimaryInputData>
    {
        protected override void CausesCheckAdd()
        {
            // IntersectionOfPositions
            var checkIntersectionOfPositions = new CheckIntersectionOfPositions(InputData.EntitySource, InputData.SlotTo,
                InputData.EntityTarget, InputData.SlotNewPositionEntityTarget);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkIntersectionOfPositions);
            
            // NoValidEntitiesInTargetSlots
            var dataEntitiesValidate = new[] {InputData.EntitySource, InputData.EntityTarget };

            var checkNoValidEntitiesInTargetSlotsEntityDisplacing = new CheckNoValidEntitiesInTargetSlots(InputData.EntitySource, InputData.SlotTo,
                dataEntitiesValidate);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoValidEntitiesInTargetSlotsEntityDisplacing);

            var checkNoValidEntitiesInTargetSlotsEntityTarget = new CheckNoValidEntitiesInTargetSlots(InputData.EntityTarget, InputData.SlotNewPositionEntityTarget,
                dataEntitiesValidate);
            CausesCheckAndAdd.AddInCausesIfNecessary(checkNoValidEntitiesInTargetSlotsEntityTarget);
            
            CheckAndAddCauseNoValidFilters();
        }
    }
}
