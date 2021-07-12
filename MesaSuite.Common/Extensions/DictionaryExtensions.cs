﻿using System;
using System.Collections.Generic;

namespace MesaSuite.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrSetDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> ctor)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary[key] = ctor();
            }

            return dictionary[key];
        }
    }
}
