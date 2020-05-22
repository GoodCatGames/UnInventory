using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    public class FilterDummyDamnedItem: 
        IFilterMoveInEmptySlots,
        IFilterSwap
    {
        private readonly IDataInventoryContainer _dummyInventoryContainer;

        public FilterDummyDamnedItem(IDataInventoryContainer dummyInventoryContainer)
        {
            _dummyInventoryContainer = dummyInventoryContainer;
        }

        public bool Validate(MoveInputData data) => ValidateDamned(data);
        
        public bool Validate(SwapPrimaryInputData data) => ValidateDamned(data);

        private bool ValidateDamned(ChangeInputData data)
        {
            var dataEntityEquipment = data.EntitySource as DataEntityEquipment;
            return !(dataEntityEquipment != null && data.FromInventory == _dummyInventoryContainer.DataInventory &&
                     dataEntityEquipment.IsDamned);
        }
    }
}
