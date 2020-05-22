using JetBrains.Annotations;

namespace UnInventory.Core.MVC.Model.Commands
{
    public abstract class AfterExecuteData<T>
        where T : ICommandInputData
    {
        [NotNull] public readonly T InputData;

        protected AfterExecuteData([NotNull] T inputData)
        {
            InputData = inputData;
        }
    }
}
