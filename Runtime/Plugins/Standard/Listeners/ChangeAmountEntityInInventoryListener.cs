using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Listeners;
using UnityEngine.Events;

namespace UnInventory.Plugins.Standard.Listeners
{
    public class ChangeAmountEntityInInventoryListener : InventoryListener
    {
        public ChangeAmountEvent ChangeAmountEvent { get; } = new ChangeAmountEvent();
        private readonly IDataInventoryContainer _targetInventory;

        public ChangeAmountEntityInInventoryListener(IDataInventoryContainer targetInventory)
        {
            _targetInventory = targetInventory;
        }

        protected override void CreateReact(CreateDataAfterExecute data)
        {
            if (data.InputData.Inventory == _targetInventory.DataInventory)
            {
                ChangeAmount(data.InputData.DataEntity);
            }
        }

        protected override void MoveReact(MoveDataAfterExecute data) => ChangeAmountForMove(data.InputData);
        protected override void StackReact(StackDataAfterExecute data) => ChangeAmountForMove(data.InputData);
        protected override void SwapReact(SwapPrimaryDataAfterExecute data) => ChangeAmountForMove(data.InputData);

        protected override void RemoveReact(RemoveDataAfterExecute data)
        {
            if (data.InputData.Inventory == _targetInventory.DataInventory)
            {
                ChangeAmount(data.InputData.DataEntity);
            }
        }
        
        private void ChangeAmountForMove(ChangeInputData dataMove)
        {
            if (!dataMove.IsInsideSomeInventory
                && (dataMove.ToInventory == _targetInventory.DataInventory || dataMove.FromInventory == _targetInventory.DataInventory))
            {
                ChangeAmount(dataMove.EntitySource);
            }
        }

        private void ChangeAmount(DataEntity dataEntity) => ChangeAmountEvent.Invoke(new ChangeAmountDataEvent(dataEntity));
    }

    public class ChangeAmountEvent : UnityEvent<ChangeAmountDataEvent>
    {
    }

    public class ChangeAmountDataEvent
    {
        public readonly DataEntity DataEntity;
        public ChangeAmountDataEvent(DataEntity dataEntity)
        {
            DataEntity = dataEntity;
        }
    }
}
