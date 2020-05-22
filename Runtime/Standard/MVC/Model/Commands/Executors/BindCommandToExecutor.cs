using System;
using System.Collections.Generic;
using UnInventory.Core.MVC.Model.Commands;
using UnInventory.Core.MVC.Model.Commands.Executors;

namespace UnInventory.Standard.MVC.Model.Commands.Executors
{
    internal class BindCommandToExecutor : IBindCommandToExecutor
    {
        private readonly Dictionary<Type, Type> _dictionaryTypeCommandToTypeExecutor = new Dictionary<Type, Type>();

        public void Add<TCommandPrimary, TExecutor>()
            where TCommandPrimary : ICommand
            where TExecutor : IExecutorPrimaryCommand
        {
            _dictionaryTypeCommandToTypeExecutor.Add(typeof(TCommandPrimary), typeof(TExecutor));
        }

        public Type GetTypeExecutorForTypeCommand<TCommandPrimary>(TCommandPrimary commandPrimary) where TCommandPrimary : ICommand
        {
            var keyType = commandPrimary.GetType();

            var containsKey = _dictionaryTypeCommandToTypeExecutor.ContainsKey(keyType);
            if (!containsKey)
            {
                throw new Exception($"No add Command type {keyType}!");
            }

            var isExistValue = _dictionaryTypeCommandToTypeExecutor.TryGetValue(keyType, out _);
            if (!isExistValue)
            {
                throw new Exception($"Executor for Command type {keyType} no bind!");
            }

            return _dictionaryTypeCommandToTypeExecutor[keyType];
        }
    }
}
