using JetBrains.Annotations;
using UnInventory.Standard.Configuration;
using UnityEngine;
using UnInventory.Samples.Sample.Trade.Data;

namespace UnInventory.Samples.Sample.Trade
{
    public class ContainerDiTrade : ContainerDiStandard
    {
        [UsedImplicitly]
        [SerializeField] private GameObject _entityPricePrefab = default;

        protected override void BindDataEntitiesToPrefabs()
        {
            base.BindDataEntitiesToPrefabs();
            BindDataEntityToPrefab<DataEntityPrice>(_entityPricePrefab);
        }
    }
}
