using System.Collections.Generic;

namespace GithubProcessor.Models
{
    public class MyDoubleDictionary
        : MyDoubleDictionary<string>
    {

    }

    public class MyIntDictionary<TKey>
        : Dictionary<TKey, int>
    {
        public void IncreaseCount(TKey key)
        {
            if (ContainsKey(key))
                this[key] = this[key] + 1;
            else
                Add(key, 1);
        }
    }

    public class MyDoubleDictionary<TKey>
        : Dictionary<TKey, double>
    {
        public void IncreaseCount(TKey key)
        {
            if (ContainsKey(key))
                this[key] = this[key] + 1;
            else
                Add(key, 1);
        }
    }

    public class MyAnyDictionary<T>
        : Dictionary<string, T>
    {
        public new void Add(string term, T value)
        {
            if (ContainsKey(term))
                this[term] = value;
            else
                base.Add(term, value);
        }
    }
}
