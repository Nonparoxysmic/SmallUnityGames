using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Noise
{
    readonly Vector2Int[] cornerVectors = new Vector2Int[]
    {
        new Vector2Int(0, 0),
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1)
    };

    readonly byte[] seed;
    readonly int horzScale;
    readonly int vertScale;
    readonly MD5 hashAlgorithm;

    public Noise(int seed, int horzScale, int vertScale)
    {
        this.seed = BitConverter.GetBytes(seed);
        this.horzScale = horzScale == 0 ? 1 : Math.Abs(horzScale);
        this.vertScale = vertScale == 0 ? 1 : Math.Abs(vertScale);
        hashAlgorithm = MD5.Create();
    }

    public float Value(int x, int y)
    {
        if (horzScale == 1 && vertScale == 1) { return SeededRandomValue(x, y); }

        int cellX = x.Floor(horzScale);
        int cellY = y.Floor(vertScale);
        float sum = 0;
        foreach (Vector2Int cornerVector in cornerVectors)
        {
            int cornerX = cellX + horzScale * cornerVector.x;
            int cornerY = cellY + vertScale * cornerVector.y;
            float xFraction = 1 - (float)Math.Abs(cornerX - x) / horzScale;
            float yFraction = 1 - (float)Math.Abs(cornerY - y) / vertScale;
            sum += xFraction * yFraction * SeededRandomValue(cornerX, cornerY);
        }
        return sum;
    }

    float SeededRandomValue(int x, int y)
    {
        IEnumerable<byte> coordBytes = BitConverter.GetBytes(x).Concat(BitConverter.GetBytes(y));
        byte[] hashInput = seed.Concat(coordBytes).ToArray();
        uint hashOutput = BitConverter.ToUInt32(hashAlgorithm.ComputeHash(hashInput), 0);
        float output = (float)hashOutput / uint.MaxValue;
        return output == 1 ? 0 : output;
    }
}
