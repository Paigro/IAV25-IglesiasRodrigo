using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Carta metida a mesa: " + card.GetCardName());
        }
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

        int nBrooms = acc % 15;
        Debug.Log("//------Hay " + nBrooms + " escobas en mesa inicial.");
        if (nBrooms != 0)
        {
            ClearTable();
        }

        return nBrooms;
    }

    public void WriteTable()
    {
        Debug.Log("//------Cartas de la mesa: ");
        for (int i = 0; i < _cardsInTable.Count; i++)
        {
            Debug.Log(_cardsInTable[i].GetCardName());
        }
    }
}
