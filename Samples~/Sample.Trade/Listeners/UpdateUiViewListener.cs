using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Listeners;

namespace UnInventory.Samples.Sample.Trade.Listeners
{
    public class UpdateUiViewListener : InventoryListener
    {
        private readonly UiView _uiView;

        public UpdateUiViewListener(UiView uiView)
        {
            _uiView = uiView;
        }

        protected override void CreateReact(CreateDataAfterExecute data) => UpdateUi();
        protected override void MoveReact(MoveDataAfterExecute data) => UpdateUi();
        protected override void StackReact(StackDataAfterExecute data) => UpdateUi();
        protected override void SwapReact(SwapPrimaryDataAfterExecute data) => UpdateUi();
        protected override void RemoveReact(RemoveDataAfterExecute data) => UpdateUi();
        
        private void UpdateUi()
        {
            _uiView.UpdateView();
        }

      
       
    }
}
