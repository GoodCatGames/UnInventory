using System.Linq;
using UnInventory.Core.Manager;
using UnInventory.Core.MVC.Model.CausesFailureCommand;
using UnInventory.Core.MVC.Model.CausesFailureCommand.Causes;
using UnInventory.Core.MVC.Model.DataBase;
using UnInventory.Core.MVC.Model.Filters;

namespace UnInventory.Core.MVC.Model.Commands
{
    public abstract class Command<TInputData> : ICommand, ISetFilters 
        where TInputData : ICommandInputData
    {
        public static T CreateCommand<T>(IFilterCollection filterCollection)
            where T : ICommand, ISetFilters, new()
        {
            var command = new T();
            command.SetFilters(filterCollection);
            return command;
        }

        public bool IsCanExecute => !_causesCollection.Any();

        public TInputData InputData { get; private set; }

        public abstract IReadOnlyCausesCollection CausesFailureIncludedNested { get; }
        public IReadOnlyCausesCollection CausesFailure => _causesCollection;
        
        protected IDatabaseReadOnly DatabaseReadOnly => InventoryManager.ContainerDi.DatabaseReadOnly;
        protected readonly CausesCheckAndAdd CausesCheckAndAdd;
        protected IFilterCollection FilterCollection;

        private readonly CausesCollection _causesCollection;

        protected Command()
        {
            _causesCollection = new CausesCollection();
            CausesCheckAndAdd = new CausesCheckAndAdd(_causesCollection);
        }

        public Command<TInputData> EnterData(TInputData inputData)
        {
            InputData = inputData;
            Update();
            return this;
        }


        public virtual void Update()
        {
            _causesCollection.Clear();
        }

        public abstract bool ExecuteTry();

        public string GetCausesFailure()
        {
            return _causesCollection.ToString();
        }

        protected T CreateCommand<T>()
            where T : ICommand, ISetFilters, new()
        {
            return CreateCommand<T>(FilterCollection);
        }
       
        void ISetFilters.SetFilters(IFilterCollection filterCollectionNew)
        {
            FilterCollection = filterCollectionNew;
        }
    }
}