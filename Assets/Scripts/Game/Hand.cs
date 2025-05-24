using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hand.
/// </summary>
public class Hand : MonoBehaviour
{
    private List<Card> _cardsInHand;
    private List<Card> _cardsInStack;
    private int _nBrooms;

    private void Awake()
    {
        _cardsInHand = new List<Card>();
        _cardsInStack = new List<Card>();
        _nBrooms = 0;
    }

    #region Hand of the player:

    public void AddCardToHand(Card card)
    {
        if (card != null)
        {
            _cardsInHand.Add(card);
            Debug.Log("Carta metida a mano: " + card.GetCardName());
        }
    }
    public void ClearHand()
    {
        _cardsInHand.Clear();
    }

    public void WriteHand()
    {
        Debug.Log("//------Cartas de la mano: ");
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            Debug.Log(_cardsInHand[i].GetCardName());
        }
    }

    #endregion

    #region Stack of player cards:

    public void AddCardToStack(Card card)
    {
        if (card != null)
        {
            _cardsInStack.Add(card);
            Debug.Log("Carta metida a pila: " + card.GetCardName());
        }
    }
    public List<Card> GetCardsInStack()
    {
        return _cardsInStack;
    }

    public void AddBroom(int nBrooms = 1)
    {
        _nBrooms++;
    }
    public int GetBrooms()
    {
        return _nBrooms;
    }

    public void ClearStack()
    {
        _cardsInStack.Clear();
    }

    public void WriteStack()
    {
        Debug.Log("//------Cartas de la pila: ");
        for (int i = 0; i < _cardsInStack.Count; i++)
        {
            Debug.Log(_cardsInStack[i].GetCardName());
        }
    }

    #endregion
}
