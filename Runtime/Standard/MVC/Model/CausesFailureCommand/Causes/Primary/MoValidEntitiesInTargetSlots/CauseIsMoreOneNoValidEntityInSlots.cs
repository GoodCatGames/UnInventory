using System.Collections.Generic;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary.MoValidEntitiesInTargetSlots
{
    public class CauseIsMoreOneNoValidEntityInSlots : CauseFailureCommand
    {
        public IReadOnlyList<DataEntity> NoValidEntities { get; }
        public readonly IReadOnlyList<DataSlot> Slots;

        public CauseIsMoreOneNoValidEntityInSlots([NotNull] List<DataEntity> noValidEntities, IReadOnlyList<DataSlot> slots)
        {
            NoValidEntities = noValidEntities;
            Slots = slots;
        }
    }
}
