using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


/// <summary>
/// Manager that manage the cards game. 
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Constants:

    private const float HAND_CARDS_OFFSET = 1.5f;

    #endregion

    #region References:

    /// <summary>
    /// Instancia del LevelManager.
    /// </summary>
    public static LevelManager Instance { get; private set; }
    /// <summary>
    /// Referencia al VisualCardsManager.
    /// </summary>
    private VisualCardsManager _VisualCardsManager;
    /// <summary>
    /// Referencia al prefab de la mano del jugador 1.
    /// </summary>
    [SerializeField]
    private GameObject _player1PF = null;
    /// <summary>
    /// Referencia al prefab de la mano del jugador 2.
    /// </summary>
    [SerializeField]
    private GameObject _player2PF = null;
    /// <summary>
    /// Referencia al prefab de la mesa.
    /// </summary>
    [SerializeField]
    private GameObject _tablePF = null;
    /// <summary>
    /// Referencia al prefab del mazo.
    /// </summary>
    [SerializeField]
    private GameObject _deckPF = null;
    /// <summary>
    /// Referencia a la pial del jugador 1.
    /// </summary>
    [SerializeField]
    private GameObject _stack1 = null;
    /// <summary>
    /// Referencia a la pila del jugador 2.
    /// </summary>
    [SerializeField]
    private GameObject _stack2 = null;



    /// <summary>
    /// Referencia al mazo.
    /// </summary>
    private Deck _deck = null;
    /// <summary>
    /// Referencia al jugador 1.
    /// </summary>
    private Player _player1 = null;
    /// <summary>
    /// Referencia al jugador 2.
    /// </summary>
    private Player _player2 = null;
    /// <summary>
    /// Referecia a la mesa.
    /// </summary>
    private Table _table = null;

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

    #region Parameters:

    /// <summary>
    /// Para controlar el turno de jugadores.
    /// </summary>
    private bool _playerIsPlaying = false;
    /// <summary>
    /// Puntos que lleva el jugador 1.
    /// </summary>
    private int _player1Points = 0;
    /// <summary>
    /// Victorias que lleva el jugador 1.
    /// </summary>
    private int _player1Wins = 0;
    /// <summary>
    /// Puntos que lleva el jugador 2.
    /// </summary>
    private int _player2Points = 0;
    /// <summary>
    /// Victorias que lleva el jugador 2.
    /// </summary>
    private int _player2Wins = 0;
    /// <summary>
    /// El jugador que empieza la ronda (pues se van alternando). TRUE = P1 y FALSE = P2.
    /// </summary>
    private bool _startingPlayer = true;
    /// <summary>
    /// Numero de rondas que lleva la partida
    /// </summary>
    private int _nRounds;
    /// <summary>
    /// Ultimo jugador que ha puesto en la mesa. Si (-1 => Nadie), (0 => Jugador 2), (1 => Jugador 1). En paridad con el bool _startingPlayer
    /// </summary>
    private int _lastPlayerThatPutInTable = -1;

    #endregion

    #region Timers:

    [SerializeField]
    private float _timer = 0;
    private bool _usingTimer = false;

    public void SetTimer(float newTime = 2f)
    {
        _usingTimer = true;
        _timer = newTime;
        Debug.Log("[LEVEL MANAGER] Empieza un timer de " + newTime + " segundos.");
    }

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

        _currentState = LevelStates.NONE;
    }
    void Start()
    {
        // Registro del LevelManager en el GameManager.
        GameManager.Instance.RegisterLevelManager(this);

        // Creacion del mazo.
        _deck = Instantiate(_deckPF).GetComponent<Deck>();
        _deck.gameObject.name = "Deck";
        _deck.gameObject.transform.position = new Vector3(-8.0f, 0, 0);
        _deck.gameObject.GetComponent<VisualCard>().SetSprite(Resources.Load<Sprite>("Cards/P0"));

        // Creacion jugador 1.
        _player1 = Instantiate(_player1PF).GetComponent<Player>();
        _player1.SetPlayable(false);
        _player1.SetHand(_player1.GetComponent<Hand>());
        _player1.SetAIModel(new MCTS());
        _player1.gameObject.name = "Player1";
        _player1.gameObject.transform.position = new Vector3(-HAND_CARDS_OFFSET, -3.8f, 0);

        // Creacion jugador 2.
        _player2 = Instantiate(_player2PF).GetComponent<Player>();
        _player2.SetPlayable(false);
        _player2.SetHand(_player2.GetComponent<Hand>());
        _player2.SetAIModel(new UtilityAI());
        _player2.gameObject.name = "Player2";
        _player2.gameObject.transform.position = new Vector3(-HAND_CARDS_OFFSET, 3.8f, 0);

        // Creacion de la mesa.
        _table = Instantiate(_tablePF).GetComponent<Table>();
        _table.gameObject.name = "Table";
        _table.gameObject.transform.position = new Vector3(0, 0, 0);
    }
    void Update()
    {
        if (!_usingTimer)
        {
            // Para el cambio de estados.
            switch (_currentState)
            {
                case LevelStates.DRAW_CARDS:
                    SetTimer(4);
                    if (_nRounds == 0) // Si es la ronda inicial entonces 
                    {
                        SetUpGame();
                    }
                    else
                    {
                        DrawCardsState();
                    }
                    break;
                case LevelStates.PLAYER:
                    if (!_playerIsPlaying)
                    {
                        // Cuando el mazo se queda sin cartas y los jugadores tambien se pasa a la siguiente ronda pasando por los resultados.
                        if (_deck.GetDeckCount() == 0 && _player1.GetPlayerHand().GetHandCount() == 0 && _player2.GetPlayerHand().GetHandCount() == 0)
                        {
                            CalculateRoundPoints();
                            ResetThings();
                            // TODO: Reset del _VCM.
                            ChangeState(LevelStates.ROUND_RESULTS);
                        }
                        // Cuando los jugadores se quedan sin cartas en la mano.
                        else if (_player1.GetPlayerHand().GetHandCount() == 0 && _player2.GetPlayerHand().GetHandCount() == 0)
                        {
                            _startingPlayer = !_startingPlayer; // Cambiamos el jugador que empieza porque en La Escoba se alternan.
                            ChangeState(LevelStates.DRAW_CARDS); // Cambiamos a robar cartas.
                        }
                        else
                        {
                            PlayerTurnState();
                        }
                    }
                    break;
                case LevelStates.ROUND_RESULTS:
                    // UI. BOTON PARA PROSEGUIR SI JUGADOR HUMANO, SINO POR TIEMPO.
                    if (_player1Points >= 21 || _player1Points >= 21)
                    {
                        if (_player1Points >= 21)
                            _player1Wins += 1;
                        else
                            _player2Wins += 1;
                        Debug.Log("[LEVEL MANAGER] Fin de partida con JUGADOR1: " + _player1Points + "-JUGADOR2: " + _player2Points);
                        ChangeState(LevelStates.LEVEL_RESULTS);
                    }
                    break;
                case LevelStates.LEVEL_RESULTS:
                    // UI. BOTON PARA PROSEGUIR SI JUGADOR HUMANO, SINO POR TIEMPO.
                    ChangeState(LevelStates.EXIT);
                    break;
                case LevelStates.EXIT:
                    break;
            }
        }
        else
        {
            //Debug.Log("[LEVEL MANAGER] --timer.");
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _usingTimer = false;
                Debug.Log("[LEVEL MANAGER] Se acabo el timer.");
            }
        }
    }

    #endregion

    #region Register methods and important getters:

    public void RegisterVisualCardsManager(VisualCardsManager visualCardsManager)
    {
        _VisualCardsManager = visualCardsManager;
    }

    #endregion

    #region LevelStates Machine and methods:

    /// <summary>
    /// Cambia el estado desde fuera del objeto.
    /// </summary>
    /// <param name="newState"> Estado al que se quiere pasar.</param>
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
            case LevelStates.NONE:
                if (_nextState == LevelStates.DRAW_CARDS)
                    _currentState = _nextState;
                break;
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
                    GameManager.Instance.RequestStateChange(GameManager.GameStates.END);
                break;
        }
        Debug.Log("[LEVEL MANAGER] Cambio de LevelState a " + _nextState);
    }

    #endregion

    #region LevelStates Methods:

    private void SetUpGame()
    {
        // Creamos el mazo y lo barajamos.
        _deck.CreateDeck();
        //_deck.ReadDeckFromTxt();
        _deck.Shuffle(10);
        _VisualCardsManager.SpawnAllDeck(_deck.transform);

        // Cogemos las manos de los jugadores.
        Hand _player1Hand = _player1.GetPlayerHand();
        Hand _player2Hand = _player2.GetPlayerHand();

        // Limpiamos la mesa y las manos y pilas de cada jugador.
        _player1Hand.ResetHand();
        _player2Hand.ResetHand();
        _table.ClearTable();

        // Cogemos a quien empieza y a quien reparte.
        Hand starting = _startingPlayer ? _player1Hand : _player2Hand;
        Hand dealer = _startingPlayer ? _player2Hand : _player1Hand;

        // Se reparten 3 cartas a cada jugador dependiendo de quien empiece.
        PlayersDrawCards(starting, dealer);

        // Se reparte la mesa
        for (int i = 0; i < 4; i++)
        {
            Card card = _deck.DrawCard();
            _table.AddCardToTable(card);

            _VisualCardsManager.MoveCardTo(card.GetCardName(), _table.gameObject.transform, new Vector2(i * HAND_CARDS_OFFSET, 0.0f));
        }

        //// Comprobar escobas en mesa inicial.
        //int initBrooms = _table.CheckInitBrooms();
        //if (initBrooms > 0)
        //{
        //    dealer.AddBroom(initBrooms); // Si empieza el jugador 1 entonces reparte el 2 y se la lleva el 2.
            
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Card tableCard = _table.GetCardsInTable()[i];
        //        dealer.AddCardToStack(tableCard); // Metemos la carta a la pila.
        //        _table.RemoveCardToTable(tableCard); // Quitamos la carta de la mesa.
        //        _VisualCardsManager.MoveCardTo(tableCard.GetCardName(), dealer.transform);
        //    }
        //}

        //  Avanzamos de ronda.
        _nRounds++;
        // Siguiente estado.
        ChangeState(LevelStates.PLAYER);
    }

    private void DrawCardsState()
    {
        // Cogemos las manos de los jugadores.
        Hand _player1Hand = _player1.GetPlayerHand();
        Hand _player2Hand = _player2.GetPlayerHand();

        // Cogemos a quien empieza y a quien reparte.
        Hand starting = _startingPlayer ? _player1Hand : _player2Hand;
        Hand dealer = _startingPlayer ? _player2Hand : _player1Hand;

        // Se reparten 3 cartas a cada jugador dependiendo de quien empiece.
        PlayersDrawCards(starting, dealer);
    }

    private void PlayerTurnState()
    {
        // Cogemos al jugador que le toca.
        Player player = _startingPlayer ? _player1 : _player2;

        // Juega.
        _playerIsPlaying = true;
        player.PlayTurn(new List<Card>(_table.GetCardsInTable())); // Para evitr que se pueda modificar la lista original se le pasa otra nueva.


    }

    private void ResetThings()
    {
        // Reseteamos el numero de rondas.
        _nRounds = 0;

        // Limpiamos la mesa.
        _table.ClearTable();

        // Limpiamos a los jugadores.
        _player1.ResetHand();
        _player2.ResetHand();

        // Limpiamos el mazo.
        _deck.ResetDeck();

        // Reseteamos el ultimo jugador en poner carta en mesa.
        _lastPlayerThatPutInTable = -1;
    }

    public void NotifiyPlayerEndTurn(List<Card> move)
    {
        // Ejecutamos el movimiento.
        ExecutePlayerMove(move);
        // Cambiamos el turno al siguiente jugador.
        _startingPlayer = !_startingPlayer;
        // Cambiamos el bool para que todo prosiga. PAIGRO AQUI: Esto mejor con un temporaizador para que se muestren las cartas, etc...
        _playerIsPlaying = false;
    }

    #endregion

    #region Cards:

    private void PlayersDrawCards(Hand starting, Hand dealer)
    {
        // 3 cartas para cada jugador.
        for (int i = 0; i < 3; i++)
        {
            Card card = _deck.DrawCard();
            _VisualCardsManager.MoveCardTo(card.GetCardName(), _player1.gameObject.transform, new Vector2(i * HAND_CARDS_OFFSET, 0.0f));
            starting.AddCardToHand(card);
            card = _deck.DrawCard();
            _VisualCardsManager.MoveCardTo(card.GetCardName(), _player2.gameObject.transform, new Vector2(i * HAND_CARDS_OFFSET, 0.0f));
            dealer.AddCardToHand(card);
        }
    }

    private void ExecutePlayerMove(List<Card> move)
    {
        // Cogemos la mano del jugador que acaba de jugar.
        Hand playerHand = _startingPlayer ? _player1.GetPlayerHand() : _player2.GetPlayerHand();

        Card cardUsed = move[0];
        if (move.Count == 1) // Ha dejado carta en la mesa: hay que quitarla de la mano.
        {
            _table.AddCardToTable(cardUsed); // Metemos la carta a la mesa.
            playerHand.PlayCard(cardUsed); // Quitamos la carta de la mano.
            _VisualCardsManager.MoveCardTo(cardUsed.GetCardName(), _table.transform); // Movemos la carta. PAIGRO AQUI: aunque alomejor habria que enseñarla primero.
        }
        else if (move.Count > 1) // Coge cartas: hay que quitarla de la mano y mover las de la mesa a la pila.
        {
            Transform objetive = _startingPlayer ? _stack1.transform : _stack2.transform; // Setteamos el objetivo del movimiento.

            playerHand.PlayCard(cardUsed); // Quitamos la carta de la mano.
            playerHand.AddCardToStack(cardUsed); // Pero la metemos a la pila.
            _VisualCardsManager.MoveCardTo(cardUsed.GetCardName(), objetive);

            // Movemos las cartas de la mesa a la pila y las quitamos de la mesa.
            for (int i = 1; i < move.Count; i++)
            {
                Card tableCard = move[i];
                playerHand.AddCardToStack(tableCard); // Metemos la carta a la pila.
                _table.RemoveCardToTable(tableCard); // Quitamos la carta de la mesa.
                _VisualCardsManager.MoveCardTo(tableCard.GetCardName(), objetive); // La movemos.
            }

            _lastPlayerThatPutInTable = _startingPlayer ? 1 : 0; // Nos guardamos el ulitmo jugador que ha puesto en la mesa.
        }
        SetTimer();
    }

    #endregion

    #region Points:

    /// <summary>
    /// Al acabar la ronda se calcula los puntos que ha conseguida cada jugador.
    /// </summary>
    private void CalculateRoundPoints()
    {
        List<Card> stack1 = _player1.GetPlayerHand().GetCardsInStack();
        List<Card> stack2 = _player2.GetPlayerHand().GetCardsInStack();

        //------Sacamos el numero de cartas de cada jugador:
        int nCards1 = _player1.GetPlayerHand().GetStackCount();
        int nCards2 = _player2.GetPlayerHand().GetStackCount();

        //------Sacamos el numero de 7s de cada jugador, los oros y si tiene el 7 de oros:
        int nSevens1 = 0;
        int nSevens2 = 0;
        int nGolds1 = 0;
        int nGolds2 = 0;

        // del jugador 1.
        for (int i = 0; i < stack1.Count; i++)
        {
            // Oros.
            if (stack1[i].GetCardSuit() == 'O')
            {
                nGolds1++;
            }
            // Sietes.
            if (stack1[i].GetCardNumber() == 7)
            {
                nSevens1++;
                // Siete de oros.
                if (stack1[i].GetCardSuit() == 'O')
                {
                    // PUNTO DE SIETE DE OROS.
                    Debug.Log("[LEVEL MANAGER] Jugador 1 tiene el siete de oros.");
                    _player1Points++;
                }
            }
        }
        // del jugador 2.
        for (int i = 0; i < stack2.Count; i++)
        {
            // Oros.
            if (stack2[i].GetCardSuit() == 'O')
            {
                nGolds2++;
            }
            // Sietes.
            if (stack2[i].GetCardNumber() == 7)
            {
                nSevens2++;
                // Siete de oros.
                if (stack2[i].GetCardSuit() == 'O')
                {
                    // PUNTO DE SIETE DE OROS.
                    Debug.Log("[LEVEL MANAGER] Jugador 2 tiene el siete de oros.");
                    _player2Points++;
                }
            }
        }

        //------Hacemos las comparaciones y sumamos los puntos:
        // PUNTO DE CARTAS.
        if (nCards1 > nCards2) // Jugador 1 tiene mas cartas.
        {
            _player1Points += 1;
        }
        else if (nCards2 > nCards1) // Jugador 2 tiene mas cartas.
        {
            _player2Points += 1;
        }
        // else: empate a cartas, nadie suma.

        // PUNTO DE SIETES.
        if (nSevens1 > nSevens2) // Jugador 1 tiene mas sietes.
        {
            _player1Points += 1;
        }
        else if (nSevens2 > nSevens1) // Jugador 2 tiene mas sietes.
        {
            _player2Points += 1;
        }
        // else: empate a sietes, nadie suma.

        // PUNTO DE OROS.
        if (nGolds1 > nGolds2) // Jugador 1 tiene mas oros.
        {
            _player1Points += 1;
        }
        else if (nGolds2 > nGolds1) // Jugador 2 tiene mas oros.
        {
            _player2Points += 1;
        }
        // else: empate a oros, nadie suma.

        //------Sumamos escobas:
        // PUNTOS DE ESCOBAS.
        _player1Points += _player1.GetPlayerHand().GetBrooms();
        _player2Points += _player2.GetPlayerHand().GetBrooms();

        Debug.Log("[LEVEL MANAGER] ------JUGADOR1: " + _player1Points + "------" + _player2Points + "------");
    }

    /// <summary>
    /// Devuelve los puntos de cada jugador en una tupla.
    /// </summary>
    /// <returns>Una tupla con los puntos de cada jugador.</returns>
    private Tuple<int, int> GetPlayerPoints()
    {
        return new Tuple<int, int>(_player1Points, _player2Points);
    }

    #endregion
}