using System;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;
using UnInventory.Core.MVC.View.Components.Slot;
using UnInventory.Standard.MVC.View.Components;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UnInventory.Editor
{
    public class InventoryCreator
    {
        public GameObject CreateInventory(PresetInventory preset)
        {
            var canvasInventory = CreateCanvasInventory(preset.NameInventory);
            var rootInventory = CreateRootInventory(preset, canvasInventory);

            // Data  
            var dataInventory = new DataInventory(preset.NameInventory, preset.TypeInventory);
            var inventoryComponent = rootInventory.GetComponent<InventoryComponent>();
            inventoryComponent.Init(dataInventory);
            
            // Slots
            CreateSlots(preset, rootInventory);

            return canvasInventory;
        }
        
        private void CreateSlots(PresetInventory preset, GameObject root)
        {
            for (var row = 1; row <= preset.NumberRows; row++)
            {
                for (var column = 1; column <= preset.NumberColumns; column++)
                {
                    var slot = Object.Instantiate(preset.SlotPrefab, root.transform);
                    slot.name = GetSlotName(column, row);

                    if (preset.TypeInventory == DataInventory.TypeInventoryEnum.FreeSlots)
                    {
                        PrepareFreeSlot(slot, preset, column, row);
                    }

                    // ISlotRootComponent
                    var slotComponent = (ISlotRootComponent) slot.AddComponent(preset.TypeSlotRootComponent);
                    if (slotComponent == null)
                    {
                        throw new Exception($"Cant Add ISlotRootComponent component to slot: {slot}!");
                    }

                    slotComponent.Data.Column = column;
                    slotComponent.Data.Row = row;

                    // SlotInputComponent
                    if (preset.TypeSlotInputComponent != null)
                    {
                        slot.AddComponent(preset.TypeSlotInputComponent);
                    }

                    // SlotDebugComponent
                    slot.AddComponent<SlotDebugComponent>();
                }
            }
        }

        private void PrepareFreeSlot(GameObject slot, PresetInventory preset, int column, int row)
        {
            slot.transform.localPosition = new Vector2(preset.CellSize.x * (column - 1), preset.CellSize.y * (row - 1));
            slot.transform.localScale = new Vector3(1, 1, 1);
            slot.transform.GetComponent<RectTransform>().sizeDelta = preset.CellSize;
        }

        private string GetSlotName(int column, int row)
        {
            return "Slot_" + column + "_" + row;
        }

        private GameObject CreateRootInventory(PresetInventory preset, GameObject canvasInventory)
        {
            var root = new GameObject(preset.NameInventory);
            root.transform.SetParent(canvasInventory.transform);
            root.transform.localPosition = Vector3.zero;
            root.transform.localScale = new Vector3(1, 1, 1);

            root.AddComponent<InventoryComponent>();

            root.AddComponent<CanvasRenderer>();
            var rectTransform = root.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2Int(preset.CellSize.x * preset.NumberColumns, preset.CellSize.y * preset.NumberRows);

            if (preset.TypeInventory != DataInventory.TypeInventoryEnum.FreeSlots)
            {
                var grid = root.AddComponent<GridLayoutGroup>();
                grid.cellSize = preset.CellSize;
            }
            return root;
        }

        private GameObject CreateCanvasInventory(string nameInventory)
        {
            var canvasRoot = InventoryManagerCreator.GetCanvasInventoryManager();
            var canvasInventory = new GameObject(GetNameCanvasInventory(nameInventory), typeof(Canvas), typeof(GraphicRaycaster));
            canvasInventory.transform.SetParent(canvasRoot.transform);
            canvasInventory.transform.localPosition = Vector3.zero;
            canvasInventory.transform.localScale = new Vector3(1, 1, 1);
            return canvasInventory;
        }

        private static string GetNameCanvasInventory(string nameInventory) => nameInventory+"Canvas";
    }
}
