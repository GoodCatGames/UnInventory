using System.Collections.Generic;

namespace UnInventory.Core.Extensions
{
    public class IdGenerator : IIdGenerator
    {
        private readonly Dictionary<string, int> _dictionaryNameLastNumber = new Dictionary<string, int>();
        
        public string TakeId(string name)
        {
            if (_dictionaryNameLastNumber.TryGetValue(name, out var number))
            {
                number = ++_dictionaryNameLastNumber[name];
            }
            else
            {
                number = 1;
                _dictionaryNameLastNumber.Add(name, number);
            }
            return name + number;
        }
    }
}
