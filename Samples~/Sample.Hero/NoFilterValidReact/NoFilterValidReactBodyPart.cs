using System.Linq;
using UnInventory.Core.MVC.Model.Filters.ResponseReacts;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Samples.Sample_Hero.Data;
using UnInventory.Samples.Sample_Hero.Filters;
using UnInventory.SamplesEnvironment;

namespace UnInventory.Samples.Sample_Hero.NoFilterValidReact
{
    public class NoFilterValidReactBodyPart :
        IFilterResponseReactConcrete<FilterDummyBodyPart, MoveInputData>,
        IFilterResponseReactConcrete<FilterDummyBodyPart, SwapPrimaryInputData>
    {
        public void Process(FilterDummyBodyPart filter, MoveInputData data) => ProcessResponse(data);

        public void Process(FilterDummyBodyPart filter, SwapPrimaryInputData data) => ProcessResponse(data);

        private void ProcessResponse(ChangeInputData data)
        {
            var equipment = (DataEntityEquipment) data.EntitySource;
            var slotDummy = data.Slots.OfType<DataSlotDummy>().First();
            var formattableString = $"{equipment.name}: for {equipment.BodyPart}, but slot: {slotDummy.BodyPart}!";
            Label.Show(equipment, formattableString);
        }
    }
}