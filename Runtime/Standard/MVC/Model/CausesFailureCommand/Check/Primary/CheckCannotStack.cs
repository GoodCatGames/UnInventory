using System.Collections.Generic;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.CausesFailureCommand.Causes.Primary;

namespace UnInventory.Standard.MVC.Model.CausesFailureCommand.Check.Primary
{
    public class CheckCannotStack : CheckCauses
    {
        public readonly DataEntity Source, Target;

        public CheckCannotStack(DataEntity source, DataEntity target)
        {
            Source = source;
            Target = target;
        }
        
        public override IEnumerable<CauseFailureCommand> GetActualCauses()
        {
            var result = new List<CauseFailureCommand>();

            var isEntitiesMayBeStack = DatabaseReadOnly.IsEntitiesMayBeStack(Source, Target);
            if (!isEntitiesMayBeStack)
            {
                result.Add(new CauseEntitiesCannotStack(Source, Target));
            }
            return result;
        }
    }
}
