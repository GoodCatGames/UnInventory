using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.DataBase;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand
{
    public abstract class CheckCauses
    {
        protected IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;

        public bool IsActual() => GetActualCauses().Any();

        public abstract IEnumerable<CauseFailureCommand> GetActualCauses();
    }
}
