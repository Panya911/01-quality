using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }

    public static class StackExtension
    {
        public static bool TryPeek<T>(this Stack<T> stack, out T value)
        {
            value = default(T);
            if (stack.Count < 0)
                return false;
            value = stack.Peek();
            return true;
        }
    }
}