using UnInventory.Standard.MVC.Model.Events;
using UnInventory.Standard.MVC.Model.Events.Move;

namespace UnInventory.Standard.MVC.Model.Commands
{
    internal class NotifierPrimaryEvents : INotifierPrimaryEvents
    {
        public EntityCreateEvent CreateEvent { get; } = new EntityCreateEvent();
        public MoveToEmptyAfterEvent MoveToEmptyAfterEvent { get; } = new MoveToEmptyAfterEvent();
        public MoveInStackAfterEvent MoveInStackAfterEvent { get; } = new MoveInStackAfterEvent();
        public MoveSwapAfterEvent MoveSwapAfterEvent { get; } = new MoveSwapAfterEvent();
        public EntityRemoveEvent EntityRemoveEvent { get; } = new EntityRemoveEvent();
    }
}