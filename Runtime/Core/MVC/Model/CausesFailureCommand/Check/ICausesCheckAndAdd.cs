namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Check
{
    public interface ICausesCheckAndAdd
    {
        void AddInCausesIfNecessary(CheckCauses causesCheck);
    }
}