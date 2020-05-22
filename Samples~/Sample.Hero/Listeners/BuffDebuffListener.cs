using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Listeners;
using UnInventory.Samples.Sample_Hero.Components;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Listeners
{
    public class BuffDebuffListener : InventoryListener
    {
        private readonly HeroComponent _heroComponent;
        private readonly IDataInventoryContainer _dummy;

        public BuffDebuffListener(HeroComponent heroComponent, IDataInventoryContainer dummy)
        {
            _heroComponent = heroComponent;
            _dummy = dummy;
        }

        protected override void CreateReact(CreateDataAfterExecute data)
        {
        }

        protected override void MoveReact(MoveDataAfterExecute data)
        {
            BuffDebaf(data.EntityInNewPosition, data.InputData.FromInventory, data.InputData.ToInventory);
        }

        protected override void StackReact(StackDataAfterExecute data)
        {
        }

        protected override void SwapReact(SwapPrimaryDataAfterExecute data)
        {
        }

        protected override void RemoveReact(RemoveDataAfterExecute data)
        {
        }
        
        private void BuffDebaf(DataEntity dataEntity, DataInventory fromInventory, DataInventory toInventory)
        {
            if (fromInventory == _dummy.DataInventory)
            {
                _heroComponent.Stats -= ((DataEntityEquipment)dataEntity).BuffStatsIfEquipment;
            }

            if (toInventory == _dummy.DataInventory)
            {
                _heroComponent.Stats += ((DataEntityEquipment)dataEntity).BuffStatsIfEquipment;
            }
        }
    }
}
