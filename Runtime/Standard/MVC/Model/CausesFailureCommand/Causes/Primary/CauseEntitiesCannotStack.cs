using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary
{
    public class CauseEntitiesCannotStack : CauseFailureCommand
    {
        [NotNull] public readonly DataEntity Source;
        [NotNull] public readonly DataEntity Target;

        public CauseEntitiesCannotStack([NotNull] DataEntity source, [NotNull] DataEntity target)
        {
            Source = source;
            Target = target;
        }
    }
}
