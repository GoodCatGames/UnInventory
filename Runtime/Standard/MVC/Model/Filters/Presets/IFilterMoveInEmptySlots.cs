using UnInventory.Core.MVC.Model.Filters;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;

namespace UnInventory.Standard.MVC.Model.Filters.Presets
{
    public interface IFilterMoveInEmptySlots : IFilterConcrete<MoveInputData>
    {
    }
}
