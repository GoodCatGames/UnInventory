using UnInventory.Core.MVC.Model.Filters.ResponseReacts;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Samples.Sample_Hero.Components;
using UnInventory.Samples.Sample_Hero.Data;
using UnInventory.Samples.Sample_Hero.Filters;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample_Hero.NoFilterValidReact
{
    public class NoFilterValidReactStats :
        IFilterResponseReactConcrete<FilterDummyStats, MoveInputData>,
        IFilterResponseReactConcrete<FilterDummyStats, SwapPrimaryInputData>
    {
        private readonly HeroComponent _heroComponent;

        public NoFilterValidReactStats(HeroComponent heroComponent)
        {
            _heroComponent = heroComponent;
        }

        public void Process(FilterDummyStats filter, MoveInputData data) => ProcessResponse(data);
        public void Process(FilterDummyStats filter, SwapPrimaryInputData data) => ProcessResponse(data);

        private void ProcessResponse(ChangeInputData data)
        {
            var equipment = (DataEntityEquipment)data.EntitySource;
           var message = $"Need: {equipment.RequiredMinStats - _heroComponent.Stats}more!";
            Label.Show(equipment, message);
        }
    }
}
