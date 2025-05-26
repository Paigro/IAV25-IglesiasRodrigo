using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    #endregion

    #region Buttons:

    public void ButtonChangeState(int newState)
    {
        GameManager.Instance.RequestStateChange((GameManager.GameStates)newState);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    #endregion
}