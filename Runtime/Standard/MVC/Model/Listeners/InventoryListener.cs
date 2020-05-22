using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model;
using UnInventory.Standard.Configuration;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;

namespace UnInventory.Standard.MVC.Model.Listeners
{
    public abstract class InventoryListener
    {
        public bool IsOn { get; private set; }

        protected ICommandsFactory Commands => InventoryManager.ContainerDi.Commands;
        private Commands.INotifierPrimaryEvents NotifierPrimaryEvents => InventoryManager.ContainerDiOverride<ContainerDiStandard>().NotifierPrimaryEvents;
        
        protected InventoryListener()
        {
            IsOn = false;
        }

        ~InventoryListener()
        {
            Off();
        }
        
        public void On()
        {
            AddListeners();
            IsOn = true;
        }

        public void Off()
        {
            RemoveListeners();
            IsOn = false;
        }

        protected virtual void CreateReact(CreateDataAfterExecute data)
        {
        }

        protected virtual void MoveReact(MoveDataAfterExecute data){}
        protected virtual void StackReact(StackDataAfterExecute data){}
        protected virtual void SwapReact(SwapPrimaryDataAfterExecute data){}
        protected virtual void RemoveReact(RemoveDataAfterExecute data){}

        private void AddListeners()
        {
            NotifierPrimaryEvents.CreateEvent.AddListener(CreateReact);
            NotifierPrimaryEvents.MoveToEmptyAfterEvent.AddListener(MoveReact);
            NotifierPrimaryEvents.MoveInStackAfterEvent.AddListener(StackReact);
            NotifierPrimaryEvents.MoveSwapAfterEvent.AddListener(SwapReact);
            NotifierPrimaryEvents.EntityRemoveEvent.AddListener(RemoveReact);
        }

        private void RemoveListeners()
        {
            NotifierPrimaryEvents.CreateEvent.RemoveListener(CreateReact);
            NotifierPrimaryEvents.MoveToEmptyAfterEvent.RemoveListener(MoveReact);
            NotifierPrimaryEvents.MoveInStackAfterEvent.RemoveListener(StackReact);
            NotifierPrimaryEvents.MoveSwapAfterEvent.RemoveListener(SwapReact);
            NotifierPrimaryEvents.EntityRemoveEvent.RemoveListener(RemoveReact);
        }
    }
}
