using System;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Standard.MVC.Model.Commands.Composite;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;

namespace UnInventory.Standard.MVC.Model.Commands.FiltersResponse
{
    internal class CommandNoValidOnlyForFiltersFactory : ICommandNoValidOnlyForFiltersFactory
    {
        public ICommandNoValidOnlyForFilters Create<T>(T command)
        where T : ICommand
        {
            switch (command)
            {
                case MoveCommand moveCommand:
                    return new CommandNoValidOnlyForFiltersPrimary(moveCommand);
                case StackCommand stackCommand:
                    return new CommandNoValidOnlyForFiltersPrimary(stackCommand);
                case SwapPrimaryCommand swapPrimaryCommand:
                    return new CommandNoValidOnlyForFiltersPrimary(swapPrimaryCommand);

                case SwapCommand swapCommand:
                    return new CommandNoValidOnlyForFiltersSwap(swapCommand);
               
                case PutCommand moveInSlotGeneralCommand:
                    return new CommandNoValidOnlyForFiltersPut(moveInSlotGeneralCommand);
            }
            throw new NotSupportedException();
        }
    }
}
