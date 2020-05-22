using System;
using System.Collections.Generic;
using System.Linq;

namespace UnInventory.Core.Extensions
{
    /// <summary>
    /// Gets all implementations of the base type or interface 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReflectionClassImplementations<T> where T : class
    {
        public IReadOnlyDictionary<string, Type> DictionaryNameType => _dictionary;
        private readonly Dictionary<string, Type> _dictionary;

        public ReflectionClassImplementations()
        {
            var types = GetTypes();
            _dictionary = types.ToDictionary((type => type.Name), type => type);
        }

        public Type GetTypeOrNull(string name) => _dictionary.TryGetValue(name, out var type) ? type : null;

        private IEnumerable<Type> GetTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = assemblies.SelectMany(assembly => assembly.GetTypes().Where(isImplementType)).Distinct();
            return types;
        }

        private bool isImplementType(Type type)
        {
            return !type.IsAbstract && !type.IsInterface &&
                   (type.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(type));
        }

    }
}
