using MpcDen22.Infrastructure.Sharing;

namespace MpcDen22.Infrastructure.Checks;

public class CheckService : ICheckService
{
}

public struct CompressedCheckData
{
    // The shares the given party sent to P1 across the
    // multiplications
    public Field SharesSentToP1;

    // For each party, the shares P1 received across all
    // multiplications
    public List<Field> SharesRecvByP1;

    // Reconstructions received from P1
    public Field ValuesRecvFromP1;

    // For each party, rep share of msg^i
    public List<Shr> Msgs;

    public CompressedCheckData(ShrManipulator m)
    {
        SharesSentToP1 = 0;
        ValuesRecvFromP1 = 0;
        SharesRecvByP1 = new List<Field>(2 * m.GetReplicator().Threshold() + 1);
        Msgs = new List<Shr>(2 * m.GetReplicator().Threshold() + 1);

        for (int i = 0; i < 2 * m.GetReplicator().Threshold() + 1; i++)
        {
            Msgs.Add(new Shr(m.GetDoubleReplicator().ShareSize(), new Field(0)));
        }
    }
}

/**
 * @brief The interface of the Check protocol.
 */
public class Check
{
    private readonly ISharedNetwork mNetwork;
    private readonly Replicator<Field> mReplicator;
    private readonly int mId;
    private readonly int mThreshold;
    private readonly int mSize;
    private readonly ShrManipulator mManipulator;
    private readonly int mCount;
    private readonly CheckData mCheckData;
    private CompressedCheckData mCompressedCD;
    private readonly List<Field> mValuesToSend;
    private readonly List<Field> mDigestsToSend;
    private readonly List<Field> mValuesReceived;
    private readonly List<Field> mDigestsReceived;
    private readonly PRG mPRG;
    private readonly List<Field> mRandomCoefficients;

    public Check(ISharedNetwork network, Replicator<Field> replicator, ShrManipulator manipulator, CheckData cd)
    {
        mNetwork = network ?? throw new ArgumentNullException(nameof(network));
        mReplicator = replicator ?? throw new ArgumentNullException(nameof(replicator));
        mManipulator = manipulator ?? throw new ArgumentNullException(nameof(manipulator));
        mId = network.Id();
        mThreshold = replicator.Threshold();
        mSize = network.Size();
        mCheckData = cd ?? throw new ArgumentNullException(nameof(cd));
        mCount = cd.counter;
        mCompressedCD = new CompressedCheckData(mManipulator);
        mValuesToSend = Enumerable.Repeat(new List<Field>(), mSize).ToList();
        mDigestsToSend = Enumerable.Repeat(new List<Field>(), mSize).ToList();
        mValuesReceived = Enumerable.Repeat(new List<Field>(), mSize).ToList();
        mDigestsReceived = Enumerable.Repeat(new List<Field>(), mSize).ToList();
        mPRG = new PRG(); // Instantiate PRG as needed
        mRandomCoefficients = new List<Field>();
    }

    // Omitted for now
    public void SetupPRG()
    {
        // Implement as needed
    }

    public void ComputeRandomCoefficients()
    {
        for (int multIdx = 0; multIdx < mCheckData.counter; multIdx++)
        {
            Field coeff = mPRG.Next(Field.ByteSize());
            mRandomCoefficients.Add(coeff);
        }
    }

    // Omitted for now
    public void AgreeOnTranscript()
    {
        // Implement as needed
    }

    public void PrepareLinearCombinations()
    {
        if (0 < mId && mId < 2 * mThreshold - 1)
        {
            for (int multIdx = 0; multIdx < mCheckData.counter; multIdx++)
            {
                mCompressedCD.SharesSentToP1 += mRandomCoefficients[multIdx] * mCheckData.SharesSentToP1[multIdx];
                mCompressedCD.ValuesRecvFromP1 += mRandomCoefficients[multIdx] * mCheckData.ValuesRecvFromP1[multIdx];
            }
        }
        else if (mId == 0)
        {
            for (int multIdx = 0; multIdx < mCheckData.counter; multIdx++)
            {
                for (int partyIdx = 0; partyIdx < 2 * mThreshold + 1; partyIdx++)
                {
                    mCompressedCD.SharesRecvByP1[partyIdx] +=
                        mRandomCoefficients[multIdx] * mCheckData.SharesRecvByP1[partyIdx][multIdx];
                }
            }
        }
    }

    // Omitted for now
    public void PrepareMsgs()
    {
        // Implement as needed
    }

    public void ReconstructMsgs()
    {
        for (int recvId = 0; recvId < mSize; ++recvId)
        {
            // Send length
            int size = mValuesToSend[recvId].Count;
            byte[] sizeBytes = BitConverter.GetBytes(size);
            mNetwork.SendBytes(recvId, sizeBytes);

            // Send values
            mNetwork.Send(recvId, mValuesToSend[recvId]);

            // Send length
            size = mDigestsToSend[recvId].Count;
            sizeBytes = BitConverter.GetBytes(size);
            mNetwork.SendBytes(recvId, sizeBytes);

            // Send hashes
            mNetwork.Send(recvId, mDigestsToSend[recvId]);
        }

        for (int senderId = 0; senderId < mSize; ++senderId)
        {
            // Receive length
            byte[] sizeBytes = mNetwork.RecvBytes(senderId, 4).ToArray();
            int size = BitConverter.ToInt32(sizeBytes, 0);

            // Receive values
            mNetwork.Recv(senderId, size);

            // Receive length
            sizeBytes = mNetwork.RecvBytes(senderId, 4).ToArray();
            size = BitConverter.ToInt32(sizeBytes, 0);

            // Receive hashes
            mNetwork.Recv(senderId, size);
        }
    }
}