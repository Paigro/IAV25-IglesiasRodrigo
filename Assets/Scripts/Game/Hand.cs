using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hand.
/// </summary>
public class Hand : MonoBehaviour
{
    #region Propierties:

    /// <summary>
    /// Lista de cartas de la mano.
    /// </summary>
    private List<Card> _cardsInHand;
    /// <summary>
    /// Lsita de cartas de la pila.
    /// </summary>
    private List<Card> _cardsInStack;

    /// <summary>
    /// Acumulador de escobas de quien tenga esta mano. 
    /// </summary>
    private int _nBrooms;

    #endregion

    #region Awake:

    private void Awake()
    {
        _cardsInHand = new List<Card>();
        _cardsInStack = new List<Card>();
        _nBrooms = 0;
    }

    #endregion

    #region Common methods:

    /// <summary>
    /// Resetea tanto la mano, como la pila, como el numero de escobas. Vaciando las listas y poniendo a 0.
    /// </summary>
    public void ResetHand()
    {
        ClearHand();
        ClearStack();
        _nBrooms = 0;
    }

    #endregion

    #region Hand of the player:

    /// <summary>
    /// Mete una carta a la mano.
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToHand(Card card)
    {
        if (card != null)
        {
            _cardsInHand.Add(card);
            //Debug.Log(" [MANO] Carta metida a mano: " + card.GetCardName());
        }
    }

    /// <summary>
    /// Juega una carta de la mano (la elimina). 
    /// </summary>
    /// <param name="card"></param>
    public void PlayCard(Card card)
    {
        _cardsInHand.Remove(card);
    }

    /// <summary>
    /// Saber el numero de cartas de la mano.
    /// </summary>
    /// <returns>El numero de carta de la mano.</returns>
    public int GetHandCount()
    {
        return _cardsInHand.Count;
    }

    /// <summary>
    /// Devuelve las cartas que hay en la mano.
    /// </summary>
    /// <returns>Una copia de la lista de cartas de la mano por proteccion.</returns>
    public List<Card> GetCardsInHand()
    {
        return new List<Card>(_cardsInHand);
    }

    /// <summary>
    /// Limpia la mano vaciando la lista.
    /// </summary>
    private void ClearHand()
    {
        _cardsInHand.Clear();
    }

    /// <summary>
    /// Escribe en el Debug de Unity la mano.
    /// </summary>
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

    /// <summary>
    /// Mete una carta a la pila.
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToStack(Card card)
    {
        if (card != null)
        {
            _cardsInStack.Add(card);
            //Debug.Log(" [MANO] [PILA] Carta metida a pila: " + card.GetCardName());
        }
    }

    /// <summary>
    /// Devuelve las cartas que hay en la pila.
    /// </summary>
    /// <returns>Una copia de la lista de cartas de la pila.</returns>
    public List<Card> GetCardsInStack()
    {
        return new List<Card>(_cardsInStack);
    }

    /// <summary>
    /// Saber el numero de cartas de la pila.
    /// </summary>
    /// <returns>El numero de cartas de la pila.</returns>
    public int GetStackCount()
    {
        return _cardsInStack.Count;
    }

    /// <summary>
    /// Limpia la pila vaciando la lista.
    /// </summary>
    private void ClearStack()
    {
        _cardsInStack.Clear();
    }

    /// <summary>
    /// Escribe en el Debug de Unity la pila.
    /// </summary>
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

    /// <summary>
    /// Suma una escoba al contador.
    /// </summary>
    /// <param name="nBrooms"></param>
    public void AddBroom(int nBrooms = 1)
    {
        //Debug.Log("[MANO] [ESCOBAS] Escoba sumada.");
        _nBrooms += nBrooms;
    }

    /// <summary>
    /// Saber el numero de escobas de la mano.
    /// </summary>
    /// <returns>El numero de escobas de la mano.</returns>
    public int GetBrooms()
    {
        return _nBrooms;
    }

    #endregion
}
