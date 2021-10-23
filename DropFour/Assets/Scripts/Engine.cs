using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public int Output { get; set; }
    
    bool isRunning;

    public int RandomMove(GameBoard board)
    {
        int[] validMoves = board.ValidMoves();
        return validMoves[UnityEngine.Random.Range(0, validMoves.Length)];
    }

    public void StartThinking(GameBoard board, float seconds)
    {
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
        // Test code for debugging threads.
        for (int i = 0; true; i++)
        {
            Debug.Log("Searching " + i + " seconds");
            Output = i;
            if (!isRunning)
            {
                return;
            }
            Thread.Sleep(1000);
        }
    }
}
