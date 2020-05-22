using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Create
{
    public class CreateDataAfterExecute : AfterExecuteData<CreateInputData>
    {
        public CreateDataAfterExecute([NotNull] CreateInputData inputData) : base(inputData)
        {
        }
    }
}
