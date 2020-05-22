using System;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;
using UnInventory.Samples.Sample_Hero.Components;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero.Filters
{
    public class FilterDummyStats :
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        [NotNull] private readonly IDataInventoryContainer _dummy;
        [NotNull] private readonly HeroComponent _heroComponent;

        public FilterDummyStats([NotNull] IDataInventoryContainer dummy, [NotNull] HeroComponent heroComponent)
        {
            _dummy = dummy ?? throw new ArgumentNullException(nameof(dummy));
            _heroComponent = heroComponent ?? throw new ArgumentNullException(nameof(heroComponent));
        }

        public bool Validate(MoveInputData data) => ValidateStatsRequired(data.ToInventory, data.EntitySource);

        // stack denied in Dummy
        public bool Validate(StackInputData data) => data.ToInventory != _dummy.DataInventory;

        public bool Validate(SwapPrimaryInputData data) =>
            ValidateStatsRequired(data.ToInventory, data.EntitySource)
            && ValidateStatsRequired(data.FromInventory, data.EntityTarget);

        private bool ValidateStatsRequired(DataInventory inventoryTo, DataEntity dataEntity)
        {
            if (inventoryTo != _dummy.DataInventory) return true;
            var dataEntityEquipment = dataEntity as DataEntityEquipment;
            var result = dataEntityEquipment?.RequiredMinStats.CompareTo(_heroComponent.Stats) < 0;
            return result;
        }
    }
}