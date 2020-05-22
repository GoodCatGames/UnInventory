using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Composite.InputData
{
    public class CreateEntitiesInputData : ICommandInputData
    {
        public DataInventory Inventory { get; }
        public IEnumerable<DataEntity> Entities { get; }

        public CreateEntitiesInputData([NotNull] DataInventory inventory, IEnumerable<DataEntity> entities)
        {
            Inventory = inventory;
            Entities = entities;
        }
    }
}
