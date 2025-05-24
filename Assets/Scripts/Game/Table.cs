using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
            Debug.Log(" [MESA] Carta metida a mesa: " + card.GetCardName());
        }
    }
    public List<Card> GetCardsInTable()
    {
        return _cardsInTable;
    }
    public void ClearTable()
    {
        _cardsInTable.Clear();
    }

    public int CheckInitBrooms()
    {
        int acc = 0;

        for (int i = 0; i < _cardsInTable.Count; i++)
        {
            acc += _cardsInTable[i].GetCardNumber();
        }

        int nBrooms = 0;
        if (acc % 15 == 0)
        {
            nBrooms = acc / 15;
        }
        Debug.Log("[MESA] Hay " + nBrooms + " escobas en mesa inicial.");
        if (nBrooms != 0)
        {
            ClearTable();
        }

        return nBrooms;
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
