using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.View.Components.Entity;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Samples.Sample.Trade.Data;

namespace UnInventory.Samples.Sample.Trade.Components
{
    public class EntityCostViewComponent : EntityViewComponent
    {
        private Text _costText;

        [UsedImplicitly]
        protected override void StartInHeir()
        {
            _costText = transform.Cast<Transform>().Single(child => child.name == "CostText").GetComponent<Text>();
        }

        protected override void UpdateView()
        {
            var cost = (Data as DataEntityPrice)?.Price ?? 0;
            _costText.text = cost == 0 ? "" : (cost * Data.Amount).ToString();
        }
    }
}
