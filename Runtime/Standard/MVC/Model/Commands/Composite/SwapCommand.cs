using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnInventory.Core.Extensions;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.Commands.Composite.InputData;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;

namespace UnInventory.Standard.MVC.Model.Commands.Composite
{
    public class SwapCommand : CommandComposite<SwapInputData>
    {
        protected override List<ICommand> GetCommandsConsidered()
        {
            var entityDisplacing = InputData.EntitySource;
            var entityTarget = InputData.EntityTarget;
            var slotNewPositionEntityDisplacing = InputData.SlotTo;

            var wishSlotsForSwap = GetWishSlotsForSwap();

            var commandsAll = wishSlotsForSwap
                .Select(slot =>
                 CreateCommand<SwapPrimaryCommand>()
                    .EnterData(new SwapPrimaryInputData(entityDisplacing, slotNewPositionEntityDisplacing, entityTarget, slot)))
                .Cast<ICommand>()
                .ToList();

            // commands sorted in the right order, return to the first CanExecute == true
            var commandsConsidered = commandsAll.TakeWhile(command => !command.IsCanExecute).ToList();
            var countNotCanExecute = commandsConsidered.Count;
            if (countNotCanExecute < commandsAll.Count)
            {
                commandsConsidered.Add(commandsAll[countNotCanExecute]);
            }
            return commandsConsidered;
        }
       
        private IEnumerable<DataSlot> GetWishSlotsForSwap()
        {
            var entityDisplacing = InputData.EntitySource;
            var entityTarget = InputData.EntityTarget;
            var comparer = InputData.Comparer;

            var displacingInventory = entityDisplacing.DataInventory;
            
            var displacingEntityPreviousPosition =
                DatabaseReadOnly.GetSlotsForEntityInInventory(displacingInventory,
                    DatabaseReadOnly.GetSlotOrNull(entityDisplacing).Vector2Int, entityDisplacing).ToArray();

            var wishSlots = new List<DataSlot>();

            if (displacingInventory.TypeInventory == DataInventory.TypeInventoryEnum.GridSupportMultislotEntity)
            {
                int widthEntityForWishSlots = entityTarget.Dimensions.x;
                int heightEntityForWishSlots = entityTarget.Dimensions.y;

                Vector2Int maxCoordinateWishSlots = new Vector2Int
                {
                    x = displacingEntityPreviousPosition.Max(slot => slot.Vector2Int.x),
                    y = displacingEntityPreviousPosition.Max(slot => slot.Vector2Int.y)
                };

                Vector2Int minCoordinateWishSlots = new Vector2Int
                {
                    x = displacingEntityPreviousPosition.Min(slot => slot.Vector2Int.x) -
                        widthEntityForWishSlots + 1,
                    y = displacingEntityPreviousPosition.Min(slot => slot.Vector2Int.y) -
                        heightEntityForWishSlots + 1
                };

                var wishSlotsPositions = Vector2IntExtension.GetArea(minCoordinateWishSlots, maxCoordinateWishSlots);
                wishSlots = DatabaseReadOnly.GetSlots(displacingInventory, wishSlotsPositions).ToList();
            }
            else
            {
                wishSlots.Add(DatabaseReadOnly.GetSlotOrNull(entityDisplacing));
            }

            wishSlots = wishSlots.OrderBy(slot => slot, comparer).ToList();
            return wishSlots;
        }
    }
}
