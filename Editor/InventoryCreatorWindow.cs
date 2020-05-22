using System;
using JetBrains.Annotations;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Slot;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnInventory.Editor
{
    public class InventoryCreatorWindow : EditorWindow
    {
        [MenuItem("GameObject/UnInventory/Inventory Creator")]
        [UsedImplicitly]
        private static void Init()
        {
            var window = (InventoryCreatorWindow)GetWindow(typeof(InventoryCreatorWindow), true, "Inventory Creator");
            window.InitInstance();
            window.Show();
        }
        
        // Inventory
        private readonly InventoryCreator _inventoryCreator = new InventoryCreator();
        private readonly PresetInventory _presetInventory = new PresetInventory();
       
        // gui
        private readonly PopupSelectionType<ISlotRootComponent> _popupSelectionTypeSlotDataComponent
            = new PopupSelectionType<ISlotRootComponent>("SlotDataComponent: ");

        private readonly PopupSelectionType<SlotInputComponent> _popupSelectionTypeSlotInputComponent
            = new PopupSelectionType<SlotInputComponent>("SlotInputComponent: ", true);

        private bool _selectionComponentsGroup = true;
        
        public void InitInstance()
        {
            _presetInventory.SlotPrefab = InventoryManager.Get().BindPrefabs.SlotPrefabStandard;
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            InventorySectionOnGui();
            EditorGUILayout.Separator();
            
            SlotSectionOnGui();
            EditorGUILayout.Separator();

            if (GUILayout.Button("Create Inventory "))
            {
                _presetInventory.TypeSlotRootComponent = _popupSelectionTypeSlotDataComponent.Selected;
                _presetInventory.TypeSlotInputComponent = _popupSelectionTypeSlotInputComponent.Selected;
                var canvasInventory = _inventoryCreator.CreateInventory(_presetInventory);
                Selection.activeGameObject = canvasInventory;
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
        
        private void InventorySectionOnGui()
        {
            GUILayout.Label("Inventory: ", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            _presetInventory.NameInventory = EditorGUILayout.TextField("Name: ", _presetInventory.NameInventory);
            _presetInventory.TypeInventory = (DataInventory.TypeInventoryEnum)EditorGUILayout.EnumPopup("Type: ", _presetInventory.TypeInventory);

            var inventoryTypeDescription = GetDescriptionTypeInventory(_presetInventory.TypeInventory);
            GUILayout.TextArea(inventoryTypeDescription, GUI.skin.box);

            _presetInventory.CellSize = EditorGUILayout.Vector2IntField("Cell dimensions: ", _presetInventory.CellSize);

            _presetInventory.NumberColumns = EditorGUILayout.IntField("Columns", _presetInventory.NumberColumns);
            _presetInventory.NumberRows = EditorGUILayout.IntField("Rows", _presetInventory.NumberRows);

            EditorGUILayout.EndVertical();
        }

        private void SlotSectionOnGui()
        {
            GUILayout.Label("Slot: ", EditorStyles.boldLabel);
            _presetInventory.SlotPrefab = (GameObject)EditorGUILayout.ObjectField("Slot prefab", _presetInventory.SlotPrefab, typeof(GameObject), false);

            _selectionComponentsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(_selectionComponentsGroup, "Add components: ");

            // SlotData (Required)
            if (_selectionComponentsGroup)
            {
                _popupSelectionTypeSlotDataComponent.OnGuiPopup();
                _popupSelectionTypeSlotInputComponent.OnGuiPopup();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private string GetDescriptionTypeInventory(DataInventory.TypeInventoryEnum typeInventory)
        {
            switch (typeInventory)
            {
                case DataInventory.TypeInventoryEnum.FreeSlots:
                    return
                        "Slots are not interconnected, you can freely move and resize them. The behavior of multislot entities is similar to Grid.";
                case DataInventory.TypeInventoryEnum.Grid:
                    return
                        "Slots are connected in one grid. Multislot entities occupy one slot (migrated from another inventory will be placed in one slot)";
                case DataInventory.TypeInventoryEnum.GridSupportMultislotEntity:
                    return
                        "Slots are connected in one grid. Multislot entities occupy several slots.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeInventory), typeInventory, null);
            }
        }
        
    }
}
