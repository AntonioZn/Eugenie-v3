namespace Eugenie.Clients.Common.Еxtensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    public static class EnumerableExtensions
    {
        public static ICollection<T> RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Remove(item);
            }

            return collection;
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        public static IEnumerable<IEnumerable<T>> GroupInto<T>(this IEnumerable<T> source, int count)
        {
            var i = 0;
            var query = from s in source
                        let num = i++
                        group s by num / count
                        into g
                        select g.ToArray();
            return query.ToList();
        }

        public static async Task ProcessConcurrently<T>(this IEnumerable<T> collection, Func<T, CancellationToken, Task> func, int threadsCount, CancellationToken token)
        {
            var queue = new ConcurrentQueue<T>(collection);
            var count = queue.Count;

            var tasks = new List<Task>();
            for (var i = 0; i < threadsCount && i < count; i++)
            {
                Func<Task> task = async () =>
                                  {
                                      T item;
                                      while (queue.TryDequeue(out item) && !token.IsCancellationRequested)
                                      {
                                          await func.Invoke(item, token);
                                      }
                                  };

                tasks.Add(task.Invoke());
            }

            await Task.WhenAll(tasks);
        }

        public static void AddUsingCurrentDispatcher<T>(this ICollection<T> collection, T item)
        {
            Application.Current.Dispatcher.Invoke(() => collection.Add(item));
        }

        public static void AddRangeUsingCurrentDispatcher<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Application.Current.Dispatcher.Invoke(() => collection.AddRange(items));
        }

        public static void RemoveUsingCurrentDispatcher<T>(this ICollection<T> collection, T item)
        {
            Application.Current.Dispatcher.Invoke(() => collection.Remove(item));
        }

        public static void RemoveRangeUsingCurrentDispatcher<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Application.Current.Dispatcher.Invoke(() => collection.RemoveRange(items));
        }
    }
}