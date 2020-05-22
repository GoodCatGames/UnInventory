using JetBrains.Annotations;
using UnityEditor;
using UnInventory.Core.Manager;
using UnInventory.Editor.Extensions;

namespace UnInventory.Editor
{
    public class SlotPrefabCreator
    {
        [MenuItem("Assets/Create/UnInventory/Prefabs/Slot Standard")]
        [UsedImplicitly]
        private static void Init()
        {
            var slotPrefabStandard = InventoryManager.Get().BindPrefabs.SlotPrefabStandard;
            CopyPrefabToActiveFolder.Copy(slotPrefabStandard, "Slot");
        }
    }
}
