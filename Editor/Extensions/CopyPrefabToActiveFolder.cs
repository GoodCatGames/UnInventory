using UnityEditor;
using UnityEngine;

namespace UnInventory.Editor.Extensions
{
    public static class CopyPrefabToActiveFolder
    {
        public static bool Copy(GameObject prefab, string namePrefabCopy)
        {
            var pathFrom = AssetDatabase.GetAssetPath(prefab.GetInstanceID());
            var pathActiveFolder = AssetDatabase.GetAssetPath(Selection.activeObject);
            var pathToTemplate = $"{pathActiveFolder}/{namePrefabCopy}.prefab";
            var pathTo = AssetDatabase.GenerateUniqueAssetPath(pathToTemplate);
            var success = AssetDatabase.CopyAsset(pathFrom, pathTo);
            return success;
        }
    }
}
