using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Input
{
    public class InputSetup
    {
        public class Correlator
        {
            private readonly List<List<PRG>> mPrgs;
            private readonly List<PRG> mPrg;
            private readonly int mShareSize;

            public Correlator(List<List<PRG> prgs, List<PRG> prg, int shareSize)
            {
                mPrgs = prgs ?? throw new ArgumentNullException(nameof(prgs));
                mPrg = prg ?? throw new ArgumentNullException(nameof(prg));
                mShareSize = shareSize;
            }

            public Mp61 GetMask()
            {
                Mp61 v = new Field();
                foreach (var prg in mPrg)
                {
                    v += GetRandomElement(prg);
                }
                return v;
            }

            public Shr GetMaskShare(int id)
            {
                var prgId = mPrgs[id];
                List<Field> share = new List<Field>(mShareSize);
                foreach (var prg in prgId)
                {
                    share.Add(GetRandomElement(prg));
                }
                return share;
            }
        }

        private readonly Network mNetwork;
        private readonly Mp61 mReplicator;
        private readonly PRG mPrg;

        public InputSetup(Network network, Lib.SecretSharing.Replicator<Field> replicator, PRG prg)
        {
            mNetwork = network ?? throw new ArgumentNullException(nameof(network));
            mReplicator = replicator ?? throw new ArgumentNullException(nameof(replicator));
            mPrg = prg ?? throw new ArgumentNullException(nameof(prg));
        }

        public Correlator Run()
        {
            return new Correlator(GenPrgs(), mPrg, mReplicator.ShareSize());
        }

        private List<List<PRG>> GenPrgs()
        {
            List<List<PRG>> prgs = new List<List<PRG>>();
            for (int i = 0; i < mNetwork.Size(); i++)
            {
                List<PRG> prgsForI = new List<PRG>(mReplicator.ShareSize());
                foreach (var v in mReplicator.Share(FieldElementToPrg(mPrg)))
                {
                    prgsForI.Add(FieldElementToPrg(v));
                }
                prgs.Add(prgsForI);
            }
            return prgs;
        }

        private static Mp61 GetRandomElement(PRG prg)
        {
            byte[] buffer = new byte[Field.ByteSize()];
            prg.Next(buffer, Field.ByteSize());
            return Field.FromBytes(buffer);
        }

        private static PRG FieldElementToPrg(Mp61 element)
        {
            byte[] buffer = new byte[Math.Max(PRG.SeedSize(), Field.ByteSize())];
            element.ToBytes(buffer);
            return new PRG(buffer);
        }
    }
}
