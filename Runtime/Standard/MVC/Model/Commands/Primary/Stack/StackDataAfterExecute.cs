using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Stack
{
    public class StackDataAfterExecute : AfterExecuteData<StackInputData>
    {
        public StackDataAfterExecute([NotNull] StackInputData inputData) : base(inputData)
        {
        }
    }
}
