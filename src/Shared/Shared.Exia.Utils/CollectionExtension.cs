using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exia.Utils {
    public static class EnumerableExtension {
        /// <summary>
        ///     Adds the given object to the end of this list if is not present.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.ICollection{T}"/></param>
        /// <param name="item">The object to be added to the end of the source list, the value cannot be null</param>
        /// <returns>Return true if item has added otherwise false</returns>
        public static bool AddIfNotExist<T>(this ICollection<T> source, T item) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            if (item == null) {
                throw new ArgumentNullException("item");
            }

            bool exist = source.Contains(item);

            if (!exist) {
                source.Add(item);
            }

            return !exist;
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of this list.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.ICollection{T}"/></param>
        /// <param name="collection">The collection whose elements should be added to the end of the source list</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            if (collection == null) {
                throw new ArgumentNullException("collection");
            }

            foreach (T item in collection) {
                source.Add(item);
            }
        }

        /// <summary>
        ///     Convert a IEnumerable<T> to a ObservableCollection<T>
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <returns>Return a new ObservableCollection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source) {
            if(source == null) {
                throw new ArgumentNullException("source");
            }

            return new ObservableCollection<T>(source);
        }

        /// <summary>
        ///     Convert a IEnumerable<T> to a ICollection<T>
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <returns>Return a new generic Collection</returns>
        public static ICollection<T> ToCollection<T>(this IEnumerable<T> source) {
            if(source == null) {
                throw new ArgumentNullException("source");
            }

            return new Collection<T>(source.ToList());
        }

        /// <summary>
        ///     Returns index of the first occurrence in a sequence by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value) {
            return source.IndexOf(value, EqualityComparer<T>.Default);
        }

        /// <summary>
        ///     Returns the index of the first occurrence in a sequence by using a specified IEqualityComparer.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <param name="value">The object to locate in the sequence</param>
        /// <param name="comparer">An equality comparer to compare values.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire sequence, if found; otherwise, –1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value, IEqualityComparer<T> comparer) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            int index = 0;

            foreach (T item in source) {
                if (comparer.Equals(item, value)) {
                    return index;
                }

                index++;
            }

            return -1;
        }

        /// <summary>
        ///     Gets the number of elements contained in the <see cref="System.Collections.IEnumerable"/>
        /// </summary>
        /// <param name="source">An <see cref="System.Collections.IEnumerable"/></param>
        /// <returns>The number of elements contained in the <see cref="System.Collections.IEnumerable"/></returns>
        public static int Count(this IEnumerable source) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            IEnumerator enumerator = source.GetEnumerator();
            int result = 0;

            while (enumerator.MoveNext()) {
                result++;
            }

            return result;
        }

        /// <summary>
        ///     Performs the specified action on each element of the source.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <param name="action">Action executed on each element</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            if (action == null) {
                throw new ArgumentNullException("action");
            }

            foreach (T item in source) {
                action(item);
            }
        }

        /// <summary>
        ///     Performs on task the specified action on each element of the source.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable{T}"/></param>
        /// <param name="action">Action executed on each element</param>
        /// <rereturns></rereturns>
        public static Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> action) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            if (action == null) {
                throw new ArgumentNullException("action");
            }

            return Task.Run(() => source.ForEach(action));
        }

        /// <summary>
        ///     Return a string that contains each element of the source list delimited by element of a specified string.
        /// </summary>
        /// <param name="source">An <see cref="System.Collections.Generic.IEnumerable"/></param>
        /// <param name="separator">string that delimit each element of the source list. Default delimiter is empty string</param>
        /// <returns>Return an <see cref="System.String"/> with each element delimited by string separator</returns>
        public static string ToSeparateString(this IEnumerable source, string separator = "") {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            StringBuilder builder = new StringBuilder();

            foreach (object item in source) {
                builder.AppendFormat("{0}{1}", item, separator);
	        }

            return builder.ToString().TrimEnd(separator.ToCharArray());
        }
    }
}