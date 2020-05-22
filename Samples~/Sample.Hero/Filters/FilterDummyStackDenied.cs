using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    public class FilterDummyStackDenied :
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        [NotNull] private readonly IDataInventoryContainer _dummy;

        public FilterDummyStackDenied([NotNull] IDataInventoryContainer dummy)
        {
            _dummy = dummy ?? throw new ArgumentNullException(nameof(dummy));
        }
        
        public bool Validate(MoveInputData data) => !(data.ToInventory == _dummy.DataInventory && data.AmountWantPut > 1);
        public bool Validate(StackInputData data) => data.ToInventory != _dummy.DataInventory;
        public bool Validate(SwapPrimaryInputData data) => !(data.ToInventory == _dummy.DataInventory && data.EntitySource.Amount > 1);
    }
}
