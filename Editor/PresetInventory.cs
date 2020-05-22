using System;
using UnInventory.Core.MVC.Model.Data;
using UnityEngine;

namespace UnInventory.Editor
{
    public class PresetInventory
    {
        public string NameInventory = "InventoryName";
        public DataInventory.TypeInventoryEnum TypeInventory = DataInventory.TypeInventoryEnum.GridSupportMultislotEntity;
        public int NumberColumns = 5;
        public int NumberRows = 5;
        public Vector2Int CellSize = new Vector2Int(50, 50);

        public GameObject SlotPrefab;
        public Type TypeSlotRootComponent;
        public Type TypeSlotInputComponent;
    }
}