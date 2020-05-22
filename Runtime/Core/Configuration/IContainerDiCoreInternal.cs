using UnityEngine;
using UnInventory.Core.Extensions;
using UnInventory.Core.MVC.Controller.BindComponentToData;
using UnInventory.Core.MVC.Model;
using UnInventory.Core.MVC.Model.Commands.Executors;
using UnInventory.Core.MVC.Model.Commands.FiltersResponse;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.View;

namespace UnInventory.Core.Configuration
{
    internal interface IContainerDiCoreInternal : IContainerDi
    {
        IIdGenerator IdGen { get; }
        ICommandPrimaryExecuteTry CommandPrimaryExtension { get; }
        IBindComponentToDataDbWrite BindComponentToDataDbWrite { get; }
        Viewer Viewer { get; }
        IDatabaseCommands DatabaseCommands { get; }
        ICommandNoValidOnlyForFiltersFactory FactoryCommandNoValidOnlyForFilters { get; }
        IPositionsManager PositionsManager { get; }
        IFiltersManager FiltersManager { get; }

        void Init(GameObject dataEntityStandardPrefab);
    }
}