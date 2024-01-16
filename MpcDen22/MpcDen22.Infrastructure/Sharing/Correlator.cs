// using MpcDen22.Infrastructure.CommonModels;
// using MpcDen22.Infrastructure.Sharing.Models;
//

using MpcDen22.Infrastructure.CommonModels;
using MpcDen22.Infrastructure.Sharing.Models;

namespace MpcDen22.Infrastructure.Sharing;

// нужен чтобы генерировать случайные числа
public class Correlator
{
    private readonly int mId;
    private readonly List<PRG> mOwnPRGs;
    private readonly List<List<PRG>> mRandPRGs;
    private readonly Replicator<Mp61> mReplicator;
    private int mSize;
    private readonly int mThreshold;
    private List<PRG> mZeroPRGs;

    public Correlator(int id, Replicator<Mp61> replicator)
    {
        mReplicator = replicator;
        mId = id;
        mThreshold = replicator.Threshold;
        mSize = replicator.Size;
        mOwnPRGs = new List<PRG>(mReplicator.AdditiveShareSize);
        mRandPRGs = new List<List<PRG>>(2 * mThreshold + 1);
        Init();
    }

    public RandomShare GenZeroShareDummy()
    {
        var output = new RandomShare();

        // Set the additive share to 0
        output.AddShare = new Mp61(0);

        // Replicated share with zeros
        List<Replicator<Mp61>> zeroRepShare = Enumerable.Repeat(new Mp61(0), mReplicator.ShareSize).ToList();

        // Fill in the vector of replicated shares with zero rep shares
        for (var i = 0; i < 2 * mThreshold + 1; i++) output.RepAddShares.Add(zeroRepShare);

        return output;
    }

    public RandomShare GenRandomShareDummy()
    {
        var output = new RandomShare();

        // Set the additive share to 0
        output.AddShare = new Field(0);

        // Replicated share with zeros
        List<Field> zeroRepShare = Enumerable.Repeat(new Field(0), mReplicator.ShareSize()).ToList();

        // Set the replicated share to 0
        output.RepShare = zeroRepShare;

        // Fill in the vector of replicated shares with zero rep shares
        for (var i = 0; i < 2 * mThreshold + 1; i++) output.RepAddShares.Add(zeroRepShare);

        return output;
    }

    public RandomShare GenRandomShare()
    {
        var output = new RandomShare();
        int elementSize = Field.ByteSize();
        var buf = new byte[elementSize];

        // Set the additive share
        output.AddShare = new Field(0);

        // Only parties in U have additive shares
        if (mId < 2 * mThreshold + 1)
            // Get the additive share by adding the PRGs obtained when Pi
            // shared its own key
            for (var i = 0; i < mReplicator.AdditiveShareSize(); i++)
            {
                mOwnPRGs[i].Next(buf);
                output.AddShare += Field.FromBytes(buf);
            }

        // Set the replicated share of each additive share
        // and of the secret
        output.RepShare = new List<Field>(new Field[mReplicator.ShareSize()]);
        output.RepAddShares = new List<List<Field>>(new List<Field>[2 * mThreshold + 1]);

        for (var shrIdx = 0; shrIdx < mReplicator.ShareSize(); shrIdx++)
        {
            output.RepShare[shrIdx] = new Field(0);

            for (var idxInU = 0; idxInU < 2 * mThreshold + 1; idxInU++)
            {
                mRandPRGs[idxInU][shrIdx].Next(buf);
                output.RepAddShares[idxInU].Add(Field.FromBytes(buf));
                output.RepShare[shrIdx] += output.RepAddShares[idxInU][shrIdx];
            }
        }

        return output;
    }

    private void Init()
    {
        // Initializes the internal PRGs to default
        var PRGs = Enumerable.Repeat(new PRG(), mReplicator.ShareSize()).ToList();
        for (var j = 0; j < 2 * mThreshold + 1; j++) mRandPRGs[j] = PRGs;
    }
}