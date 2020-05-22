using UnInventory.Core.MVC.Model.Filters.ResponseReacts;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Samples.Sample_Hero.Data;
using UnInventory.Samples.Sample_Hero.Filters;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample_Hero.NoFilterValidReact
{
    public class NoFilterValidReactTwoHanded :
        IFilterResponseReactConcrete<FilterDummyTwoHanded, MoveInputData>,
        IFilterResponseReactConcrete<FilterDummyTwoHanded, SwapPrimaryInputData>
    {
        public void Process(FilterDummyTwoHanded filter, MoveInputData data) => ProcessResponse(data);
        public void Process(FilterDummyTwoHanded filter, SwapPrimaryInputData data) => ProcessResponse(data);

        private void ProcessResponse(ChangeInputData data)
        {
            var equipment = (DataEntityEquipment)data.EntitySource;
           var message = $"The hand is occupied with two-handed weapons!";
            Label.Show(equipment, message);
        }
    }
}
