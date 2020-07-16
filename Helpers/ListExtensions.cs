using System;
using System.Collections.Generic;
using System.Linq;

namespace DataService.Helpers
{
    public static class ListExtensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct =
              new[] { Enumerable.Empty<T>() };
            IEnumerable<IEnumerable<T>> result = emptyProduct;
            foreach (IEnumerable<T> sequence in sequences)
            {
                result = from accseq in result from item in sequence select accseq.Concat(new[] { item });
            }
            return result;
        }

        /// <summary>
        /// From a number of input sequences makes a result sequence of sequences of elements
        /// taken from the same position of each input sequence.
        /// Example: ((1,2,3,4,5), (6,7,8,9,10), (11,12,13,14,15)) --> ((1,6,11), (2,7,12), (3,8,13), (4,9,14), (5,10,15))
        /// </summary>
        /// <typeparam name="T">Type of sequence elements</typeparam>
        /// <param name="source">source seq of seqs</param>
        /// <param name="fillDefault">
        /// Defines how to handle situation when input sequences are of different length.
        ///     false -- throw InvalidOperationException
        ///     true  -- fill missing values by the default values for the type T.
        /// </param>
        /// <returns>Pivoted sequence</returns>
        public static IEnumerable<IEnumerable<T>> Pivot<T>(this IEnumerable<IEnumerable<T>> source, bool fillDefault = false)
        {
            IList<IEnumerator<T>> heads = new List<IEnumerator<T>>();

            foreach (IEnumerable<T> sourceSeq in source)
            {
                heads.Add(sourceSeq.GetEnumerator());
            }

            while (MoveAllHeads(heads, fillDefault))
            {
                yield return ReadHeads(heads);
            }
        }

        private static IEnumerable<T> ReadHeads<T>(IEnumerable<IEnumerator<T>> heads)
        {
            foreach (IEnumerator<T> head in heads)
            {
                if (head == null)
                    yield return default(T);
                else
                    yield return head.Current;
            }
        }

        private static bool MoveAllHeads<T>(IList<IEnumerator<T>> heads, bool fillDefault)
        {
            bool any = false;
            bool all = true;

            for (int i = 0; i < heads.Count; ++i)
            {
                bool hasNext = false;

                if (heads[i] != null) hasNext = heads[i].MoveNext();

                if (!hasNext) heads[i] = null;

                any |= hasNext;
                all &= hasNext;
            }

            if (any && !all && !fillDefault)
                throw new InvalidOperationException("Input sequences are of different length");

            return any;
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
       this IEnumerable<TFirst> first,
       IEnumerable<TSecond> second,
       Func<TFirst, TSecond, TResult> func)
        {
            using (var enumeratorA = first.GetEnumerator())
            using (var enumeratorB = second.GetEnumerator())
            {
                while (enumeratorA.MoveNext())
                {
                    enumeratorB.MoveNext();
                    yield return func(enumeratorA.Current, enumeratorB.Current);
                }
            }
        }
    }
}