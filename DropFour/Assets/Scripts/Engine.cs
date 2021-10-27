using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public int Depth { get; set; }
    public int Output { get; set; }

    bool isRunning;
    Dictionary<int, int> moveScores;
    System.Random random;

    void Awake()
    {
        random = new System.Random();
    }


    public int RandomMove(GameBoard board)
    {
        int[] validMoves = board.ValidMoves();
        return validMoves[random.Next(0, validMoves.Length)];
    }

    public void StartThinking(GameBoard board, float seconds)
    {
        moveScores = new Dictionary<int, int>();
        int[] validMoves = board.ValidMoves();
        foreach (int move in validMoves)
        {
            moveScores.Add(move, 0);
        }
        isRunning = true;
        StartCoroutine(HandleSearch(board, seconds));
    }

    IEnumerator HandleSearch(GameBoard board, float seconds)
    {
        var t = new Thread(() => Search(board));
        t.Start();
        yield return new WaitForSeconds(seconds);
        isRunning = false;
    }

    void Search(GameBoard board)
    {
        for (int i = 1; i <= 42 - board.MovesMade; i++)
        {
            if (!isRunning) { return; }
            SearchToDepth(board, i);
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
        foreach (int move in validMoves)
        {
            if (board.CurrentPlayer == 0)
            {
                board.MakeMove(move);
                if (AlphaBetaSearch(board, alpha, beta, depthRemaining - 1, out int eval))
                {
                    if (eval >= beta)
                    {
                        board.UnmakeLastMove();
                        score = beta;
                        return true;
                    }
                    if (eval > alpha)
                    {
                        alpha = eval;
                    }
                }
                else
                {
                    board.UnmakeLastMove();
                    score = 0;
                    return false;
                }
                board.UnmakeLastMove();
                score = alpha;
                return true;
            }
            else
            {
                board.MakeMove(move);
                if (AlphaBetaSearch(board, alpha, beta, depthRemaining - 1, out int eval))
                {
                    if (eval <= alpha)
                    {
                        board.UnmakeLastMove();
                        score = alpha;
                        return true;
                    }
                    if (eval < beta)
                    {
                        beta = eval;
                    }
                }
                else
                {
                    board.UnmakeLastMove();
                    score = 0;
                    return false;
                }
                board.UnmakeLastMove();
                score = beta;
                return true;
            }
        }
        score = 0;
        return false;
    }

    int EvaluatePosition(GameBoard board)
    {
        return 0;  // TEMPORARY
    }
}
