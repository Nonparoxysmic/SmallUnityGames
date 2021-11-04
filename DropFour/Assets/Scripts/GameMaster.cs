using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public float delayBetweenTurns;
    public float tokenAcceleration;
    public float tokenMaxSpeed;

    Engine computerA;
    Engine computerB;
    Engine moverOne;
    Engine moverTwo;
    GameBoard board;
    InputManager inputManager;

    bool playerMoved;
    GameResult gameResult;
    GameState currentState;
    GameType gameType;
    int movesMade;
    int playerMovedSelection;

    void Awake()
    {
        board = new GameBoard();
        inputManager = GetComponent<InputManager>();
        if (!PlayerPrefs.HasKey("GameType"))
        {
            PlayerPrefs.SetInt("GameType", (int)GameType.RandomFirst);
        }
        gameType = (GameType)PlayerPrefs.GetInt("GameType");
    }

    void Start()
    {
        computerA = GameObject.Find("Computer Player A").GetComponent<Engine>();
        computerB = GameObject.Find("Computer Player B").GetComponent<Engine>();
        switch (gameType)
        {
            case GameType.RandomFirst:
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    moverOne = null;
                    moverTwo = computerA;
                }
                else
                {
                    moverOne = computerA;
                    moverTwo = null;
                }
                break;
            case GameType.PlayerFirst:
                moverOne = null;
                moverTwo = computerA;
                break;
            case GameType.ComputerFirst:
                moverOne = computerA;
                moverTwo = null;
                break;
            case GameType.TwoPlayer:
                moverOne = null;
                moverTwo = null;
                break;
            case GameType.TwoComputer:
                moverOne = computerA;
                moverTwo = computerB;
                break;
        }
        if (moverOne == null)
        {
            StartCoroutine(PlayerTurn());
        }
        else StartCoroutine(ComputerTurn(moverOne));
    }

    void ShowSelection(bool doShow)
    {
        inputManager.ShowSelection(doShow);
    }

    public void SelectionActivated(int currentSelection)
    {
        if (currentState != GameState.PlayerTurn) return;
        if (currentSelection >= 0 && currentSelection <= 6)
        {
            if (board.IsValidMove(currentSelection))
            {
                playerMovedSelection = currentSelection;
                playerMoved = true;
            }
        }
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(delayBetweenTurns);
        currentState = GameState.PlayerTurn;
        ShowSelection(true);
        yield return new WaitUntil(() => playerMoved);
        currentState = GameState.Processing;
        playerMoved = false;
        ShowSelection(false);
        board.MakeMove(playerMovedSelection);
        inputManager.selectionActivated.Invoke(playerMovedSelection, movesMade & 1);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
            gameResult = GameResult.WinLoss;
        }
        else if (board.MovesMade >= 42)
        {
            currentState = GameState.Ending;
            gameResult = GameResult.Tie;
        }
        movesMade++;
        if (currentState == GameState.Ending)
        {
            currentState = GameState.End;
            GameOver();
        }
        else
        {
            Engine nextMover = movesMade % 2 == 0 ? moverOne : moverTwo;
            if (nextMover == null)
            {
                StartCoroutine(PlayerTurn());
            }
            else StartCoroutine(ComputerTurn(nextMover));
        }
    }

    IEnumerator ComputerTurn(Engine engine)
    {
        currentState = GameState.ComputerTurn;
        engine.StartThinking(board);
        yield return new WaitForSeconds(Math.Max(engine.thinkTime + 0.1f, delayBetweenTurns));
        int chosenMove = engine.Output;
        Debug.Log("Chosen move: " + chosenMove + ", Depth completed: " + engine.Depth);
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
            gameResult = GameResult.WinLoss;
        }
        else if (board.MovesMade >= 42)
        {
            currentState = GameState.Ending;
            gameResult = GameResult.Tie;
        }
        movesMade++;
        if (currentState == GameState.Ending)
        {
            currentState = GameState.End;
            GameOver();
        }
        else
        {
            Engine nextMover = movesMade % 2 == 0 ? moverOne : moverTwo;
            if (nextMover == null)
            {
                StartCoroutine(PlayerTurn());
            }
            else StartCoroutine(ComputerTurn(nextMover));
        }
    }

    void GameOver()
    {
        if (gameResult == GameResult.InProgress)
        {
            Debug.LogError("Game result not set before game over.");
            return;
        }
        if (gameResult == GameResult.Tie)
        {
            Debug.Log("TIED GAME");
            return;
        }
        if (gameType == GameType.TwoPlayer || gameType == GameType.TwoComputer)
        {
            int winner = movesMade % 2 == 0 ? 2 : 1;
            Debug.Log("PLAYER " + winner + " WINS");
        }
        else
        {
            Engine winner = movesMade % 2 == 0 ? moverTwo : moverOne;
            if (winner == null)
            {
                Debug.Log("PLAYER WINS");
            }
            else Debug.Log("COMPUTER WINS");
        }
    }
}

public enum GameState
{
    Start,
    PlayerTurn,
    ComputerTurn,
    Processing,
    Ending,
    End
}

public enum GameResult
{
    InProgress,
    WinLoss,
    Tie
}
