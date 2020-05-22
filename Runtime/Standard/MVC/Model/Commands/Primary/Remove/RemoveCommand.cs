using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Remove
{
    public class RemoveCommand : CommandPrimary<RemoveInputData>
    {
        protected override void CausesCheckAdd()
        {
            CheckAndAddCauseNoValidFilters();
        }
    }
}
