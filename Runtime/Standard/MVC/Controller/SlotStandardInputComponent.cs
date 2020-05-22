using JetBrains.Annotations;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnInventory.Core.Configuration;
using UnInventory.Core.MVC.Controller;
using UnInventory.Core.MVC.View.Components.Slot;

namespace UnInventory.Standard.MVC.Controller
{
    [IsDefaultInventoryCreator]
    [RequireComponent(typeof(ISlotRootComponent))]
    public class SlotStandardInputComponent : SlotInputComponent
        ,IBeginDragHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler 
    {
        private const float TimerStartAddAmountInHandPerSecond = 0.3f;

        private float _timerAddAmountInHandPerSecond = 0.3f;
        private bool _modeAddAmountInHandPerSecond;
      
        private enum MousePressedModeEnum
        {
            NoMode,
            Left,
            Right
        }

        private static MousePressedModeEnum _mouseMode;

        private static void SetMouseMode(MousePressedModeEnum mode)
        {
            var old = _mouseMode;
            _mouseMode = _mouseMode == MousePressedModeEnum.NoMode ? mode : _mouseMode;
            //print($"{old} => {mode}");
        }
        private static void RemoveMouseMode()
        {
            _mouseMode = MousePressedModeEnum.NoMode;
            //print("remove MouseMode");
        }

        [UsedImplicitly]
        public void Update()
        {
            _timerAddAmountInHandPerSecond -= Time.deltaTime;
            if (_timerAddAmountInHandPerSecond <= 0 
                && _modeAddAmountInHandPerSecond 
                && !Hand.IsEmpty)
            {
                Hand.AddAmountInHand(1);
                _timerAddAmountInHandPerSecond = TimerStartAddAmountInHandPerSecond;
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.PointerIdIsLeftMouse())
            {
                PressedLeftMouse(eventData);
            }

            if (eventData.PointerIdIsRightMouse())
            {
                PressedRightMouse(eventData);
            }
        }

        private void PressedLeftMouse(PointerEventData eventData)
        {
            if (_mouseMode == MousePressedModeEnum.Right)
            {
                return;
            }
            
            if (_mouseMode == MousePressedModeEnum.Left)
            {
                OnPointerUp(eventData);
                return;
            }
            SetMouseMode(MousePressedModeEnum.Left);

            _modeAddAmountInHandPerSecond = true;
            _timerAddAmountInHandPerSecond = TimerStartAddAmountInHandPerSecond;
            Hand.TakeEntityOnPositionInHandTry(SlotComponent.Data, eventData.position);
        }

        private void PressedRightMouse(PointerEventData eventData)
        {
            if (_mouseMode == MousePressedModeEnum.Left)
            {
                return;
            }
            SetMouseMode(MousePressedModeEnum.Right);

            var success = Hand.TakeEntityOnPositionInHandTry(SlotComponent.Data, eventData.position);
            if (success)
            {
                Hand.AddAmountInHandSourcePercent(100);
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.PointerIdIsLeftMouse())
            {
                if(_mouseMode != MousePressedModeEnum.Left) { return;}
                RemoveMouseMode();
                _modeAddAmountInHandPerSecond = false;
                PutInSlotOrUndoTakeHand(eventData);
            }

            if (eventData.PointerIdIsRightMouse())
            {
                if (_mouseMode != MousePressedModeEnum.Right) { return; }
                RemoveMouseMode();
                PutInSlotOrUndoTakeHand(eventData);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_mouseMode != MousePressedModeEnum.Left) { return; }
            _modeAddAmountInHandPerSecond = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            Hand.PositionSet(eventData.position);
        }
    }
}
