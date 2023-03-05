using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public UnityEvent_Bool showGuidesChanged;
    public UnityEvent_Int playerColorChanged;

    [SerializeField] GameObject gameOverPopup;
    [SerializeField] Text gameOverText;
    [SerializeField] GameObject restartButton;
    [SerializeField] SpriteRenderer alternateBackgroundSR;

    public float delayBetweenTurns;
    public float tokenAcceleration;
    public float tokenMaxSpeed;

    [HideInInspector] public bool isPaused;

    DebugOutput debugOutput;
    Engine computerA;
    Engine computerB;
    Engine moverOne;
    Engine moverTwo;
    GameBoard board;
    InputManager inputManager;
    SceneController sceneController;

    bool playerMoved;
    GameResult gameResult;
    GameState currentState;
    GameType gameType;
    int movesMade;
    int playerMovedSelection;

    void Awake()
    {
        board = new GameBoard();
        debugOutput = GetComponent<DebugOutput>();
        inputManager = GetComponent<InputManager>();
        showGuidesChanged = new UnityEvent_Bool();
        playerColorChanged = new UnityEvent_Int();
        if (!PlayerPrefs.HasKey("GameType"))
        {
            PlayerPrefs.SetInt("GameType", (int)GameType.RandomFirst);
        }
        if (!PlayerPrefs.HasKey("EngineOneStrength"))
        {
            PlayerPrefs.SetInt("EngineOneStrength", 1);
        }
        if (!PlayerPrefs.HasKey("EngineTwoStrength"))
        {
            PlayerPrefs.SetInt("EngineTwoStrength", 1);
        }
        if (!PlayerPrefs.HasKey("ShowDebugLog"))
        {
            PlayerPrefs.SetInt("ShowDebugLog", 0);
        }
        if (!PlayerPrefs.HasKey("ShowPlacementGuides"))
        {
            PlayerPrefs.SetInt("ShowPlacementGuides", 0);
        }
        ShowDebugText(PlayerPrefs.GetInt("ShowDebugLog") != 0);
        gameType = (GameType)PlayerPrefs.GetInt("GameType");
    }

    void Start()
    {
        ShowPlacementGuides(PlayerPrefs.GetInt("ShowPlacementGuides") != 0);
        computerA = GameObject.Find("Computer Player A").GetComponent<Engine>();
        computerB = GameObject.Find("Computer Player B").GetComponent<Engine>();
        sceneController = GameObject.Find("Scene Controller").GetComponent<SceneController>();
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
        if (currentState != GameState.PlayerTurn || isPaused) return;
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
        playerColorChanged.Invoke(board.CurrentPlayer);
        yield return new WaitForSeconds(delayBetweenTurns);
        currentState = GameState.PlayerTurn;
        ShowSelection(true);
        yield return new WaitUntil(() => playerMoved);
        currentState = GameState.Processing;
        playerMoved = false;
        ShowSelection(false);
        debugOutput.AddText((movesMade + 1) + ". Player " + (board.CurrentPlayer + 1)
            + " in column " + (playerMovedSelection + 1));
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
            yield return new WaitForSeconds(delayBetweenTurns);
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0066:Convert switch statement to expression", Justification = "Switch expression not available in Unity 2019")]
    IEnumerator ComputerTurn(Engine engine)
    {
        // Start turn
        playerColorChanged.Invoke(board.CurrentPlayer);
        currentState = GameState.ComputerTurn;
        // Wait if paused
        if (isPaused)
        {
            yield return new WaitUntil(() => !isPaused);
        }
        // Update engine strength from preferences
        if (engine == computerA)
        {
            engine.Strength = PlayerPrefs.GetInt("EngineOneStrength");
        }
        else engine.Strength = PlayerPrefs.GetInt("EngineTwoStrength");
        // Determine chosen move
        int chosenMove;
        if (engine.Strength == 0)
        {
            yield return new WaitForSeconds(Math.Max(0.1f, delayBetweenTurns));
            engine.Depth = 0;
            chosenMove = engine.RandomMove(board);
        }
        else if (movesMade == 0)
        {
            yield return new WaitForSeconds(Math.Max(0.1f, delayBetweenTurns));
            engine.Depth = 0;
            chosenMove = 3;
        }
        else if (movesMade == 1)
        {
            yield return new WaitForSeconds(Math.Max(0.1f, delayBetweenTurns));
            engine.Depth = 0;
            int rand = UnityEngine.Random.Range(0, 4);
            int[] choices;
            switch (board.MoveList[0])
            {
                case 0:
                    choices = new int[] { 1, 3 };
                    break;
                case 1:
                    choices = new int[] { 2 };
                    break;
                case 2:
                    choices = new int[] { 2, 3, 4, 5 };
                    break;
                case 4:
                    choices = new int[] { 1, 2, 3, 4 };
                    break;
                case 5:
                    choices = new int[] { 4 };
                    break;
                case 6:
                    choices = new int[] { 3, 5 };
                    break;
                default:
                    choices = new int[] { 3 };
                    break;
            }
            chosenMove = choices[rand % choices.Length];
        }
        else
        {
            engine.StartThinking(board);
            yield return new WaitForSeconds(Math.Max(engine.thinkTime + 0.1f, delayBetweenTurns));
            chosenMove = engine.Output;
        }
        // Get score of chosen move
        string scoreString = engine.outputScore.ToString();
        if (engine.outputScore == int.MaxValue)
        {
            scoreString = "Red Victory";
        }
        if (engine.outputScore == int.MinValue)
        {
            scoreString = "Yellow Victory";
        }
        if (!engine.useNnet && engine.outputScore == 0)
        {
            scoreString = "??";
        }
        if (engine.Strength == 0 || movesMade == 0 || movesMade == 1)
        {
            scoreString = "--";
        }
        // Log chosen move
        debugOutput.AddText((movesMade + 1) + ". Player " + (board.CurrentPlayer + 1) + " in column "
            + (chosenMove + 1) + " (Score: " + scoreString + ", Depth: " + engine.Depth + ")");
        // Make chosen move
        board.MakeMove(chosenMove);
        inputManager.selectionActivated.Invoke(chosenMove, movesMade & 1);
        // End turn
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
            yield return new WaitForSeconds(delayBetweenTurns);
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
        debugOutput.AddText("GAME OVER");
        if (gameResult == GameResult.InProgress)
        {
            SetGameOverText("ERROR: Game result not set.");
        }
        else if (gameResult == GameResult.Tie)
        {
            SetGameOverText("TIED GAME");
        }
        else if (gameType == GameType.TwoPlayer || gameType == GameType.TwoComputer)
        {
            string winner = movesMade % 2 == 0 ? "YELLOW" : "RED";
            SetGameOverText(winner + " VICTORY");
        }
        else
        {
            Engine winner = movesMade % 2 == 0 ? moverTwo : moverOne;
            if (winner == null)
            {
                SetGameOverText("YOU WIN");
            }
            else SetGameOverText("YOU LOSE");
        }
        gameOverPopup.SetActive(true);
        restartButton.SetActive(true);
        sceneController.ChangeExitButtonText("Exit to Main Menu");
    }

    void SetGameOverText(string text)
    {
        debugOutput.AddText(text);
        gameOverText.text = "GAME OVER" + Environment.NewLine + Environment.NewLine + text;
    }

    public void ShowDebugText(bool doShow)
    {
        debugOutput.debugOutputText.enabled = doShow;
        debugOutput.debugBackgroundImage.enabled = doShow;
    }

    public void ShowPlacementGuides(bool doShow)
    {
        showGuidesChanged.Invoke(doShow);
        if (gameType == GameType.TwoComputer)
        {
            alternateBackgroundSR.enabled = false;
        }
        else alternateBackgroundSR.enabled = doShow;
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
