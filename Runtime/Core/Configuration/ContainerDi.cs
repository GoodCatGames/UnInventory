using UnityEngine;
using UnInventory.Core.Extensions;
using UnInventory.Core.InventoryBindings;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Controller.Hand;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.Data;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.View;

namespace UnInventory.Core.Configuration
{
    public abstract class ContainerDi : MonoBehaviour, IContainerDiCoreInternal
    {
        public IHand Hand { get; protected set;  }
        public IInventoryBindingFactory InventoryBindingFactory { get; protected set; }
        public IFiltersManager FiltersManager { get; protected set; }

        public IIdGenerator IdGen { get; protected set; }

        public ICommandPrimaryExecuteTry CommandPrimaryExtension { get; protected set; }

        public ICommandsFactory Commands { get; private set; }

        public IBindComponentToDataDbRead BindComponentToDataDbRead { get; protected set; }
        public IBindComponentToDataDbWrite BindComponentToDataDbWrite { get; protected set; }

        public Viewer Viewer { get; protected set; }

        public IDatabaseCommands DatabaseCommands { get; protected set; }
        public IDatabaseReadOnly DatabaseReadOnly { get; protected set; }
        
        public IPositionsManager PositionsManager { get; protected set; }
        
        public ICommandNoValidOnlyForFiltersFactory FactoryCommandNoValidOnlyForFilters { get; protected set; }

        protected IDatabaseNotifier DatabaseNotifier { get; set; }
        protected IInstantiator Instantiator;
        protected IBindCommandToExecutor BindCommandToExecutor;
        protected IExecutorCommandFactory ExecutorFactory;

        protected IFactoryTypeToPrefab FactoryDataEntityPrefab;

        protected GameObject DataEntityStandardPrefab;

        public void Init(GameObject dataEntityStandardPrefab)
        {
            DataEntityStandardPrefab = dataEntityStandardPrefab;

            LoadBasic();
            Commands = LoadCommandsFactory();
            BindDataEntitiesToPrefabs();
            BindPrimaryCommandsToExecute();
            Instantiator = LoadInstantiator();
            CommandPrimaryExtension = LoadCommandPrimaryExtension();
            LoadBindComponentToDataDbReadAndWrite();
            Viewer = LoadViewer();
        }

        protected abstract void LoadBasic();
        protected abstract ICommandsFactory LoadCommandsFactory();
        protected abstract void BindDataEntitiesToPrefabs();
        protected abstract void BindPrimaryCommandsToExecute();
        protected abstract IInstantiator LoadInstantiator();
        protected abstract ICommandPrimaryExecuteTry LoadCommandPrimaryExtension();
        protected abstract void LoadBindComponentToDataDbReadAndWrite();
        protected abstract Viewer LoadViewer();
        
        protected void BindDataEntityToPrefab<T>(GameObject prefab)
            where T : DataEntity => FactoryDataEntityPrefab.Add<T>(prefab);
    }
}
