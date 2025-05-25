using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deck.
/// </summary>
public class Deck : MonoBehaviour
{
    private List<Card> _cardsInDeck;

    void Awake()
    {
        _cardsInDeck = new List<Card>();
    }

    public void CreateDeck()
    {
        Debug.Log("[MAZO] Creando mazo.");
        _cardsInDeck.Clear();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                switch (i)
                {
                    case 0:
                        _cardsInDeck.Add(new Card('O', j));
                        break;
                    case 1:
                        _cardsInDeck.Add(new Card('E', j));
                        break;
                    case 2:
                        _cardsInDeck.Add(new Card('C', j));
                        break;
                    case 3:
                        _cardsInDeck.Add(new Card('B', j));
                        break;
                }
            }
        }
    }

    public void Shuffle(int nTimes = 1)
    {
        Debug.Log("[MAZO] Barajando mazo " + nTimes + " veces.");
        int n = _cardsInDeck.Count;
        for (int i = 0; i < nTimes; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int k = Random.Range(0, j + 1);
                Card aux = _cardsInDeck[j];
                _cardsInDeck[j] = _cardsInDeck[k];
                _cardsInDeck[k] = aux;
            }
        }
    }

    public void ResetDeck()
    {
        Debug.Log("[MAZO] Reseteando mazo.");
        CreateDeck();
        Shuffle(10);
    }

    public Card DrawCard()
    {
        if (_cardsInDeck.Count == 0)
        {
            Debug.LogWarning("[MAZO] No hay cartas en el mazo.");
            return null;
        }
        Card drawCard = _cardsInDeck[_cardsInDeck.Count - 1];
        _cardsInDeck.RemoveAt(_cardsInDeck.Count - 1);
        return drawCard;
    }
    public int GetDeckCount()
    {
        return _cardsInDeck.Count;
    }

    public void WriteDeck()
    {
        Debug.Log("[MAZO] Escribiendo mazo con " + _cardsInDeck.Count + " cartas.");
        for (int i = 0; i < _cardsInDeck.Count; i++)
        {
            Debug.Log("Carta: " + _cardsInDeck[i].GetCardName());
        }
    }

    public void ReadDeckFromTxt()
    {
        Debug.Log("[MAZO] Leyendo mazo.");

        // Limpiamos la lista por si acaso.
        _cardsInDeck.Clear();

        // Cogemos el archivo de resources.
        TextAsset deckFile = Resources.Load<TextAsset>("deck");

        if (deckFile == null)
        {
            Debug.LogError("[MAZO] No se ha encontrado el archivo");
            return;
        }

        string[] lines = deckFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            string[] parts = trimmedLine.Split(' ');

            if (parts.Length == 2)
            {
                char suit = parts[0][0];
                if (int.TryParse(parts[1], out int number))
                {
                    _cardsInDeck.Add(new Card(suit, number));
                    Debug.Log("[MAZO] Carta metida desde txt: " + suit + number);
                }
                else
                {
                    Debug.LogWarning("[MAZO] Numero no valido en linea: " + line);
                }
            }
            else
            {
                Debug.LogWarning("[MAZO] Linea mal hecha: " + line);
            }
        }
    }
}