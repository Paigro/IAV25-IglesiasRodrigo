using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCardsManager : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab; // Prefab de carta.
    [SerializeField] private float _cardsMovementSpeed = 1.0f; // Velocidad de las cartas al moverse.

    private Transform _deckTrans; // Transform del mazo.
    private Transform _tableTrans; // Transform de la mesa.
    private Transform _player1Trans; // Transform del jugador 1.
    private Transform _player2Trans; // Transform del jugador 2.
    private Transform _stack1Trans; // Transform de la pila del jugador 1. Aunque seguramente este fuera de la pantalla.
    private Transform _stack2Trans; // Transform de la pila del jugador 2. Aunque seguramente este fuera de la pantalla.

    private Dictionary<string, Sprite> _spriteDict; // Diccionario que asocia el nombre de la carta con su sprite.

    List<GameObject> _cardsGaOb;

    void Start()
    {
        LevelManager.Instance.RegisterVisualCardsManager(this);

        _spriteDict = new Dictionary<string, Sprite>();
        _cardsGaOb = new List<GameObject>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        foreach (Sprite sprite in sprites)
        {
            _spriteDict[sprite.name] = sprite;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="where"></param>
    public void SpawnAllDeck(Transform where)
    {
        string[] suits = { "O", "C", "E", "B" };

        foreach (string suit in suits)
        {
            for (int i = 1; i <= 10; i++)
            {
                string cardName = suit + i;

                GameObject card = Instantiate(_cardPrefab, where);
                card.name = cardName;

                if (_spriteDict.TryGetValue(cardName, out Sprite sprite))
                    card.GetComponent<VisualCard>().SetSprite(sprite);
                else
                    Debug.Log("[VISUAL CARD MANAGER] Sprite no encontrado: " + cardName);

                _cardsGaOb.Add(card);
            }
        }
    }

    /// <summary>
    /// Moves a card to.
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="whereToGo"></param>
    public void MoveCardTo(string cardName, Transform whereToGo, Vector2 offsetFromWhere = new Vector2())
    {
        foreach (GameObject card in _cardsGaOb)
        {
            if (card.name == cardName)
            {
                StartCoroutine(AnimateMove(card.transform, whereToGo, offsetFromWhere));
                return;
            }
        }

        Debug.LogWarning("[VISUAL CARD MANAGER] No se encontro la carta con nombre: " + cardName);

    }
    private IEnumerator AnimateMove(Transform card, Transform destination, Vector2 offset)
    {
        Vector3 startPos = card.position;
        Vector3 endPos = destination.position + (Vector3)offset;  // Suma el offset aquí (convertido a Vector3)
        Quaternion startRot = card.rotation;
        Quaternion endRot = destination.rotation;

        float duration = _cardsMovementSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            card.position = Vector3.Lerp(startPos, endPos, t);
            card.rotation = Quaternion.Lerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.position = endPos;
        card.rotation = endRot;
        //card.SetParent(destination);
    }
}