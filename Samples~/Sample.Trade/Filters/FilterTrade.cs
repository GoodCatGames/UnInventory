using System;
using System.Collections.Generic;
using System.Linq;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Primary;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Filters.Presets;

namespace UnInventory.Samples.Sample.Trade.Filters
{
    /// <summary>
    /// Moving only within your inventory group
    /// </summary>
    public class FilterTrade : 
        IFilterMoveInEmptySlots,
        IFilterStack,
        IFilterSwap
    {
        private readonly List<IDataInventoryContainer> _dataInventoryContainers;

        public FilterTrade(List<IDataInventoryContainer> dataInventoryContainers)
        {
            _dataInventoryContainers = dataInventoryContainers ?? throw new ArgumentNullException(nameof(dataInventoryContainers));
        }

        public bool Validate(MoveInputData data)
        {
            return MoveOnlyIntoGroup(data);
        }

        public bool Validate(StackInputData data)
        {
            return MoveOnlyIntoGroup(data);
        }

        public bool Validate(SwapPrimaryInputData data)
        {
            return MoveOnlyIntoGroup(data);
        }

        private bool MoveOnlyIntoGroup(ChangeInputData dataMove)
        {
            var dataInventoriesGroup = _dataInventoryContainers.Select(container => container.DataInventory);
            if (dataMove.IsInsideSomeInventory)
            {
                return true;
            }
        
            var inventoriesInGroup = dataInventoriesGroup.Where(dataMove.Inventories.Contains).ToArray();
            var isMoveInsideGroup = inventoriesInGroup.Length == 2;
            var isMoveOutGroup = inventoriesInGroup.Length == 0;

            return isMoveOutGroup || isMoveInsideGroup;
        }
    }
}
