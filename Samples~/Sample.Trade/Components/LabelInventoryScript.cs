using JetBrains.Annotations;
using UnInventory.Core.MVC.View.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UnInventory.Samples.Sample.Trade.Components
{
    [UsedImplicitly]
    public class LabelInventoryScript : MonoBehaviour
    {
        private Text _label;
        private InventoryComponent _inventoryComponent;

        [UsedImplicitly]
        private void Start()
        {
            _label = GetComponent<Text>();
            _inventoryComponent = transform.parent.GetComponentInChildren<InventoryComponent>();
            _label.text = _inventoryComponent.DataInventory.NameInstance;
        }
    }
}
