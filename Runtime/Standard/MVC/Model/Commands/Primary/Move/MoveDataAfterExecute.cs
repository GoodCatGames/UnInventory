using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Standard.MVC.Model.Commands.Primary.Move
{
    public class MoveDataAfterExecute : AfterExecuteData<MoveInputData>
    {
        public DataEntity EntityInNewPosition => IsCreatedNewEntity ? DataEntityNewCreate : InputData.EntitySource;
        protected bool IsCreatedNewEntity => DataEntityNewCreate != null;

        [CanBeNull] public readonly DataEntity DataEntityNewCreate;
        
        public MoveDataAfterExecute(MoveInputData inputData, [CanBeNull] DataEntity dataEntityNewCreate) : base(inputData)
        {
            DataEntityNewCreate = dataEntityNewCreate;
        }
    }
}
