using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;

namespace UnInventory.Core.MVC.Model.Commands
{
    public interface ICommand
    {
        bool IsCanExecute { get; }
        IReadOnlyCausesCollection CausesFailure { get; }
        IReadOnlyCausesCollection CausesFailureIncludedNested { get; }

        void Update();
        bool ExecuteTry();
        string GetCausesFailure();
    }
}