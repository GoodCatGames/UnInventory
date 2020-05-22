using UnInventory.Core.MVC.Model.Commands;

namespace UnInventory.Core.MVC.Model
{
    public interface ICommandsFactory
    {
        /// <summary>
        /// FiltersOnlyHand will be used
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateForHand<T>()
            where T : ICommand, ISetFilters, new();

        /// <summary>
        /// FiltersAll will be used
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>()
            where T : ICommand, ISetFilters, new();
    }
}