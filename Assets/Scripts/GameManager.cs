using UnityEngine;

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
    /// Si se esta jugando un partida o no.
    /// </summary>
    private bool _gameInProgress = false;
    /// <summary>
    /// 
    /// </summary>
    private int _nGames;

    #endregion

    #region Awake, Start and Update:

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case GameStates.START:
                break;
            case GameStates.MENU:
                break;
            case GameStates.LEVEL:
                if (!_gameInProgress)
                {
                    ChangeState(GameStates.END);
                }
                break;
            case GameStates.END:
                ChangeState(GameStates.START);
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
        //ui hace cosas.
        UpdateState();
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameStates.START:
                _currentState = _nextState;
                _UIManager.ChangeMenu(GameStates.MENU, LevelManager.LevelStates.NONE);
                break;
            case GameStates.MENU:
                _currentState = _nextState;
                _gameInProgress = true;
                _UIManager.ChangeMenu(GameStates.LEVEL, LevelManager.LevelStates.DRAW_CARDS);
                _LevelManager.RequestStateChange(LevelManager.LevelStates.DRAW_CARDS);
                // cambiar las cosas necesarias.
                break;
            case GameStates.LEVEL:
                // cambiar las cosas necesarias.
                break;
            case GameStates.END:
                // cambiar las cosas necesarias.
                break;
        }
        Debug.Log("[GAME MANAGER] Cambio de GameState a " + _nextState);
    }

    #endregion

    #region Notifications:

    public void NotifyGameIsOver()
    {
        _gameInProgress = false;
    }

    #endregion

    #region From UI:

    public void SetNGames(int nGames)
    {
        Debug.Log("[GAME MANAGER] Se ponen " + nGames + " partidas.");
        _nGames = nGames;
    }

    #endregion
}