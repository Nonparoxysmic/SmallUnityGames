using System;

public class NeuralNetwork
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "'new(...)' not available in Unity 2019")]
    readonly static Random random = new Random();

    public int layerCount;
    public int[] layerSizes;
    public float[][] neurons;
    public float[][] biases;
    public float[][][] weights;

    public NeuralNetwork() { }

    public NeuralNetwork(int[] layerSizes)
    {
        layerCount = layerSizes.Length;

        if (layerSizes[layerCount - 1] != 1)
        {
            throw new ArgumentException();
        }

        this.layerSizes = new int[layerCount];
        for (int i = 0; i < layerCount; i++)
        {
            this.layerSizes[i] = layerSizes[i];
        }

        neurons = new float[layerCount][];
        for (int i = 0; i < layerCount; i++)
        {
            neurons[i] = new float[layerSizes[i]];
        }

        biases = new float[layerCount][];
        biases[0] = new float[0];
        for (int i = 1; i < layerCount; i++)
        {
            biases[i] = new float[layerSizes[i]];
            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = GenerateInitialBias();
            }
        }

        weights = new float[layerCount][][];
        weights[0] = new float[0][];
        for (int i = 1; i < layerCount; i++)
        {
            weights[i] = new float[layerSizes[i]][];
            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[layerSizes[i - 1]];
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = GenerateInitialWeight();
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork networkToCopy)
    {
        layerCount = networkToCopy.layerCount;

        layerSizes = new int[layerCount];
        for (int i = 0; i < layerCount; i++)
        {
            layerSizes[i] = networkToCopy.layerSizes[i];
        }

        neurons = new float[layerCount][];
        for (int i = 0; i < layerCount; i++)
        {
            neurons[i] = new float[layerSizes[i]];
        }

        biases = new float[layerCount][];
        biases[0] = new float[0];
        for (int i = 1; i < layerCount; i++)
        {
            biases[i] = new float[layerSizes[i]];
            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = networkToCopy.biases[i][j];
            }
        }

        weights = new float[layerCount][][];
        weights[0] = new float[0][];
        for (int i = 1; i < layerCount; i++)
        {
            weights[i] = new float[layerSizes[i]][];
            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[layerSizes[i - 1]];
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = networkToCopy.weights[i][j][k];
                }
            }
        }
    }

    public int CalculateOutput(int[] input)
    {
        if (input == null)
        {
            throw new ArgumentNullException();
        }
        if (input.Length != layerSizes[0])
        {
            throw new ArgumentException();
        }

        for (int j = 0; j < input.Length; j++)
        {
            neurons[0][j] = input[j];
        }
        for (int i = 1; i < layerCount; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0;
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    value += weights[i][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = value + biases[i][j];
                if (i < layerCount - 1)
                {
                    neurons[i][j] = ActivationFunction(neurons[i][j]);
                }
            }
        }

        return (int)neurons[layerCount - 1][0];
    }

    float ActivationFunction(float input)
    {
        if (input < 0) { return 0; }
        return input;
    }

    float GenerateInitialBias()
    {
        return random.Next(-10, 11);  // TEMPORARY
    }

    float GenerateInitialWeight()
    {
        return random.Next(-100, 101);  // TEMPORARY
    }
}
