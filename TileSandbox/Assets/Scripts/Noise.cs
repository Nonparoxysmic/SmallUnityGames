using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

public class Noise
{
    readonly byte[] seed;
    readonly MD5 hashAlgorithm;

    public Noise(int seed)
    {
        this.seed = BitConverter.GetBytes(seed);
        hashAlgorithm = MD5.Create();
    }

    public float Value(int x, int y)
    {
        IEnumerable<byte> coordBytes = BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y));
        byte[] hashInput = seed.Concat(coordBytes).ToArray();
        uint hashBytes = BitConverter.ToUInt32(hashAlgorithm.ComputeHash(hashInput), 0);
        float output = (float)hashBytes / uint.MaxValue;
        return output == 1 ? 0 : output;
    }
}
