namespace MpcDen22.Infrastructure.Input
{
    public class Input
    {
        private readonly Network mNetwork;
        private readonly ShrManipulator mManipulator;
        private readonly InputSetup.Correlator mCorrelator;
        private readonly int mId;
        private readonly int mSize;
        private readonly List<List<Shr>> mSharesToReceive;
        private readonly List<Field> mSharesToDistribute;

        public Input(Network network, ShrManipulator manipulator, InputSetup.Correlator correlator)
        {
            mNetwork = network ?? throw new ArgumentNullException(nameof(network));
            mManipulator = manipulator ?? throw new ArgumentNullException(nameof(manipulator));
            mCorrelator = correlator ?? throw new ArgumentNullException(nameof(correlator));
            mId = network.Id();
            mSize = network.Size();
            mSharesToReceive = new List<List<Shr>>(mSize);
            mSharesToDistribute = new List<Field>();
        }

        public void Prepare(Field secret)
        {
            var mask = mCorrelator.GetMask();
            mSharesToDistribute.Add(secret - mask);
            PrepareToReceive(mId);
        }

        public void Prepare(IEnumerable<Field> secrets)
        {
            foreach (var secret in secrets)
            {
                Prepare(secret);
            }
        }

        public void PrepareToReceive(int id)
        {
            mSharesToReceive[id].Add(mCorrelator.GetMaskShare(id));
        }

        public void PrepareToReceive(int id, int n)
        {
            for (int i = 0; i < n; i++)
            {
                PrepareToReceive(id);
            }
        }

        public List<List<Shr>> Run()
        {
            var timerSend = new Timer("Input_send");
            timerSend.Start();
            for (int i = 0; i < mSize; i++)
            {
                if (mSharesToDistribute.Count > 0)
                {
                    // not a proper broadcast.
                    mNetwork.Send(i, mSharesToDistribute);
                }
            }
            timerSend.Stop();

            var timerRecvAddConstant = new Timer("Input_recv_add_constant");
            timerRecvAddConstant.Start();
            var output = new List<List<Shr>>(mSize);
            for (int i = 0; i < mSize; i++)
            {
                List<Shr> maskedShares = mSharesToReceive[i];
                List<Field> masked = mNetwork.Recv(i, maskedShares.Count);

                var temp = new List<Shr>();
                for (int j = 0; j < masked.Count; j++)
                {
                    temp.Add(mManipulator.AddConstant(maskedShares[j], masked[j]));
                }

                output.Add(temp);
            }
            timerRecvAddConstant.Stop();

            return output;
        }
    }
}
