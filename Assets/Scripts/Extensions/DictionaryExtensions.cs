using System.Linq;
using System.Collections.Generic;

namespace AMARI.Assets.Scripts.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetMatchingComponent<TKey, TValue>(this IDictionary<TKey, TValue> extDict, TKey component)
        {
            return extDict[component];
        }
    }
}