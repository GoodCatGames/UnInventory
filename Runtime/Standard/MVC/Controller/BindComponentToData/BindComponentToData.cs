using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.View.Components;

namespace UnInventory.Standard.MVC.Controller.BindComponentToData
{
    internal class BindComponentToData<TComponent, TData>
        where TComponent : class, IComponentUnInventory<TData> 
        where TData : class, IData
    {
        public IReadOnlyList<TComponent> Components => _components;
        private readonly List<TComponent> _components = new List<TComponent>();

        public void Add([NotNull] TComponent component)
        {
            Contract.Assert(!TryGetComponent(component.Data, out _));
            _components.Add(component);
            RemoveDestroyed();
        }

        public TComponent GetComponent([NotNull] TData data)
        {
            var componentExist = TryGetComponent(data, out var componentResult);
            Contract.Assert(componentExist, $"Component don t exist for {data}");
            return componentResult;
        }

        public bool TryGetComponent([NotNull] TData data, out TComponent componentResult)
        {
            componentResult = _components.SingleOrDefault(component => component.Data == data);
            return componentResult != null;
        }

        public void Remove([NotNull] TComponent component)
        {
            if (!_components.Contains(component))
            {
                return;
            }
            _components.Remove(component);
        }

        /// <summary>
        /// Components could be destroyed
        /// </summary>
        private void RemoveDestroyed()
        {
            _components.RemoveAll(component => component == null);
            _components.RemoveAll(component => component?.RectTransform == null);
        }
    }
}
