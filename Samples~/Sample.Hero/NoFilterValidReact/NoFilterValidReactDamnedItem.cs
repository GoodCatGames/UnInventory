using UnInventory.Core.MVC.Model.Filters.ResponseReacts;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Samples.Sample_Hero.Filters;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample_Hero.NoFilterValidReact
{
    public class NoFilterValidReactDamnedItem :
        IFilterResponseReactConcrete<FilterDummyDamnedItem, MoveInputData>,
        IFilterResponseReactConcrete<FilterDummyDamnedItem, SwapPrimaryInputData>
    {
        public void Process(FilterDummyDamnedItem filter, MoveInputData data) => ProcessResponse(filter, data);
        public void Process(FilterDummyDamnedItem filter, SwapPrimaryInputData data) => ProcessResponse(filter, data);

        private void ProcessResponse(FilterDummyDamnedItem filter, ChangeInputData data)
        {
            Label.Show("Damned item cannot be removed!");
        }
    }
}
