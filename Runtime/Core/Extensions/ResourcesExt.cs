using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnInventory.Core.MVC.Model.Data;
using UnityEngine;

namespace UnInventory.Core.Extensions
{
    public static class ResourcesExt
    {
        /// <summary>
        /// Loads a DataEntity enumeration from ScriptableObjects at the specified path
        /// </summary>
        /// <param name="path">The path inside the Resources folder without the "/" at the end</param>
        /// <returns></returns>
        public static IEnumerable<DataEntity> LoadDataEntities(string path)
        {
            var objects = Resources.LoadAll(path + "/");
            if (!objects.Any())
            {
                Debug.LogWarning($"No objects in Resource folder: {path}");
            }
            var entities = new List<DataEntity>(objects.Cast<DataEntity>().Select(entity => entity.Copy()));
            return entities;
        }
    }
}
