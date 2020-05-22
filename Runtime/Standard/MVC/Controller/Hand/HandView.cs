using JetBrains.Annotations;
using UnInventory.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components.Entity;

namespace UnInventory.Standard.MVC.Controller.Hand
{
    internal class HandView : IHandView
    {
        private readonly Transform _handViewTransform;

        public HandView([NotNull] IEntityRootComponent entityComponentHand, [NotNull] DataEntity dataEntityHand)
        {
            _handViewTransform = entityComponentHand.Transform.gameObject.InstantiateOnRootCanvas().transform;
            entityComponentHand = _handViewTransform.GetComponent<IEntityRootComponent>();

            entityComponentHand.Init(null, dataEntityHand);

            var entityViewComponentHand = _handViewTransform.gameObject.GetComponent<EntityViewComponent>();
            entityViewComponentHand.Start();
            
            SetViewDifferentFromSourceEntity();
        }
        
        public void PositionSet(Vector2 position)
        {
            if (_handViewTransform != null)
            {
                _handViewTransform.position = position;
            }
        }

        public void Destroy()
        {
            Object.Destroy(_handViewTransform.gameObject);
        }

        protected void SetViewDifferentFromSourceEntity()
        {
            var amountText = _handViewTransform.GetComponentInChildren<Text>();
            amountText.color = Color.blue;
        }
    }
}