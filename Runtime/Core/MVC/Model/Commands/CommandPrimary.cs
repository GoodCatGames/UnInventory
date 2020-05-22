using System.Linq;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Check;
using UnInventory.Core.MVC.Model.Filters;

namespace UnInventory.Core.MVC.Model.Commands
{
    public abstract class CommandPrimary<TInputData> : Command<TInputData>
        where TInputData : class, ICommandInputData  
    {
        public override bool ExecuteTry() => InventoryManager.ContainerDiInternal.CommandPrimaryExtension.ExecuteTry(this);
        
        public override IReadOnlyCausesCollection CausesFailureIncludedNested => CausesFailure;

        public sealed override void Update()
        {
            base.Update();
            CausesCheckAdd();
        }

        /// <summary>
        /// Should use CausesCheckAndAdd in this function 
        /// </summary>
        protected abstract void CausesCheckAdd();

        protected void CheckAndAddCauseNoValidFilters()
        {
            CheckAndAddCauseNoValidFilters(FilterCollection.GetFilterCollectionConcrete<TInputData>(), InputData);
        }

        private void CheckAndAddCauseNoValidFilters<T>(IFilterCollectionConcrete<T> filtersCollection, T data) 
            where T : class, ICommandInputData
        {
            filtersCollection.Validate(data, out var responses);
            var filterNoValidNewCollection = new Filters.Response.FilterNoValidCollection(responses.ToList());
            if (filterNoValidNewCollection.Any())
            {
                var checkNoValidFilters = new CheckNoValidFilters(filterNoValidNewCollection);
                CausesCheckAndAdd.AddInCausesIfNecessary(checkNoValidFilters);
            }
        }
    }
}
