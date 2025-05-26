using TMPro;
using UnityEngine;


/// <summary>
/// Manager to manage the UI of the game.
/// </summary>
public class UIManager : MonoBehaviour
{
    #region References:

    /// <summary>
    /// Instancia del UIManager.
    /// </summary>
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// Texto de las cartas que le quedan al deck.
    /// </summary>
    [SerializeField] private TMP_Text _deckRemainingCards;
    /// <summary>
    /// Texto que indica de quien es el turno.
    /// </summary>
    [SerializeField] private TMP_Text _turn;
    /// <summary>
    /// Texto que indica los resultados del jugador 1 al final de la ronda.
    /// </summary>
    [SerializeField] private TMP_Text _player1RoundResult;
    /// <summary>
    /// Texto que indica los resultados del jugador 2 al final de la ronda.
    /// </summary>
    [SerializeField] private TMP_Text _player2RoundResult;
    /// <summary>
    /// Texto que indica los resultados del jugador 1 al final de la partida.
    /// </summary>
    [SerializeField] private TMP_Text _player1LevelResult;
    /// <summary>
    /// Texto que indica los resultados del jugador 2 al final de la partida.
    /// </summary>
    [SerializeField] private TMP_Text _player2LevelResult;
    /// <summary>
    /// Texto que indica los resultados del jugador 1 al final del juego
    /// </summary>
    [SerializeField] private TMP_Text _player1GameResult;
    /// <summary>
    /// Texto que indica los resultados del jugador 2 al final del juego
    /// </summary>
    [SerializeField] private TMP_Text _player2GameResult;

    /// <summary>
    /// Referencia a la UI del menu inicial. Que tendra boton de inicio y boton de salir.
    /// </summary>
    [SerializeField] private GameObject _startMenu;
    /// <summary>
    /// Referencia a la UI del menu de opciones. Que tendra dos sliders para seleccionar los modelos de IA y otro para el numero de partidas totales.
    /// </summary>
    [SerializeField] private GameObject _menuMenu;
    /// <summary>
    /// Referencia a la UI del menu del nivel. Que tendra el indicativo de a que jugador le toca y el numero de cartas restantes en el mazo.
    /// </summary>
    [SerializeField] private GameObject _levelMenu;
    /// <summary>
    /// Referencia a la UI del menu de resultados de ronda.
    /// </summary>
    [SerializeField] private GameObject _roundEndMenu;
    /// <summary>
    /// Referencia a la UI del menu de resultados de partida.
    /// </summary>
    [SerializeField] private GameObject _levelEndMenu;
    /// <summary>
    /// Referencia a la UI del menu de resultados del juego en total. Que dendra los resultados, boton para volver al start y boton para salir.
    /// </summary>
    [SerializeField] private GameObject _endMenu;

    #endregion

    #region propiedades:

    /// <summary>
    /// Estado del GameManager actual.
    /// </summary>
    private GameManager.GameStates _activeGameMenu;
    /// <summary>
    /// Estado del LevelManager actual.
    /// </summary>
    private LevelManager.LevelStates _activeLevelMenu;

    #endregion

    #region Awake, Start and Update:

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
    void Start()
    {
        // Se registra al GameManager.
        GameManager.Instance.RegisterUIManager(this);

        // Activar solo el menu inicial al principio.
        _startMenu.SetActive(true);
        _menuMenu.SetActive(false);
        _levelMenu.SetActive(false);
        _roundEndMenu.SetActive(false);
        _levelEndMenu.SetActive(false);
        _endMenu.SetActive(false);
    }

    #endregion

    #region State Machine:

    public void ChangeMenu(GameManager.GameStates newGameMenu, LevelManager.LevelStates newLevelMenu)
    {
        _activeGameMenu = newGameMenu;
        _activeLevelMenu = newLevelMenu;

        switch (newGameMenu)
        {
            case GameManager.GameStates.START:
                _startMenu.SetActive(true);
                _menuMenu.SetActive(false);
                _levelMenu.SetActive(false);
                _roundEndMenu.SetActive(false);
                _levelEndMenu.SetActive(false);
                _endMenu.SetActive(false);
                break;
            case GameManager.GameStates.MENU:
                _startMenu.SetActive(false);
                _menuMenu.SetActive(true);
                _levelMenu.SetActive(false);
                _roundEndMenu.SetActive(false);
                _levelEndMenu.SetActive(false);
                _endMenu.SetActive(false);
                break;
            case GameManager.GameStates.LEVEL:
                switch (newLevelMenu)
                {
                    case LevelManager.LevelStates.DRAW_CARDS:
                    case LevelManager.LevelStates.PLAYER:
                        _startMenu.SetActive(false);
                        _menuMenu.SetActive(false);
                        _levelMenu.SetActive(true);
                        _roundEndMenu.SetActive(false);
                        _levelEndMenu.SetActive(false);
                        _endMenu.SetActive(false);
                        break;
                    case LevelManager.LevelStates.ROUND_RESULTS:
                        _startMenu.SetActive(false);
                        _menuMenu.SetActive(false);
                        _levelMenu.SetActive(false);
                        _roundEndMenu.SetActive(true);
                        _levelEndMenu.SetActive(false);
                        _endMenu.SetActive(false);
                        break;
                    case LevelManager.LevelStates.LEVEL_RESULTS:
                        _startMenu.SetActive(false);
                        _menuMenu.SetActive(false);
                        _levelMenu.SetActive(false);
                        _roundEndMenu.SetActive(false);
                        _levelEndMenu.SetActive(true);
                        _endMenu.SetActive(false);
                        break;
                }
                break;
            case GameManager.GameStates.END:
                _startMenu.SetActive(false);
                _menuMenu.SetActive(false);
                _levelMenu.SetActive(false);
                _roundEndMenu.SetActive(false);
                _levelEndMenu.SetActive(false);
                _endMenu.SetActive(true);
                break;
        }
    }

