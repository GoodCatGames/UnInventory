using System.Linq;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Listeners;

namespace UnInventory.Samples.Sample_Hero.Listeners
{
    public class HotBarListener : InventoryListener
    {
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;

        private readonly IDataInventoryContainer _skillInventory;
        private readonly IDataInventoryContainer _bagInventory; 

        public HotBarListener(IDataInventoryContainer skillInventory, IDataInventoryContainer bagInventory)
        {
            _skillInventory = skillInventory;
            _bagInventory = bagInventory;
        }


        /// <summary>
        /// Create a copy with the total count from the original inventory (If the original inventory! = Panel)
        /// </summary>
        /// <param name="data"></param>
        private void MoveToEmptyForHotBar(MoveDataAfterExecute data)
        {
            if (data.InputData.ToInventory != _skillInventory.DataInventory)
            {
                return;
            }

            var isMoveInsidePanel = data.InputData.ToInventory == _skillInventory.DataInventory && data.InputData.ToInventory == data.InputData.FromInventory;

            //  get the total number in the source inventory (number remaining + new)
            var id = data.InputData.EntitySource.Id;

            var sumInSourceInventoryLost = DatabaseReadOnly.GetEntitiesWithId(_bagInventory.DataInventory, id).Sum(dataEntity => dataEntity.Amount);
            var sum = isMoveInsidePanel ? sumInSourceInventoryLost :
                sumInSourceInventoryLost + data.InputData.AmountWantPut;
            
            data.EntityInNewPosition.AmountMax = sum;
            data.EntityInNewPosition.Amount = sum;

            if (isMoveInsidePanel)
            {
                // destroy Source
                if (!data.InputData.IsFullStackMove)
                {
                    Commands.Create<RemoveCommand>().EnterData(new RemoveInputData(data.InputData.EntitySource, data.InputData.EntitySource.Amount))
                        .ExecuteTry();
                }
            }
            else
            {
                // return the taken amount to Source
                if (data.InputData.IsFullStackMove)
                {
                    var dataEntityForFromInventory = data.InputData.EntitySource.Copy();
                    dataEntityForFromInventory.Amount = data.InputData.AmountWantPut; 
                    var slot = DatabaseReadOnly.GetSlotOrNull(data.InputData.FromInventory, data.InputData.SlotFrom.Vector2Int);
                    Commands.Create<CreateCommand>().EnterData(new CreateInputData(dataEntityForFromInventory, slot)).ExecuteTry();
                }
                else
                {
                    data.InputData.EntitySource.Amount += data.InputData.AmountWantPut;
                }
            }
        }

        protected override void CreateReact(CreateDataAfterExecute data)
        {
        }

        protected override void MoveReact(MoveDataAfterExecute data)
        {
            MoveToEmptyForHotBar(data);
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
    }
}
