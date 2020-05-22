using System;
using System.Collections.Generic;

namespace UnInventory.Core.MVC.Model.CausesFailureCommand.Causes
{
    public interface IReadOnlyCausesCollection : IReadOnlyCollection<CauseFailureCommand>
    {
        bool IsContainsOnly<T>()
            where T : CauseFailureCommand;

        bool IsCause<T>() where T : CauseFailureCommand;

        bool IsCause<T>(out T cause)
            where T : CauseFailureCommand;

        T GetCauseFirstOrDefault<T>() where T : CauseFailureCommand;
        List<Type> GetTypes();

        bool IsContainsOnly<T>(out T cause)
            where T : CauseFailureCommand;
    }
}