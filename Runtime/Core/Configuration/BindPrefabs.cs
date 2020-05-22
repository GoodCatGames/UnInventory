using System;
using UnityEngine;

namespace UnInventory.Core.Configuration
{
    [Serializable]
    public class BindPrefabs
    {
        public GameObject EntityPrefabStandard => _EntityPrefabStandard;
        [SerializeField] private GameObject _EntityPrefabStandard = default;

        public GameObject SlotPrefabStandard => _slotPrefabStandard;
        [SerializeField] public GameObject _slotPrefabStandard = default;

        public GameObject PointForDebug => _pointForDebug;
        [SerializeField] private GameObject _pointForDebug = default;
    }
}
