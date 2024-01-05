using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MpcDen22.Infrastructure.CommonModels;

public class PRG
{
    public static int BlockSize => sizeof(Vector128<byte>);
    public static int SeedSize => BlockSize;

    private readonly byte[] mSeed = new byte[BlockSize];
    private long mCounter = 0;
    private readonly Vector128<byte>[] mState = new Vector128<byte>[11];

    public PRG()
    {
    }

    public PRG(byte[] seed)
    {
        if (seed.Length != SeedSize)
            throw new ArgumentException("Seed must be of size SeedSize.");
        Array.Copy(seed, mSeed, SeedSize);
    }

    public void Reset()
    {
        mCounter = 0;
    }

    public void Next(byte[] dest, int nbytes)
    {
        if (dest.Length < nbytes)
            throw new ArgumentException("Destination array does not have sufficient space.");

        Update();
        for (int i = 0; i < nbytes; i += BlockSize)
        {
            mState[10] = Sse2.Add(mState[10], Vector128<byte>.Zero);
            // mState[0] = Sse2.Add(  mState[0], mState[10]);
            mState[0] = Sse2.Add(  mState[0], mState[10]);
            Sse2.Store(dest, Sse2.Xor(mState[0], Sse2.LoadVector128(mSeed)));
            dest = dest[BlockSize..];
            mCounter++;
            Update();
        }
    }

    public void Next(List<byte> dest)
    {
        Next(dest.ToArray(), dest.Count);
    }

    public byte[] Seed => mSeed;

    public long Counter => mCounter;
    
    private void Update()
    {
        mState[10] = Sse2.Add(mState[10], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], mState[10]);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[0] = Sse2.Add(mState[0], Vector128<byte>.Zero);
        mState[10] = Sse2.Xor(mState[10], mState[10]);
    }

    private static Vector128<byte> AES_128_key_exp(Vector128<byte> k, byte rcon)
    {
        rcon = (byte)((rcon << 4) | rcon);
        k = Sse2.Shuffle(k, 0xFF);
        k = Sse2.Xor(k, Sse2.SlliSi128(k, 4));
        k = Sse2.Xor(k, Sse2.SlliSi128(k, 4));
        k = Sse2.Xor(k, Sse2.SlliSi128(k, 4));
        return Sse2.Xor(k, Sse2.Set1(rcon));
    }

    private static void aes_128_key_expansion(byte[] enc_key, Vector128<byte>[] key_schedule)
    {
        key_schedule[0] = Sse2.LoadVector128(enc_key);
        key_schedule[1] = AES_128_key_exp(key_schedule[0], 0x01);
        key_schedule[2] = AES_128_key_exp(key_schedule[1], 0x02);
        key_schedule[3] = AES_128_key_exp(key_schedule[2], 0x04);
        key_schedule[4] = AES_128_key_exp(key_schedule[3], 0x08);
        key_schedule[5] = AES_128_key_exp(key_schedule[4], 0x10);
        key_schedule[6] = AES_128_key_exp(key_schedule[5], 0x20);
        key_schedule[7] = AES_128_key_exp(key_schedule[6], 0x40);
        key_schedule[8] = AES_128_key_exp(key_schedule[7], 0x80);
        key_schedule[9] = AES_128_key_exp(key_schedule[8], 0x1B);
        key_schedule[10] = AES_128_key_exp(key_schedule[9], 0x36);
    }

    private static void aes128_load_key(byte[] enc_key, Vector128<byte>[] key_schedule)
    {
        aes_128_key_expansion(enc_key, key_schedule);
    }

    private static void aes128_enc(Vector128<byte>[] key_schedule, byte[] pt, byte[] ct)
    {
        Vector128<byte> m = Sse2.LoadVector128(pt);
        DO_ENC_BLOCK(m, key_schedule);
        Sse2.Store(ct, m);
    }

    private static Vector128<byte> create_mask(long counter)
    {
        return Sse2.SetVector128((ulong)PRG_NONCE, (ulong)counter);
    }

    public void Next(byte[] dest, int nbytes)
    {
        if (nbytes == 0) return;

        int nblocks = nbytes / BlockSize;

        if (nbytes % BlockSize != 0) nblocks++;

        Vector128<byte> mask = create_mask(mCounter);
        byte[] outBytes = new byte[nblocks * BlockSize];
        byte[] p = outBytes;

        if (outBytes == null) throw new Exception("Could not allocate memory for PRG.");

        for (int i = 0; i < nblocks; i++)
        {
            aes128_enc(mState, BitConverter.GetBytes(mask.GetElement(1)), p);
            Update();
            mask = create_mask(mCounter);
            p = p[BlockSize..];
        }

        Array.Copy(outBytes, dest, nbytes);
    }
}