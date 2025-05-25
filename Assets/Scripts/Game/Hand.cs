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

    #region Awake, Start and Update:

    private void Awake()
    {
        _cardsInHand = new List<Card>();
        _cardsInStack = new List<Card>();
        _nBrooms = 0;
    }

    #endregion

    #region Common methods:

    public void ResetHand()
    {
        ClearHand();
        ClearStack();
        _nBrooms = 0;
    }

    #endregion

    #region Hand of the player:

    public void AddCardToHand(Card card)
    {
        if (card != null)
        {
            _cardsInHand.Add(card);
            //Debug.Log(" [MANO] Carta metida a mano: " + card.GetCardName());
        }
    }
    public void PlayCard(Card card)
    {
        _cardsInHand.Remove(card);
    }
    public int GetHandCount()
    {
        return _cardsInHand.Count;
    }
    public List<Card> GetCardsInHand()
    {
        return _cardsInHand;
    }
    public void ClearHand()
    {
        _cardsInHand.Clear();
    }
    public void WriteHand()
    {
        Debug.Log("[MANO] Cartas de la mano: ");
        for (int i = 0; i < _cardsInHand.Count; i++)
        {
            Debug.Log(_cardsInHand[i].GetCardName());
        }
    }

    #endregion

    #region Stack of players cards:

    public void AddCardToStack(Card card)
    {
        if (card != null)
        {
            _cardsInStack.Add(card);
            //Debug.Log(" [MANO] [PILA] Carta metida a pila: " + card.GetCardName());
        }
    }
    public List<Card> GetCardsInStack()
    {
        return _cardsInStack;
    }
    public int GetStackCount()
    {
        return _cardsInStack.Count;
    }
    public void ClearStack()
    {
        _cardsInStack.Clear();
    }

    public void WriteStack()
    {
        Debug.Log("[MANO] [PILA] Cartas de la pila: ");
        for (int i = 0; i < _cardsInStack.Count; i++)
        {
            Debug.Log(_cardsInStack[i].GetCardName());
        }
    }

    #endregion

    #region Brooms:

    public void AddBroom(int nBrooms = 1)
    {
        //Debug.Log("[MANO] [ESCOBAS] Escoba sumada.");
        _nBrooms += nBrooms;
    }
    public int GetBrooms()
    {
        return _nBrooms;
    }

    #endregion
}
