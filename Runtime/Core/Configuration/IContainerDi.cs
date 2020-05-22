using UnInventory.Core.InventoryBindings;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Controller.Hand;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.DataBase;

namespace UnInventory.Core.Configuration
{
    public interface IContainerDi
    {
        ICommandsFactory Commands { get; }
        IDatabaseReadOnly DatabaseReadOnly { get; }
        IHand Hand { get; }
        IInventoryBindingFactory InventoryBindingFactory { get; }
        IBindComponentToDataDbRead BindComponentToDataDbRead { get; }
    }
}