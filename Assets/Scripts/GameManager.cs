using UnityEngine;


/// <summary>
/// Manager of the whole game.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region References:

    /// <summary>
    /// Instancia del GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// Referencia al UIManager.
    /// </summary>
    private UIManager _UIManager;
    /// <summary>
    /// Referencia al LevelManager.
    /// </summary>
    private LevelManager _LevelManager;

    #endregion

    #region Properties:

    /// <summary>
    /// Enum de estados de juego. 
    /// START = Menu de inicio del juego.
    /// MENU = Menu donde se elige el numero de rondas y los modelos de IA de cada jugador.
    /// LEVEL = El juego en si gestionado por el LevelManager.
    /// END = Pantalla con los resultados finales de todas las partidas.
    /// </summary>
    public enum GameStates { START, MENU, LEVEL, END }
    /// <summary>
    /// Estado actual de juego.
    /// </summary>
    private GameStates _currentState;
    /// <summary>
    /// Siguiente estado del juego.
    /// </summary>
    private GameStates _nextState;
    /// <summary>
    /// Juegos maximos que se van a jugar.
    /// </summary>
    private int _nGames = 1;
    /// <summary>
    /// Juegos que se han jugado.
    /// </summary>
    private int _gamesDone = 0;
    /// <summary>
    /// Veces que ha ganado el jugador 1.
    /// </summary>
    private int _player1Wins = 0;
    /// <summary>
    /// Veces que ha ganado el jugador 2.
    /// </summary>
    private int _player2Wins = 0;

    #endregion

    #region Awake and Update:

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        switch (_currentState)
        {
            case GameStates.START:
                break;
            case GameStates.MENU:
                break;
            case GameStates.LEVEL:
                break;
            case GameStates.END:
                break;
        }
    }

    #endregion

    #region Register methods:
    public void RegisterUIManager(UIManager uiManager)
    {
        _UIManager = uiManager;
    }

    public void RegisterLevelManager(LevelManager levelManager)
    {
        _LevelManager = levelManager;
    }

    #endregion

    #region GameStates Machine and methods:

    public void RequestStateChange(GameStates newState)
    {
        ChangeState(newState);
    }

    public GameStates GetCurrentState()
    {
        return _currentState;
    }

    private void ChangeState(GameStates state)
    {
        _nextState = state;
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameStates.START:
                _currentState = _nextState;
                _UIManager.ChangeMenu(GameStates.MENU, LevelManager.LevelStates.NONE);
                _player1Wins = 0;
                _player2Wins = 0;
                _nGames = 1;
                _gamesDone = 0;
                LevelManager.Instance.ResetWinners();
                break;
            case GameStates.MENU:
                switch (_nextState)
                {
                    case GameStates.START:
                        _currentState = _nextState;
                        _UIManager.ChangeMenu(GameStates.START, LevelManager.LevelStates.NONE);
                        break;
                    case GameStates.LEVEL:
                        _currentState = _nextState;
                        _UIManager.ChangeMenu(GameStates.LEVEL, LevelManager.LevelStates.DRAW_CARDS);
                        _LevelManager.RequestStateChange(LevelManager.LevelStates.DRAW_CARDS);
                        break;
                }
                break;
            case GameStates.LEVEL:
                _currentState = _nextState;
                break;
            case GameStates.END:
                _currentState = _nextState;
                _UIManager.ChangeMenu(GameStates.START, LevelManager.LevelStates.NONE);
                break;
        }
        Debug.Log("[GAME MANAGER] Cambio de GameState a " + _nextState);
    }

    #endregion

    #region Notifications:

    public void NotifyGameIsOver(int player1Wins, int player2Wins)
    {
        _player1Wins = player1Wins;
        _player2Wins = player2Wins;

        _gamesDone++;

        if (_gamesDone < _nGames)
        {
            Debug.Log("[GAME MANAGER] Juego: " + _gamesDone + " de " + _nGames);

            _UIManager.ChangeMenu(GameStates.LEVEL, LevelManager.LevelStates.DRAW_CARDS);
            _LevelManager.RequestStateChange(LevelManager.LevelStates.DRAW_CARDS);
        }
        else
        {
            Debug.Log("[GAME MANAGER] Adios juego.");

            _UIManager.SetGameResultTexts(_player1Wins, _player2Wins, _nGames);
            _UIManager.ChangeMenu(GameStates.END, LevelManager.LevelStates.NONE);
            _LevelManager.RequestStateChange(LevelManager.LevelStates.NONE);
            ChangeState(GameStates.END);
        }
    }

    #endregion

    #region From UI:

    /// <summary>
    /// Settea desde la UI el numero de partidas que se van a hacer en el juego desde un boton.
    /// </summary>
    /// <param name="nGames"></param>
    public void SetNGames(int nGames)
    {
        Debug.Log("[GAME MANAGER] Se ponen " + nGames + " partidas.");
        _gamesDone = 0;
        _nGames = nGames;
    }

    #endregion
}