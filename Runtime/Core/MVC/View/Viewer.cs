using System;
using System.Linq;
using JetBrains.Annotations;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using Object = UnityEngine.Object;

namespace UnInventory.Core.MVC.View
{
    public abstract class Viewer
    {
        public static Viewer Create<T>(IDatabaseNotifier databaseNotifier, IDatabaseReadOnly readOnlyDatabase,
            IInstantiator instantiator, IPositionsManager positioner, IBindComponentToDataDbRead bindComponentToDataDbRead)
        where T : Viewer, new() 
        {
            var viewer = new T();
            viewer.Init(databaseNotifier, readOnlyDatabase, instantiator, positioner, bindComponentToDataDbRead);
            viewer.Subscribe();
            return viewer;
        }

        private IDatabaseNotifier _notifier;
        private IDatabaseReadOnly _readOnlyDatabase;
        private IInstantiator _instantiator;
        private IPositionsManager _positioner;
        private IBindComponentToDataDbRead _bindComponentToDataDbRead;

        protected virtual void Init([NotNull] IDatabaseNotifier databaseNotifier, IDatabaseReadOnly readOnlyDatabase, 
            [NotNull] IInstantiator instantiator,
            [NotNull] IPositionsManager positioner,
            [NotNull] IBindComponentToDataDbRead bindComponentToDataDbRead)
        {
            _notifier = databaseNotifier ?? throw new ArgumentNullException(nameof(databaseNotifier));
            _readOnlyDatabase = readOnlyDatabase;
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));
            _positioner = positioner ?? throw new ArgumentNullException(nameof(positioner));
            _bindComponentToDataDbRead = bindComponentToDataDbRead;
        }

        ~Viewer()
        {
            Unsubscribe();
        }

        public virtual void UpdateEntitiesInventory(DataInventory dataInventory)
        {
            var entities = _readOnlyDatabase.Entities.Where(entity => entity.DataInventory == dataInventory);
            foreach (var dataEntity in entities)
            {
                var isEntityExist = _bindComponentToDataDbRead.TryGetEntityComponent(dataEntity, out _);
                if (!isEntityExist)
                {
                    Create(dataEntity, _readOnlyDatabase.GetSlotOrNull(dataEntity));
                }
                UpdatePositionEntity(dataEntity);
            }
        }

        protected virtual void Subscribe()
        {
            _notifier.CreateEvent.AddListener(entity => Create(entity, _readOnlyDatabase.GetSlotOrNull(entity)));
            _notifier.CreateEvent.AddListener(UpdatePositionEntity);
            _notifier.ChangePositionEvent.AddListener(UpdatePositionEntity);
            _notifier.RemoveEvent.AddListener(Remove);
        }

        protected virtual void Unsubscribe()
        {
            _notifier.CreateEvent.RemoveListener(entity => Create(entity, _readOnlyDatabase.GetSlotOrNull(entity)));
            _notifier.ChangePositionEvent.RemoveListener(UpdatePositionEntity);
            _notifier.RemoveEvent.RemoveListener(Remove);
        }

        protected virtual void UpdatePositionEntity(DataEntity dataEntity)
        {
            var slot = _readOnlyDatabase.GetSlotOrNull(dataEntity);

            var inventory = dataEntity.DataInventory;
            var tryGetInventoryComponent = _bindComponentToDataDbRead.TryGetInventoryComponent(inventory, out _);
            if(!tryGetInventoryComponent) { return; }

            var entityComponent = _bindComponentToDataDbRead.GetEntityComponent(dataEntity);
            var slotDataComponent = _bindComponentToDataDbRead.GetSlotComponent(slot);

            _positioner.PlaceEntityTransform(entityComponent, slotDataComponent);
        }

        protected virtual void Create(DataEntity entity, DataSlot toSlotLeftTop)
        {
            var tryGetInventoryComponent = _bindComponentToDataDbRead.TryGetInventoryComponent(entity.DataInventory, out _);
            if (tryGetInventoryComponent)
            {
                _instantiator.AddEntity(entity, toSlotLeftTop);
            }
            //Debug.Log("Create: "+entity);
        }

        protected virtual void Remove(DataEntity dataEntity)
        {
            var tryGetEntityComponent = _bindComponentToDataDbRead.TryGetEntityComponent(dataEntity, out var componentResult);
            if (tryGetEntityComponent)
            {
                Object.Destroy(componentResult.Transform.gameObject);
            }
        }
    }
}


