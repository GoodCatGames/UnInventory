using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary
{
    public class CauseIsOutBorder : CauseFailureCommand
    {
        [NotNull]  public readonly DataEntity DataEntity;
        [NotNull]  public readonly DataSlot DataSlot;

        public CauseIsOutBorder([NotNull] DataEntity dataEntity, [NotNull] DataSlot dataSlot)
        {
            DataEntity = dataEntity;
            DataSlot = dataSlot;
        }
    }
}
