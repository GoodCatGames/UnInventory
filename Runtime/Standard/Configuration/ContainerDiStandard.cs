using UnInventory.Core.Configuration;
using UnInventory.Core.Extensions;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Core.MVC.View;
using UnInventory.Standard.InventoryBindings;
using UnInventory.Standard.MVC.Controller.BindComponentToData;
using UnInventory.Standard.MVC.Controller.Hand;
using UnInventory.Standard.MVC.Model;
using UnInventory.Standard.MVC.Model.Commands;
using UnInventory.Standard.MVC.Model.Commands.Executors;
using UnInventory.Standard.MVC.Model.Commands.FiltersResponse;
using UnInventory.Standard.MVC.Model.Commands.Primary.Create;
using UnInventory.Standard.MVC.Model.Commands.Primary.Move;
using UnInventory.Standard.MVC.Model.Commands.Primary.Remove;
using UnInventory.Standard.MVC.Model.Commands.Primary.Stack;
using UnInventory.Standard.MVC.Model.Commands.Primary.SwapPrimary;
using UnInventory.Standard.MVC.Model.Data;
using UnInventory.Standard.MVC.Model.DataBase;
using UnInventory.Standard.MVC.View;

namespace UnInventory.Standard.Configuration
{
    public class ContainerDiStandard : ContainerDi
    {
        public INotifierPrimaryEvents NotifierPrimaryEvents { get; private set; }

        private DataBase _dataBase;
       
        protected override void LoadBasic()
        {
            InventoryBindingFactory = new InventoryBindingFactory();

            IdGen = new IdGenerator();
            FactoryCommandNoValidOnlyForFilters = new CommandNoValidOnlyForFiltersFactory();
            Hand = new Hand();

            _dataBase = new DataBase();
            DatabaseCommands = _dataBase;
            DatabaseReadOnly = _dataBase;
            DatabaseNotifier = _dataBase;

            NotifierPrimaryEvents = new NotifierPrimaryEvents();
            PositionsManager = new PositionsManager();
            FactoryDataEntityPrefab = new FactoryTypeToPrefab();
            BindCommandToExecutor = new BindCommandToExecutor();
            ExecutorFactory = new ExecutorCommandFactory();
            FiltersManager = new FiltersManager();
        }

        protected override ICommandsFactory LoadCommandsFactory()
        {
            return new CommandsFactory(FiltersManager);
        }

        protected override void BindDataEntitiesToPrefabs()
        {
            BindDataEntityToPrefab<DataEntityStandard>(DataEntityStandardPrefab);
        }

        protected override void BindPrimaryCommandsToExecute()
        {
            BindCommandToExecutor.Add<CreateCommand, CreateExecute>();
            BindCommandToExecutor.Add<MoveCommand, MoveExecute>();
            BindCommandToExecutor.Add<StackCommand, StackExecute>();
            BindCommandToExecutor.Add<SwapPrimaryCommand, SwapPrimaryExecute>();
            BindCommandToExecutor.Add<RemoveCommand, RemoveExecute>();
        }

        protected override IInstantiator LoadInstantiator()
        {
            return new Instantiator(FactoryDataEntityPrefab);
        }

        protected override ICommandPrimaryExecuteTry LoadCommandPrimaryExtension()
        {
            return new CommandPrimaryExtension(BindCommandToExecutor, ExecutorFactory);
        }

        protected override void LoadBindComponentToDataDbReadAndWrite()
        {
            var bindComponentToDataDb = new BindComponentToDataDb(DatabaseCommands, DatabaseReadOnly);
            BindComponentToDataDbRead = bindComponentToDataDb;
            BindComponentToDataDbWrite = bindComponentToDataDb;
        }

        protected override Viewer LoadViewer()
        {
            return Viewer.Create<ViewerStandard>(DatabaseNotifier, DatabaseReadOnly, Instantiator, PositionsManager, BindComponentToDataDbRead);
        }
    }
}

