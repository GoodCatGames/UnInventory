using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Remove
{
    public class RemoveDataAfterExecute : AfterExecuteData<RemoveInputData>
    {
        public RemoveDataAfterExecute([NotNull] RemoveInputData inputData) : base(inputData)
        {
        }
    }
}
