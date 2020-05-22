using JetBrains.Annotations;
using UnInventory.Plugins.Standard.Listeners;
using UnInventory.Core.MVC.View.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UnInventory.Samples.Sample.Trade.Components
{
    [UsedImplicitly]
    public class TableSumScript : MonoBehaviour
    {
        private Text _sumText;
        private InventoryComponent _inventoryComponent;
        private ChangeAmountEntityInInventoryListener _changeAmountEntityInInventoryListener;

        [UsedImplicitly]
        private void Start()
        {
            _inventoryComponent = transform.parent.GetComponentInChildren<InventoryComponent>();

            _sumText = GetComponent<Text>();
            _sumText.text = string.Empty;
            
            _changeAmountEntityInInventoryListener = new ChangeAmountEntityInInventoryListener(_inventoryComponent);
            _changeAmountEntityInInventoryListener.On();
            _changeAmountEntityInInventoryListener.ChangeAmountEvent.AddListener(data => UpdateSum());

            UpdateSum();
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            _changeAmountEntityInInventoryListener?.ChangeAmountEvent.RemoveListener(data => UpdateSum());
        }

        private void UpdateSum()
        {
            var sumPrice = ApplicationTrade.GetSumPrice(_inventoryComponent);
            _sumText.text = sumPrice == 0 ? string.Empty : sumPrice + " coins";
        }
    }
}
