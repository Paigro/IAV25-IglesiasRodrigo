using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

/// <summary>
/// Manager that manage the cards game. 
/// </summary>
public class LevelManager : MonoBehaviour
{

    #region References:

    /// <summary>
    /// Instancia del LevelManager.
    /// </summary>
    public static LevelManager Instance { get; private set; }
    /// <summary>
    /// El jugador que empieza la ronda pues se van alternando. TRUE = P1 y FALSE = P2.
    /// </summary>
    private bool _startingPlayer = true;
    /// <summary>
    /// Referencia a la mano del jugador 1.
    /// </summary>
    private Player _player1 = null;
    /// <summary>
    /// Referencia a la mano del jugador 2.
    /// </summary>
    private Player _player2 = null;
    /// <summary>
    /// Referencia a la mesa.
    /// </summary>
    private Table _table = null;
    /// <summary>
    /// Referencia al mazo.
    /// </summary>
    private Deck _deck = null;


    #endregion

    #region Properties:

    /// <summary>
    /// Enum de estados de partida. 
    /// DRAW_CARDS = Menu de inicio del juego.
    /// PLAYER = Turno del jugador.
    /// ROUND_RESULTS = Menu con los resultados de la ronda.
    /// LEVEL_RESULTS = Menu con los resultados de la partida.
    /// EXIT = Para ir al final del juego.
    /// </summary>
    public enum LevelStates { NONE, DRAW_CARDS, PLAYER, ROUND_RESULTS, LEVEL_RESULTS, EXIT }
    /// <summary>
    /// Estado actual de la partida.
    /// </summary>
    private LevelStates _currentState;
    /// <summary>
    /// Siguiente estado de la partida.
    /// </summary>
    private LevelStates _nextState;

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
    void Start()
    {
        // Registro del LevelManager en el GameManager.
        GameManager.Instance.RegisterLevelManager(this);

        // Creacion de
        _deck = new GameObject("Deck").AddComponent<Deck>();

        // Creacion jugador 1.
        _player1 = new GameObject("Player 1").AddComponent<Player>(); // PAIGRO AQUI: esto lo tendria que hacer el game manage creo.
        Hand player1Hand = _player1.gameObject.AddComponent<Hand>();
        _player1.SetPlayable(false);
        _player1.SetHand(player1Hand);
        //_player1.SetAIModel(null);

        // Creacion jugador 2.
        _player2 = new GameObject("Player 2").AddComponent<Player>();
        Hand player2Hand = _player2.gameObject.AddComponent<Hand>();
        _player2.SetPlayable(false);
        _player2.SetHand(player2Hand);
        //_player2.SetAIModel(null);

        // Creacion de la mesa.
        _table = new GameObject("Table").AddComponent<Table>();

        // El estado inicial es el de robar cartas.
        _currentState = LevelStates.DRAW_CARDS;
    }
    void Update()
    {
        switch (_currentState)
        {
            case LevelStates.DRAW_CARDS:
                DrawCardsState();
                break;
            case LevelStates.PLAYER:
                PlayerTurnState();
                break;

        }
    }

    #endregion

    #region LevelStates Machine and methods:
    public void RequestStateChange(LevelStates newState)
    {
        ChangeState(newState);
    }

    public LevelStates GetCurrentState()
    {
        return _currentState;
    }

    private void ChangeState(LevelStates state)
    {
        _nextState = state;
        switch (_currentState)
        {
            case LevelStates.DRAW_CARDS:
                if (_nextState == LevelStates.PLAYER)
                    _currentState = _nextState;
                break;
            case LevelStates.PLAYER:
                if (_nextState == LevelStates.PLAYER || _nextState == LevelStates.DRAW_CARDS || _nextState == LevelStates.ROUND_RESULTS)
                    _currentState = _nextState;
                break;
            case LevelStates.ROUND_RESULTS:
                if (_nextState == LevelStates.DRAW_CARDS || _nextState == LevelStates.LEVEL_RESULTS)
                    _currentState = _nextState;
                break;
            case LevelStates.LEVEL_RESULTS:
                if (_nextState == LevelStates.EXIT)
                    GameManager.Instance.RequestStateChange(GameStates.END);
                break;
        }
        Debug.Log("//------Cambio de LevelState a " + _nextState);
    }

    private void DrawCardsState()
    {
        // Creamos el mazo y lo barajamos.
        _deck.CreateDeck();
        _deck.Shuffle(10);
        _deck.WriteDeck(); // PAIGRO AQUI: eliminar cuando no haga falta.

        // Cogemos las manos de los jugadores.
        Hand _player1Hand = _player1.GetPlayerHand();
        Hand _player2Hand = _player2.GetPlayerHand();

        // Limpiamos la mesa y las manos y pilas de cada jugador.
        _player1Hand.ClearAll();
        _player2Hand.ClearAll();
        _table.ClearTable();

        // Cogemos a quien empieza y a quien reparte.
        Hand starting = _startingPlayer ? _player1Hand : _player2Hand;
        Hand dealer = _startingPlayer ? _player2Hand : _player1Hand;

        // Se reparten 3 cartas a cada jugador dependiendo de quien empiece.
        for (int i = 0; i < 3; i++)
        {
            starting.AddCardToHand(_deck.DrawCard());
            dealer.AddCardToHand(_deck.DrawCard());
        }

        // Se reparte la mesa
        for (int i = 0; i < 4; i++)
        {
            _table.AddCardToTable(_deck.DrawCard());
        }

        // Comprobar escobas en mesa inicial.
        dealer.AddBroom(_table.CheckInitBrooms()); // Si empieza el jugador 1 entonces reparte el 2 y se la lleva el 2.

        // Siguiente estado.
        ChangeState(LevelStates.PLAYER);
    }

    private void PlayerTurnState()
    {

    }

    #endregion
}
