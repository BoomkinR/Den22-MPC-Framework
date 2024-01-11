using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Sharing;

/// <summary>
/// A factory for working with replicated shares.
/// </summary>
/// <typeparam name="T">The type of the ring over which shares are defined.</typeparam>
public class Replicator<T>
{
    // Type of an index set.
    public class IndexSet : List<int>
    {
    }

    private List<List<int>> mCombinations;
    private Dictionary<List<int>, int> mRevComb;
    private List<IndexSet> mLookup;
    private int mDifferenceSize;

    /// <summary>
    /// Creates a new Replicator.
    /// </summary>
    /// <param name="n">The number of shares that can be created.</param>
    /// <param name="t">The privacy threshold.</param>
    public Replicator(int n, int t)
    {
        Size = n;
        Threshold = t;
        ShareSize = Combinations.Binom(n - 1, t);
        AdditiveShareSize = Combinations.Binom(n, t);

        if (Size <= Threshold)
            throw new ArgumentException("Privacy threshold cannot be larger than n");
        if (Threshold == 0)
            throw new ArgumentException("Privacy threshold cannot be 0");

        Init();
    }

    /// <summary>
    /// Returns the number of shares this replicator can create.
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Returns the privacy threshold of this replicator.
    /// </summary>
    public int Threshold { get; }

    /// <summary>
    /// Returns the total number of additive shares used when creating a secret sharing.
    /// </summary>
    public int AdditiveShareSize { get; }

    /// <summary>
    /// The size of an individual share.
    /// </summary>
    public int ShareSize { get; }

    /// <summary>
    /// The size of a share in bytes.
    /// </summary>
    public int ShareSizeBytes => ShareSize * 8;

    /// <summary>
    /// Returns the combination corresponding to the given index.
    /// </summary>
    /// <param name="idx">The index to query.</param>
    /// <returns>(Sorted) combination corresponding to this index.</returns>
    public List<int> Combination(int idx) => mCombinations[idx];

    /// <summary>
    /// Returns the index corresponding to the given combination.
    /// </summary>
    /// <param name="combination">(Sorted) combination to query.</param>
    /// <returns>Index corresponding to this combination.</returns>
    public int RevComb(List<int> combination) => mRevComb[combination];

    /// <summary>
    /// Returns the index set for a particular replicated share.
    /// </summary>
    /// <param name="id">The replicated share index.</param>
    /// <returns>The index set for a replicated share.</returns>
    public IndexSet IndexSetFor(int id) => mLookup[id];

    /// <summary>
    /// Number of elements which differ between two shares.
    /// </summary>
    /// <returns>Number of elements which differ between two shares.</returns>
    public int DifferenceSize() => mDifferenceSize;

    /// <summary>
    /// Read a single share from a byte pointer.
    /// </summary>
    /// <param name="buffer">A pointer to some bytes.</param>
    /// <returns>A replicated share.</returns>
    public void ShareToBytes(List<RingElement> share, byte[] buffer)
    {
        ToBytes(buffer, share);
    }

    public static void ToBytes(byte[] buffer, List<RingElement> vector)
    {
        int elementSize = ByteSize<RingElement>();
        int totalSize = elementSize * vector.Count;

        if (buffer.Length < totalSize)
            throw new ArgumentException("Buffer size is insufficient.");

        for (int i = 0; i < vector.Count; ++i)
        {
            byte[] elementBytes = JsonSerializer.SerializeToUtf8Bytes(vector[i]);
            Array.Copy(elementBytes, 0, buffer, i * elementSize, elementSize);
        }
    }

    private static int ByteSize<T>()
    {
        return System.Runtime.InteropServices.Marshal.SizeOf<T>();
    }

    public void SharesToBytes(List<List<RingElement>> shares, byte[] buffer)
    {
        int offset = 0;
        foreach (var share in shares)
        {
            ShareToBytes(share, buffer.Skip(offset).ToArray());
            offset += ShareSizeBytes;
        }
    }

    public RingElement Reconstruct(List<List<RingElement>> shares)
    {
        var redundant = ComputeRedundantAddShares(shares);
        Mp61 secret = new Mp61(123);
        var additiveShares = new List<RingElement>(AdditiveShareSize);

        for (int i = 0; i < AdditiveShareSize; ++i)
        {
            secret.Value = secret + redundant[i][0].Value;
        }

        return secret;
    }

    public RingElement ErrorDetection(List<List<RingElement>> shares)
    {
        var redundant = ComputeRedundantAddShares(shares);
        Mp61 secret = default;
        var additiveShares = new List<RingElement>(AdditiveShareSize);

        Mp61 comparison;
        for (int i = 0; i < AdditiveShareSize; ++i)
        {
            // Check that all received shares are equal
            comparison = redundant[i][0];
            foreach (var elt in redundant[i])
            {
                if (!elt.Equals(comparison))
                {
                    throw new InvalidOperationException("Inconsistent shares");
                }
            }

            secret.Value =secret + comparison.Value;
        }

        return secret;
    }

    public List<List<Mp61>> ComputeRedundantAddShares(List<List<RingElement>> shares)
    {
        var redundant = new List<List<Mp61>>(AdditiveShareSize);
        for (int i = 0; i < AdditiveShareSize; ++i)
        {
            redundant[i] = new List<Mp61>(Size - Threshold);
        }

        for (int partyIdx = 0; partyIdx < Size; ++partyIdx)
        {
            for (int j = 0; j < ShareSize; ++j)
            {
                int shrIdx = mLookup[partyIdx][j];
                redundant[shrIdx].Add((Mp61) shares[partyIdx][j]);
            }
        }

        return redundant;
    }

    private void Init()
    {
        int k = Size - Threshold;
        int m = Size;
        List<int> combination = new List<int>(k);
        CombinationsAndSets.NthCombination(combination, 0, m);
        mLookup = new List<IndexSet>(Size);
        for (int i = 0; i < Size; ++i)
        {
            mLookup[i] = new IndexSet();
            mLookup[i].Capacity = ShareSize;
        }

        int shareIdx = 0;
        mCombinations = new List<List<int>>(AdditiveShareSize);
        do
        {
            // Fill in mCombinations
            mCombinations.Add(new List<int>(combination));
            // Fill in mRevComb
            mRevComb.Add(new List<int>(combination), shareIdx);
            foreach (int partyIdx in combination)
            {
                mLookup[partyIdx].Add(shareIdx);
            }

            shareIdx++;
        } while (CombinationsAndSets.NextCombination(combination, m, k));

        int d = 0;
        SetOperations.Difference(mLookup[0], mLookup[1], (int idx) => { d++; });
        mDifferenceSize = d;
    }

    public List<List<RingElement?>> AdditiveShare(RingElement secret, PRG prg)
    {
        List<RingElement?> additiveShares = AdditiveSharing.ShareAdditive(secret, AdditiveShareSize, prg);
        List<List<RingElement?>> shares = new List<List<RingElement?>>(Size);
        for (int i = 0; i < Size; ++i)
        {
            List<RingElement?> share = new List<RingElement?>();
            share.Capacity = ShareSize;
            IndexSet iset = IndexSetFor(i);
            foreach (int index in iset)
            {
                share.Add(additiveShares[index]);
            }

            shares.Add(share);
        }

        return shares;
    }
}