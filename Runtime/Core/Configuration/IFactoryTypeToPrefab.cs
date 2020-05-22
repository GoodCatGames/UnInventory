using System;
using UnityEngine;

namespace UnInventory.Core.Configuration
{
    public interface IFactoryTypeToPrefab
    {
        void Add<T>(GameObject prefab);
        void Add(Type type, GameObject prefab);
        GameObject GetPrefab(Type type);
    }
}