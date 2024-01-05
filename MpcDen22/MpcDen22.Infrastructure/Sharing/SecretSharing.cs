namespace MpcDen22.Infrastructure.Sharing;

/// <summary>
/// Provides utility methods for combinations and binomial coefficients.
/// </summary>
public static class Combinations
{
    /// <summary>
    /// Computes the binomial coefficient (n choose k).
    /// </summary>
    /// <param name="n">The total number of elements.</param>
    /// <param name="k">The number of elements to choose.</param>
    /// <returns>The binomial coefficient (n choose k).</returns>
    public static int Binom(int n, int k)
    {
        int end = k > n - k ? n - k : k;
        int top = 1;
        int bot = 1;

        for (int i = 1; i <= end; ++i)
        {
            top *= (n + 1 - i);
            bot *= i;
        }

        return top / bot;
    }

    /// <summary>
    /// Computes the next lexicographic combination.
    /// </summary>
    /// <param name="c">The current combination represented as an array.</param>
    /// <param name="m">The size of the set to pick from.</param>
    /// <param name="k">The number of elements to pick.</param>
    /// <returns>True if a next combination exists; otherwise, false.</returns>
    public static bool NextCombination(int[] c, int m, int k)
    {
        for (int i = k - 1; i >= 0; --i)
        {
            if (c[i] < m - k + i)
            {
                c[i]++;
                for (int j = i + 1; j < k; ++j)
                {
                    c[j] = c[j - 1] + 1;
                }
                return true;
            }
        }

        return false;
    }
}

/// <summary>
/// Provides utility methods for combinations and set operations.
/// </summary>
public static class CombinationsAndSets
{
    /// <summary>
    /// Fills the array <paramref name="c"/> with the n'th m-choose-k combination lexicographically.
    /// </summary>
    /// <typeparam name="T">The type of the set in which combinations are stored.</typeparam>
    /// <param name="c">The array to store the n'th combination.</param>
    /// <param name="n">The index of the combination.</param>
    /// <param name="m">The size of the set to pick from.</param>
    /// <remarks><paramref name="c"/> also determines the number of elements to pick.</remarks>
    public static void NthCombination<T>(List<T> c, int n, int m)
    {
        int k = c.Count;
        for (int i = 0; i < k; ++i)
        {
            c[i] = (T)(object)i;
        }

        while (n-- > 0 && NextCombination(c, m, k))
        {
            // Looping to the desired combination.
        }
    }

    /// <summary>
    /// Computes the intersection of two index sets and calls a callback with the indices.
    /// </summary>
    /// <typeparam name="T">The type of the set. Must support iteration.</typeparam>
    /// <param name="set">The first set.</param>
    /// <param name="other">The second set.</param>
    /// <param name="cb">A callback called with indices of elements in <paramref name="set"/> which also appear in <paramref name="other"/>.</param>
    public static void Intersection<T>(IEnumerable<T> set, IEnumerable<T> other, Action<int> cb)
    {
        using var enumerator1 = set.GetEnumerator();
        using var enumerator2 = other.GetEnumerator();
        int i = 0;

        // Implementing set_intersection with a twist.
        while (enumerator1.MoveNext() && enumerator2.MoveNext())
        {
            if (Comparer<T>.Default.Compare(enumerator1.Current, enumerator2.Current) < 0)
            {
                ++i;
            }
            else
            {
                if (EqualityComparer<T>.Default.Equals(enumerator2.Current, enumerator1.Current))
                {
                    cb(i);
                }
            }
        }
    }

    /// <summary>
    /// Computes the next lexicographic combination.
    /// </summary>
    /// <typeparam name="T">The type of the set in which combinations are stored.</typeparam>
    /// <param name="c">The array representing the current combination.</param>
    /// <param name="m">The size of the set to pick from.</param>
    /// <param name="k">The number of elements to pick.</param>
    /// <returns>True if a next combination exists; otherwise, false.</returns>
    public static bool NextCombination<T>(List<T> c, int m, int k)
    {
        for (int i = k - 1; i >= 0; --i)
        {
            if (Comparer<T>.Default.Compare(c[i], (T)(object)(m - k + i)) < 0)
            {
                c[i] = (T)(object)(Convert.ToInt32(c[i]) + 1);
                for (int j = i + 1; j < k; ++j)
                {
                    c[j] = (T)(object)(Convert.ToInt32(c[j - 1]) + 1);
                }
                return true;
            }
        }

        return false;
    }
}

public class SetOperations
{
    public static void Intersection<T>(IEnumerable<T> set, IEnumerable<T> other, Action<int> cb)
    {
        using (var enumerator1 = set.GetEnumerator())
        using (var enumerator2 = other.GetEnumerator())
        {
            int i = 0;

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                if (Comparer<T>.Default.Compare(enumerator1.Current, enumerator2.Current) == 0)
                {
                    cb(i);
                }

                i++;
            }
        }
    }

    public static void Difference<T>(IEnumerable<T> set, IEnumerable<T> other, Action<int> cb)
    {
        using (var enumerator1 = set.GetEnumerator())
        using (var enumerator2 = other.GetEnumerator())
        {
            int i = 0;

            while (enumerator1.MoveNext())
            {
                if (!enumerator2.MoveNext())
                {
                    cb(i++);
                    while (enumerator1.MoveNext())
                    {
                        cb(i++);
                    }
                    return;
                }

                if (Comparer<T>.Default.Compare(enumerator1.Current, enumerator2.Current) < 0)
                {
                    cb(i++);
                }
                else if (Comparer<T>.Default.Compare(enumerator1.Current, enumerator2.Current) > 0)
                {
                    i++;
                }
            }
        }
    }
}