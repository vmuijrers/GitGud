using System.Collections.Generic;
using System.Linq;

namespace ClickerExample
{
    public static class Registry<T> where T : class
    {
        public static HashSet<T> Items { get; private set; } = new HashSet<T>();

        public static void Register(T item) 
        { 
            Items.Add(item);
        }

        public static void UnRegister(T item)
        {
            Items.Remove(item);
        }

        public static IEnumerable<T> Filter(System.Func<T, bool> filter)
        {
            return Items.Where(filter);
        }
    }

}

