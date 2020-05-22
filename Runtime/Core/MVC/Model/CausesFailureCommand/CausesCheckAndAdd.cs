using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Check;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand
{
    public class CausesCheckAndAdd : ICausesCheckAndAdd
    {
        private readonly CausesCollection _causesCollection;

        public CausesCheckAndAdd(CausesCollection causesCollection)
        {
            _causesCollection = causesCollection;
        }
       
        public void AddInCausesIfNecessary(CheckCauses causesCheck)
        {
            var actualCauses = causesCheck.GetActualCauses();
            _causesCollection.AddRange(actualCauses);
        }
    }
}
