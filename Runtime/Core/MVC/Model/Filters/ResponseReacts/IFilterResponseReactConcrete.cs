using JetBrains.Annotations;
using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model.Filters.ResponseReacts
{
    public interface IFilterResponseReactConcrete<in TFilter, in TData> : IFilterResponseReact
        where TFilter : IFilterConcrete<TData>
        where TData : ICommandInputData
    {
        [UsedImplicitly]
        void Process(TFilter filter, TData data);
    }
}
