using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Composite
{
    public class CauseNotFreeSlotsInventoryForCreateEntities : CauseFailureCommand
    {
        public readonly DataInventory Inventory;
        public readonly IEnumerable<DataEntity> Entities;

        public CauseNotFreeSlotsInventoryForCreateEntities(DataInventory inventory, IEnumerable<DataEntity> entities)
        {
            Inventory = inventory;
            Entities = entities;
        }
    }
}
