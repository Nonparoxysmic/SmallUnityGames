using System;
using System.Collections;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
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
        gameResult = GameResult.InProgress;
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
        else StartCoroutine(ComputerTurn(moverOne, 0));
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
                playerMoved = true;
                playerMovedSelection = currentSelection;
            }
        }
    }

    IEnumerator PlayerTurn()
    {
        currentState = GameState.PlayerTurn;
        ShowSelection(true);
        while (!playerMoved)
        {
            yield return null;
        }
        currentState = GameState.Processing;
        playerMoved = false;
        ShowSelection(false);
        board.MakeMove(playerMovedSelection);
        inputManager.selectionActivated.Invoke(playerMovedSelection, movesMade & 1);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
            gameResult = GameResult.PlayerWin;
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
            else StartCoroutine(ComputerTurn(nextMover, 1));
        }
    }

    IEnumerator ComputerTurn(Engine engine, float delaySeconds)
    {
        currentState = GameState.ComputerTurn;
        engine.StartThinking(board);
        yield return new WaitForSeconds(Math.Max(engine.thinkTime + 0.1f, delaySeconds));
        int chosenMove = engine.Output;
        Debug.Log("Chosen move: " + chosenMove + ", Depth completed: " + engine.Depth);
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        yield return new WaitForSeconds(0.5f);
        if (board.HasConnectedFour(movesMade & 1))
        {
            currentState = GameState.Ending;
            gameResult = GameResult.ComputerWin;
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
            else StartCoroutine(ComputerTurn(nextMover, 1));
        }
    }

    void GameOver()
    {
        switch (gameResult)
        {
            case GameResult.PlayerWin:
                Debug.Log("PLAYER WINS");
                break;
            case GameResult.ComputerWin:
                Debug.Log("COMPUTER WINS");
                break;
            case GameResult.Tie:
                Debug.Log("TIED GAME");
                break;
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
    PlayerWin,
    ComputerWin,
    Tie
}
