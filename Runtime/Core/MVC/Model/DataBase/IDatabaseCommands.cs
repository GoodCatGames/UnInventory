using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;

namespace UnInventory.Core.MVC.Model.DataBase
{
    public interface IDatabaseCommands
    {
        void CreateEntityInSlot([NotNull] DataEntity entity, [NotNull] DataSlot toSlotLeftTop);

        void RegisterSlot([NotNull] DataSlot slot);
        void RemoveEntity([NotNull] DataEntity entity);
        
        void MoveEntityInSlot([NotNull] DataEntity entity, [NotNull] DataSlot toSlotLeftTop);

        void UnbindEntityFromSlots([NotNull] DataEntity entity);
    }
}