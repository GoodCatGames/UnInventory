using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Standard.MVC.View.Components
{
    [RequireComponent(typeof(ISlotRootComponent))]
    public class SlotDebugComponent : MonoBehaviour
    {
        private const string TextRow = "TextRow";
        private const string TextColumn = "TextColumn";

        private IDebug Debug => (IDebug) _inventoryManager;
        private IInventoryManager _inventoryManager => InventoryManager.Get();
        private bool _debugMode;

        private ISlotRootComponent _slotComponent;

        private Outline _outline;

        [UsedImplicitly]
        private void Awake()
        {
            _debugMode = Debug.DebugMode;

            if (_debugMode)
            {
                _slotComponent = GetComponent<ISlotRootComponent>();
                var dataSlot = _slotComponent.Data;
                _outline = GetComponent<Outline>();

                var textRow = transform.Cast<Transform>().First(tr => tr.name == TextRow);
                textRow.GetComponent<Text>().text = dataSlot.Row.ToString();
                textRow.gameObject.SetActive(true);

                var textColumn = transform.Cast<Transform>().First(tr => tr.name == TextColumn);
                textColumn.GetComponent<Text>().text = dataSlot.Column.ToString();
                textColumn.gameObject.SetActive(true);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_debugMode)
            {
                var isEmpty = _slotComponent.IsEmpty; 
                // выделение занятых слотов
                if (isEmpty)
                {
                    _outline.effectDistance = new Vector2Int(2, -2);
                    _outline.effectColor = Color.black;
                }
                else
                {
                    _outline.effectDistance = new Vector2Int(4, -4);
                    _outline.effectColor = Color.red;
                }
            }
        }
    }
}