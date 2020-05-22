using JetBrains.Annotations;
using UnityEditor;
using UnInventory.Core.Manager;
using UnInventory.Editor.Extensions;

namespace UnInventory.Editor
{
    public class EntityPrefabCreator
    {
        [MenuItem("Assets/Create/UnInventory/Prefabs/Entity Standard")]
        [UsedImplicitly]
        private static void Init()
        {
            var slotPrefabStandard = InventoryManager.Get().BindPrefabs.EntityPrefabStandard;
            CopyPrefabToActiveFolder.Copy(slotPrefabStandard, "Entity");
        }
    }
}
