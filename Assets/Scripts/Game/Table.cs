using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Table : MonoBehaviour
{
    private List<Card> _cardsInTable;

    private void Awake()
    {
        _cardsInTable = new List<Card>();
    }

    public void AddCardToTable(Card card)
    {
        if (card != null)
        {
            _cardsInTable.Add(card);
            //Debug.Log(" [MESA] Carta metida a mesa: " + card.GetCardName());
        }
    }
    public void RemoveCardToTable(Card card)
    {
        _cardsInTable.Remove(card);
    }
    public List<Card> GetCardsInTable()
    {
        return _cardsInTable;
    }
    public void ClearTable()
    {
        _cardsInTable.Clear();
    }
    /// <summary>
    /// Comprueba si hay escobas en la mesa inicial.
    /// </summary>
    /// <returns>El numero de escobas en la mesa</returns>
    public int CheckInitBrooms()
    {
        int sum = GetTableSum();

        int nBrooms = 0;
        if (sum % 15 == 0)
        {
            nBrooms = sum / 15;
        }
        //Debug.Log("[MESA] Hay " + nBrooms + " escobas en mesa inicial.");
        return nBrooms;
    }
    /// <summary>
    /// Calcula el total de puntos de la mesa.
    /// </summary>
    /// <returns>Los puntos totales de la mesa.</returns>
    public int GetTableSum()
    {
        int acc = 0;
        for (int i = 0; i < _cardsInTable.Count; i++)
        {
            acc += _cardsInTable[i].GetCardNumber();
        }
        //Debug.Log("[MESA] Puntos en mesa: " + acc);
        return acc;
    }

    public void WriteTable()
    {
        Debug.Log("[MESA] Cartas de la mesa: ");
        for (int i = 0; i < _cardsInTable.Count; i++)
        {
            Debug.Log(_cardsInTable[i].GetCardName());
        }
    }
}