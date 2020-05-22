using JetBrains.Annotations;
using UnityEngine;
using UnInventory.Core.MVC.Model.Data;
using UnityEngine.Events;

namespace UnInventory.Core.MVC.View.Components
{
    public abstract class ViewComponent<TData, TRootComponent> : MonoBehaviour
        where TData : class, IData 
        where TRootComponent : IComponentUnInventory<TData>
    {
        protected TRootComponent RootComponent;
        protected TData Data => RootComponent?.Data;

        [UsedImplicitly]
        public void Start()
        {
            Init();
            StartInHeir();
            UpdateView();
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            OnDestroyInHeir();
            RemoveDataChangeListener(Data);
            RemoveSetNewDataListener(RootComponent, ProcessSetNewDataEvent);
        }

        protected abstract void StartInHeir();
        protected abstract void UpdateView();
        protected virtual void OnDestroyInHeir() { }

        private void Init()
        {
            RootComponent = GetComponent<TRootComponent>();
            AddDataChangeListener(Data);
            AddSetNewDataListener(RootComponent, ProcessSetNewDataEvent);
        }

        private void ProcessSetNewDataEvent(TData dataOld, TData dataNew)
        {
            RemoveDataChangeListener(dataOld);
            AddDataChangeListener(dataNew);
            UpdateView();
        }
        
        private void AddSetNewDataListener(TRootComponent rootComponent, UnityAction<TData, TData> action) =>
            rootComponent.SetNewDataEvent.AddListener(action);
        private void RemoveSetNewDataListener(TRootComponent rootComponent,
            UnityAction<TData, TData> action) => rootComponent?.SetNewDataEvent.RemoveListener(action);
 
        private void AddDataChangeListener(TData data) => data.DataChangeEvent.AddListener(UpdateView);

        private void RemoveDataChangeListener(TData data) => data?.DataChangeEvent.RemoveListener(UpdateView);
    }
}