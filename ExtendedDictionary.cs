using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Skrypty
{
    public class ExtendedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {

        public ExtendedDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }

        public void AddOrUpdate(TKey key, TValue value)
        {
            if (base.Keys.Contains(key))
                base[key] = value;
            else
                base.Add(key, value);
        }
    }
}

