using System;

public class NeuralNetwork
{
    public int[] layerSizes;

    public NeuralNetwork()
    {

    }

    public NeuralNetwork(int[] layerSizes)
    {
        this.layerSizes = new int[layerSizes.Length];
        for (int i = 0; i < layerSizes.Length; i++)
        {
            this.layerSizes[i] = layerSizes[i];
        }
    }
}