    #endregion

    #region Text methods:

    /// <summary>
    /// Actualiza el texto que indica el numero de cartas restantes del mazo.
    /// </summary>
    /// <param name="remainingCards"></param>
    public void UpdateDeckText(int remainingCards)
    {
        _deckRemainingCards.text = remainingCards.ToString();
    }

    /// <summary>
    /// Actualiza el texto que indica de quien es el turno.
    /// </summary>
    /// <param name="player"></param>
    public void UpdateTurnTest(int player)
    {
        _turn.text = "Turn: " + player.ToString();
    }

    /// <summary>
    /// Actualiza el texto de resultado de ronda con:
    /// Numero de puntos de cada jugador.
    /// Numero de cartas de cada jugador.
    /// Numero de sietes de cada jugador.
    /// Numero de oros de cada jugador.
    /// Numero de sietes de oros de cada jugador (aunque solo lo pueda tener 1.
    /// Numero de escobas de cada jugador.
    /// </summary>
    public void SetRoundResultTexts(int points1, int points2, int cards1, int cards2, int sevens1, int sevens2, int golds1, int golds2, int goldenSeven1, int goldenSeven2, int brooms1, int brooms2)
    {
        _player1RoundResult.text = "Player 1:     " + points1 + "         (" + cards1.ToString() + " )" + "         (" + sevens1.ToString() + " )" + "         (" + golds1.ToString() + " )" + "         (" + goldenSeven1.ToString() + " )" + "         (" + brooms1.ToString() + " )";
        _player2RoundResult.text = "Player 2:     " + points2 + "         (" + cards2.ToString() + " )" + "         (" + sevens2.ToString() + " )" + "         (" + golds2.ToString() + " )" + "         (" + goldenSeven2.ToString() + " )" + "         (" + brooms2.ToString() + " )";
    }

    /// <summary>
    /// Actualiza el texto de resultado de partida con:
    /// Numero de puntos finales de cada jugador.
    /// Y quien ha perdido y quien ganado.
    /// </summary>
    public void SetLevelResultTexts(int points1, int points2)
    {
        if (points1 == points2) // Empate.
        {
            _player1LevelResult.text = "Player 1:     " + points1 + "   (Tie)";
            _player2LevelResult.text = "Player 2:     " + points2 + "   (Tie)";
        }
        else // No empate.
        {
            string winner1 = points1 > points2 ? "(Winner)" : "(Loser)";
            string winner2 = points1 < points2 ? "(Winner)" : "(Loser)";

            _player1LevelResult.text = "Player 1:     " + points1 + "   " + winner1;
            _player2LevelResult.text = "Player 2:     " + points2 + "   " + winner2;
        }
    }
    public void SetGameResultTexts(int points1, int points2, int rounds)
    {
        _player1GameResult.text = "Player 1 won:     " + points1 + "/" + rounds+" times";
        _player2GameResult.text = "Player 2 won:     " + points2 + "/" + rounds+" times";
    }

    #endregion

    #region Buttons:

    /// <summary>
    /// Boton para cambiar de estado del GameManager pues no hay botones en otros estados.
    /// </summary>
    /// <param name="newState"></param>
    public void ButtonChangeState(int newState)
    {
        Debug.Log("[UI MANAGER] Boton para cambiar a estado: " + (GameManager.GameStates)newState);
        GameManager.Instance.RequestStateChange((GameManager.GameStates)newState);
    }

    /// <summary>
    /// Boton para cambiar la velocidad de las transiciones y el tiempo que pasa entre acciones, reaprtir cartas yla muestra de resultados.
    /// </summary>
    /// <param name="speed"></param>
    public void ButtonSetSpeed(int speed)
    {
        Debug.Log("[UI MANAGER] Boton para cambiar la velocidad a: " + speed);
        LevelManager.Instance.SetSpeed(speed);
    }

    /// <summary>
    /// Boton para salir de la aplicacion.
    /// </summary>
    public void ButtonExit()
    {
        Application.Quit();
    }

    #endregion
}