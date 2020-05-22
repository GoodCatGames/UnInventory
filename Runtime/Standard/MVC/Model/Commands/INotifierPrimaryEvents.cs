using UnInventory.Standard.MVC.Model.Events;
using UnInventory.Standard.MVC.Model.Events.Move;

namespace UnInventory.Standard.MVC.Model.Commands
{
    public interface INotifierPrimaryEvents
    {
        EntityCreateEvent CreateEvent { get; }
        MoveToEmptyAfterEvent MoveToEmptyAfterEvent { get; }
        MoveInStackAfterEvent MoveInStackAfterEvent { get; }
        MoveSwapAfterEvent MoveSwapAfterEvent { get; }
        EntityRemoveEvent EntityRemoveEvent { get; }
    }
}