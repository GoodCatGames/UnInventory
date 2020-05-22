using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Causes
{
    public class CausesCollection : Collection<CauseFailureCommand>, IReadOnlyCausesCollection
    {
        // ctor s
        public CausesCollection()
        {
        }

        public CausesCollection(CausesCollection causesCollection) : base(causesCollection)
        {
        }

        public CausesCollection(IReadOnlyCausesCollection causesCollection) : base(causesCollection.ToList())
        {
        }

        public bool IsContainsOnly<T>()
            where T : CauseFailureCommand => Count == 1 && IsCause<T>();

        public bool IsContainsOnly<T>(out T cause)
            where T : CauseFailureCommand
        {
            T causeOut = null;
            var result = Count == 1 && IsCause(out causeOut);
            cause = causeOut;
            return result;
        }

        public bool IsCause<T>() where T : CauseFailureCommand => IsCause<T>(out var _);

        public bool IsCause<T>(out T cause)
            where T : CauseFailureCommand
        {
            cause = GetCauseFirstOrDefault<T>();
            return cause != null;
        }

        public T GetCauseFirstOrDefault<T>() where T : CauseFailureCommand => this.OfType<T>().FirstOrDefault();

        public void AddRange(IEnumerable<CauseFailureCommand> causes)
        {
            foreach (var cause in causes)
            {
                Add(cause);
            }
        }

        public List<Type> GetTypes()
        {
            return this.Select(cause => cause.GetType()).ToList();
        }
        
        public override string ToString()
        {
            var result = "";
            result = this.Select(cause => result += cause.GetType().Name + ", ").LastOrDefault();
            return result ?? "";
        }
    }
}
