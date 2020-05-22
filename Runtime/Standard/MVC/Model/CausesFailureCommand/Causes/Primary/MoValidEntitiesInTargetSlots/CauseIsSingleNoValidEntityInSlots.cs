using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots
{
    public class CauseIsSingleNoValidEntityInSlots : CauseFailureCommand
    {
        public readonly DataEntity Entity;
        public readonly IReadOnlyList<DataSlot> Slots;

        public CauseIsSingleNoValidEntityInSlots(DataEntity entity, IReadOnlyList<DataSlot> slots)
        {
            Entity = entity;
            Slots = slots;
        }
    }
}
