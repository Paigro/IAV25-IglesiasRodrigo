using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region References:

    /// <summary>
    /// Instancia del GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// Referencia al mazo.
    /// </summary>
    public Deck _deck = null;
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
    public enum GameStates { START,MENU, LEVEL, END }
    /// <summary>
    /// Estado actual de juego.
    /// </summary>
    private GameStates _currentState;
    /// <summary>
    /// Siguiente estado del juego.
    /// </summary>
    private GameStates _nextState;

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

        _deck = this.AddComponent<Deck>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_deck != null)
        {
            _deck.WriteDeck();

        }
    }

    #endregion

    #region Initial methods:
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
                if (_nextState == GameStates.LEVEL)
                    _currentState = _nextState;
                // cambiar las cosas necesarias.
                break;
            case GameStates.LEVEL:
                // cambiar las cosas necesarias.
                break;
            case GameStates.END:
                // cambiar las cosas necesarias.
                break;
        }
        Debug.Log("//------Cambio de GameState a " + _nextState);
    }

    #endregion
}