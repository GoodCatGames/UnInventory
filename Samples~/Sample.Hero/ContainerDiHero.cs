using JetBrains.Annotations;
using UnInventory.Standard.Configuration;
using UnityEngine;
using UnInventory.Samples.Sample_Hero.Data;

namespace UnInventory.Samples.Sample_Hero
{
    public class ContainerDiHero : ContainerDiStandard
    {
        [UsedImplicitly]
        [SerializeField] private GameObject _equipmentPrefab = default;

        protected override void BindDataEntitiesToPrefabs()
        {
            base.BindDataEntitiesToPrefabs();
            BindDataEntityToPrefab<DataEntityEquipment>(_equipmentPrefab);
        }
    }
}
