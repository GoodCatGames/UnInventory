using System;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.View;
using UnInventory.Core.MVC.View.Components.Entity;
using UnInventory.Core.MVC.View.Components.Slot;
using UnInventory.Standard.Configuration;

namespace UnInventory.Standard.MVC.View
{
    internal class PositionsManager : IPositionsManager
    {
        private float RatioSizeEntityToSlot => InventoryManager.Get().RatioSizeEntityToSlot;
        private IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;
        private IBindComponentToDataDbRead BindComponentToDataDbRead => InventoryManager.ContainerDiOverride<ContainerDiStandard>().
            BindComponentToDataDbRead;

        /// <summary>
        /// Only for GridInventory
        /// </summary>
        public void PlaceEntityTransform(IEntityRootComponent entity, ISlotRootComponent slot)
        {
            var isAreaEntityForInventory = DatabaseReadOnly.IsAreaEntityForInventory(entity, slot.Data.DataInventory);
            if(isAreaEntityForInventory)
            {
                PlaceTransformInArea(entity, slot);
            }
            else
            {
                PlaceTransformInOneSlot(entity, slot);
            }
        }

        public Vector2 GetDeltaEntityAreaCenterToLeftTopWithScaleFactor(DataEntity dataEntity)
        {
            var entityComponent = BindComponentToDataDbRead.GetEntityComponent(dataEntity);
            var delta = GetDeltaEntityAreaCenterToLeftTop(entityComponent);
            var scaleFactor = InventoryManager.CanvasRootRectTransform.localScale;
            return delta * scaleFactor;
        }
        
        private Vector2 GetDeltaEntityAreaCenterToLeftTop(IEntityRootComponent entityComponent)
        {
            var entityArea = entityComponent.Data;

            // размеры картинки
            var sizeEntity = GetSizeEntityArea(entityArea);

            var sizeCellInImage = sizeEntity / entityArea.Dimensions;
            sizeEntity = (entityArea.Dimensions - new Vector2Int(1, 1)) * sizeCellInImage;
            var result = new Vector2(-sizeEntity.x / 2, sizeEntity.y / 2);
            //var scaleFactor = InventoryManager.CanvasRootRectTransform.localScale; 
            return result;// * scaleFactor;
        }

        private void PlaceTransformInOneSlot(IEntityRootComponent entity, ISlotRootComponent slot)
        {
            // prepare entity
            SetSizeEntityOneSlot(entity, slot);
            entity.Transform.SetParent(slot.Transform);

            // place
            entity.Transform.localPosition = new Vector3();
        }

        private void PlaceTransformInArea(IEntityRootComponent entity, ISlotRootComponent slotLeftTop)
        {
            // prepare entity
            var entityTransform = entity.Transform;
            entity.RectTransform.sizeDelta = GetSizeEntityArea(entity.Data, slotLeftTop) * RatioSizeEntityToSlot;
            
            var canvasTransform = slotLeftTop.InventoryComponent.CanvasInventory.transform;
            entityTransform.SetParent(canvasTransform);
            entityTransform.SetAsLastSibling();

            // place
            var positionSlot = GetSlotPosition(slotLeftTop);
            
            var delta = GetDeltaEntityAreaCenterToLeftTop(entity);
            var resultPosition = positionSlot - delta;

            entityTransform.localPosition = resultPosition;
        }

        private Vector2 GetSlotPosition(ISlotRootComponent slotLeftTop)
        {
            if (slotLeftTop.Data.DataInventory.TypeInventory == DataInventory.TypeInventoryEnum.FreeSlots)
            {
                return slotLeftTop.Transform.localPosition;
            }
            else
            {
                var inventoryComponent = slotLeftTop.InventoryComponent;
                var gridLayoutGroup = inventoryComponent.GridLayoutGroup;
                UpdateGrid(gridLayoutGroup);
                var gridRect = inventoryComponent.RectTransform;
                var slotRect = slotLeftTop.RectTransform;
                return GetChildLocalPosition(gridRect, slotRect);
            }
        }

        private void UpdateGrid(LayoutGroup gridLayoutGroup)
        {
            gridLayoutGroup.CalculateLayoutInputHorizontal();
            gridLayoutGroup.CalculateLayoutInputVertical();
            gridLayoutGroup.SetLayoutHorizontal();
            gridLayoutGroup.SetLayoutVertical();
        }

        
        private Vector2 GetChildLocalPosition(RectTransform rectTransformGrid, RectTransform rectTransformChild)
        {
            var localPositionGrid = (Vector2)rectTransformGrid.localPosition;
            var sizeDeltaGrid = rectTransformGrid.sizeDelta;
            var deltaGridFromCenterToLeftTop = new Vector2(-0.5f * sizeDeltaGrid.x, 0.5f * sizeDeltaGrid.y);

            var anchoredPositionChild = rectTransformChild.anchoredPosition;
            var childPosition = localPositionGrid + anchoredPositionChild + deltaGridFromCenterToLeftTop;
            return childPosition;
        }
        
        private void SetSizeEntityOneSlot(IEntityRootComponent entity, ISlotRootComponent slot)
        {
            var sizeEntityImage = GetSizeEntityOneSlot(slot);
            entity.RectTransform.sizeDelta = sizeEntityImage;
        }

        private Vector2 GetSizeEntityArea(DataEntity dataEntity)
        {
            var slotOrNull = DatabaseReadOnly.GetSlotOrNull(dataEntity);
            var slotComponent = BindComponentToDataDbRead.GetSlotComponent(slotOrNull);
            return GetSizeEntityArea(dataEntity, slotComponent);
        }

        private Vector2 GetSizeEntityArea(DataEntity dataEntity, ISlotRootComponent slot)
        {

            if (dataEntity.DataInventory.TypeInventory == DataInventory.TypeInventoryEnum.FreeSlots)
            {
                return GetSizeEntityOneSlot(slot);
            }
            else
            // grid TypeInventory
            {
                var sizeSlot = GetSizeSlot(slot);
                var sizeResult = sizeSlot * dataEntity.Dimensions;
                return sizeResult;
            }
        }

        private Vector2 GetSizeEntityOneSlot(ISlotRootComponent slot)
        {
            var slotSize = GetSizeSlot(slot);
            var sizeEntityImage = slotSize * RatioSizeEntityToSlot;
            return sizeEntityImage;
        }

        private Vector2 GetSizeSlot(ISlotRootComponent slotLeftTop)
        {
            var inventoryComponent = slotLeftTop.InventoryComponent;
            var typeInventory = slotLeftTop.Data.DataInventory.TypeInventory;
            switch (typeInventory)
            {
                case DataInventory.TypeInventoryEnum.Grid:
                case DataInventory.TypeInventoryEnum.GridSupportMultislotEntity:
                {
                    System.Diagnostics.Debug.Assert(inventoryComponent.GridLayoutGroup != null, "inventoryComponent.GridLayoutGroup != null");
                    return inventoryComponent.GridLayoutGroup.cellSize;
                }
                
                case DataInventory.TypeInventoryEnum.FreeSlots:
                    var slotSize = slotLeftTop.RectTransform.sizeDelta;
                    return slotSize;
                default:
                    throw new Exception();
            }
        }
    }
}