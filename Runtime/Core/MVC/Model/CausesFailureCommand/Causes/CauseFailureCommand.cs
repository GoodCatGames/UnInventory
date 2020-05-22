using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.DataBase;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Causes
{
    public abstract class CauseFailureCommand
    {
        protected IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;
    }
}
