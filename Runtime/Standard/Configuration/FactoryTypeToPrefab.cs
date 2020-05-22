using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.Configuration;

namespace UnInventory.Standard.Configuration
{
    internal class FactoryTypeToPrefab : IFactoryTypeToPrefab
    {
        private readonly Dictionary<Type, GameObject> _dictionary = new Dictionary<Type, GameObject>();

        public void Add<T>(GameObject prefab) => Add(typeof(T), prefab);
        
        public void Add(Type type, GameObject prefab) => _dictionary.Add(type, prefab);

        [NotNull]
        public GameObject GetPrefab(Type type)
        {
            if (!_dictionary.ContainsKey(type))
            {
                throw new Exception($"Type {type.Name} as no associated prefab! Use ContainerDi override void BindDataEntitiesToPrefabs()");
            }
            return _dictionary[type];
        }
    }
}
