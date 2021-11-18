using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class NeuralNetworkHandler : MonoBehaviour
{
    public TextAsset neuralNetValues;

    NeuralNetwork nnet;

    void Start()
    {
        if (neuralNetValues == null) { return; }
        var xmlSerializer = new XmlSerializer(typeof(NeuralNetwork));
        var stream = new MemoryStream(neuralNetValues.bytes);
        nnet = (NeuralNetwork)xmlSerializer.Deserialize(stream);
    }

    public int EvaluatePosition(GameBoard board)
    {
        if (neuralNetValues == null) { return 0; }
        return 0;  // TEMPORARY
    }
}
