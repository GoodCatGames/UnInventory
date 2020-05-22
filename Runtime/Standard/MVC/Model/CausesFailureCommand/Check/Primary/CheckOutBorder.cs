using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckOutBorder : CheckCauses
    {
        public readonly DataEntity DataEntity;
        public readonly DataSlot DataSlot;

        public CheckOutBorder(DataEntity dataEntity, DataSlot dataSlot)
        {
            DataEntity = dataEntity;
            DataSlot = dataSlot;
        }

        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();
            var isOutBorder = DatabaseReadOnly.IsOutBorderInventory(DataSlot.DataInventory, DataEntity, DataSlot.Vector2Int);
            if (isOutBorder)
            {
                result.Add(new CauseIsOutBorder(DataEntity, DataSlot));
            }
            return result;
        }
    }
}
