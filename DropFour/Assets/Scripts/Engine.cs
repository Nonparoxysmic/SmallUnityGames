using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "'new(...)' not available in Unity 2019")]
    readonly static System.Random random = new System.Random();

    public int Depth { get; set; }
    public int Output { get; set; }
    public int Strength { get; set; }

    [Header("Strength 1")]
    [SerializeField] bool useNeuralNetwork1;
    [SerializeField] [Range(1, 42)] int depthLimit1;
    [SerializeField] [Range(0.1f, 5)] float thinkTime1;

    [Header("Strength 2")]
    [SerializeField] bool useNeuralNetwork2;
    [SerializeField] [Range(1, 42)] int depthLimit2;
    [SerializeField] [Range(0.1f, 5)] float thinkTime2;

    [Header("Strength 3")]
    [SerializeField] bool useNeuralNetwork3;
    [SerializeField] [Range(1, 42)] int depthLimit3;
    [SerializeField] [Range(0.1f, 5)] float thinkTime3;

    [HideInInspector] public NeuralNetworkHandler neuralNetworkHandler;

    [HideInInspector] public int depthLimit;
    [HideInInspector] public int outputScore;
    [HideInInspector] public float thinkTime;

    bool isRunning;
    bool useNnet;
    Dictionary<int, int> moveScores;

    void Awake()
    {
        neuralNetworkHandler = GetComponent<NeuralNetworkHandler>();
    }

    public int RandomMove(GameBoard board)
    {
        int[] validMoves = board.ValidMoves();
        return validMoves[random.Next(0, validMoves.Length)];
    }

    public void StartThinking(GameBoard board)
    {
        UpdateStrength();
        moveScores = new Dictionary<int, int>();
        int[] validMoves = board.ValidMoves();
        foreach (int move in validMoves)
        {
            moveScores.Add(move, 0);
        }
        outputScore = 0;
        isRunning = true;
        StartCoroutine(HandleSearch(board));
    }

    void UpdateStrength()
    {
        switch (Strength)
        {
            case 1:
                useNnet = useNeuralNetwork1;
                depthLimit = depthLimit1;
                thinkTime = thinkTime1;
                break;
            case 2:
                useNnet = useNeuralNetwork2;
                depthLimit = depthLimit2;
                thinkTime = thinkTime2;
                break;
            case 3:
                useNnet = useNeuralNetwork3;
                depthLimit = depthLimit3;
                thinkTime = thinkTime3;
                break;
        }
    }

    IEnumerator HandleSearch(GameBoard board)
    {
        var t = new Thread(() => Search(board));
        t.Start();
        yield return new WaitForSeconds(thinkTime);
        isRunning = false;
    }

    void Search(GameBoard board)
    {
        Depth = 0;
        Output = 0;
        Thread.Sleep(50);
        for (int i = 1; i <= 42 - board.MovesMade && i <= depthLimit; i++)
        {
            if (!isRunning) { return; }
            SearchToDepth(board, i);
            if (outputScore == int.MaxValue || outputScore == int.MinValue) { return; }
        }
    }

    void SearchToDepth(GameBoard board, int depth)
    {
        IOrderedEnumerable<KeyValuePair<int, int>> orderedMoves;
        if (board.CurrentPlayer == 0)
        {
            orderedMoves = from entry in moveScores
                           orderby entry.Value descending
                           select entry;
        }
        else
        {
            orderedMoves = from entry in moveScores
                           orderby entry.Value ascending
                           select entry;
        }

        foreach (var kvp in orderedMoves)
        {
            board.MakeMove(kvp.Key);
            if (AlphaBetaSearch(board, int.MinValue, int.MaxValue, depth - 1, out int score))
            {
                moveScores[kvp.Key] = score;
            }
            board.UnmakeLastMove();
            Output = BestMove(moveScores, board.CurrentPlayer);
            outputScore = moveScores[Output];
            if ((board.CurrentPlayer == 0 && outputScore == int.MaxValue) ||
                (board.CurrentPlayer == 1 && outputScore == int.MinValue)) { break; }
            if (!isRunning) { return; }
        }
        Depth = depth;
    }

    int BestMove(Dictionary<int, int> moveScores, int player)
    {
        var highestScoreMoves = new List<int>();
        int highestScore = int.MinValue;
        var lowestScoreMoves = new List<int>();
        int lowestScore = int.MaxValue;
        foreach (var kvp in moveScores)
        {
            if (kvp.Value > highestScore)
            {
                highestScoreMoves = new List<int>();
            }
            if (kvp.Value >= highestScore)
            {
                highestScoreMoves.Add(kvp.Key);
                highestScore = kvp.Value;
            }
            if (kvp.Value < lowestScore)
            {
                lowestScoreMoves = new List<int>();
            }
            if (kvp.Value <= lowestScore)
            {
                lowestScoreMoves.Add(kvp.Key);
                lowestScore = kvp.Value;
            }
        }
        if (player == 0)
        {
            return highestScoreMoves[random.Next(0, highestScoreMoves.Count)];
        }
        return lowestScoreMoves[random.Next(0, lowestScoreMoves.Count)];
    }

    bool AlphaBetaSearch(GameBoard board, int alpha, int beta, int depthRemaining, out int score)
    {
        if (board.HasConnectedFour(0))
        {
            score = int.MaxValue;
            return true;
        }
        if (board.HasConnectedFour(1))
        {
            score = int.MinValue;
            return true;
        }
        if (board.MovesMade == 42)
        {
            score = 0;
            return true;
        }
        if (depthRemaining == 0)
        {
            score = EvaluatePosition(board);
            return true;
        }
        if (!isRunning)
        {
            score = 0;
            return false;
        }

        int[] validMoves = board.ValidMoves();
        if (board.CurrentPlayer == 0)
        {
            int value = int.MinValue;
            foreach (int move in validMoves)
            {
                board.MakeMove(move);
                if (AlphaBetaSearch(board, alpha, beta, depthRemaining - 1, out int eval))
                {
                    board.UnmakeLastMove();
                    value = Math.Max(value, eval);
                    if (value >= beta) { break; }
                    alpha = Math.Max(alpha, value);
                }
                else
                {
                    board.UnmakeLastMove();
                    score = 0;
                    return false;
                }
            }
            score = value;
            return true;
        }
        else
        {
            int value = int.MaxValue;
            foreach (int move in validMoves)
            {
                board.MakeMove(move);
                if (AlphaBetaSearch(board, alpha, beta, depthRemaining - 1, out int eval))
                {
                    board.UnmakeLastMove();
                    value = Math.Min(value, eval);
                    if (value <= alpha) { break; }
                    beta = Math.Min(beta, value);
                }
                else
                {
                    board.UnmakeLastMove();
                    score = 0;
                    return false;
                }
            }
            score = value;
            return true;
        }
    }

    int EvaluatePosition(GameBoard board)
    {
        if (neuralNetworkHandler == null || !useNnet) { return 0; }
        return neuralNetworkHandler.EvaluatePosition(board);
    }
}
