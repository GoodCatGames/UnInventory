using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary
{
    public class SwapPrimaryDataAfterExecute : AfterExecuteData<SwapPrimaryInputData>
    {
        public SwapPrimaryDataAfterExecute(SwapPrimaryInputData inputData) : base(inputData)
        {
        }
    }
}
