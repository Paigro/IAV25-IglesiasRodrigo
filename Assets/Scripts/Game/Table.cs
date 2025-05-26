using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Table : MonoBehaviour
{
    #region Porperties:

    /// <summary>
    /// Lista de cartas de la mesa.
    /// </summary>
    private List<Card> _cardsInTable;

    #endregion

    #region Awake

    private void Awake()
    {
        _cardsInTable = new List<Card>();
    }

    #endregion

    #region Card movements:

    /// <summary>
    /// Mete una carta a la mesa.
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToTable(Card card)
    {
        if (card != null)
        {
            _cardsInTable.Add(card);
            //Debug.Log(" [MESA] Carta metida a mesa: " + card.GetCardName());
        }
    }

    /// <summary>
    /// Saca una carta de la mesa.
    /// </summary>
    /// <param name="card"></param>
    public void RemoveCardToTable(Card card)
    {
        _cardsInTable.Remove(card);
        //Debug.Log(" [MESA] Carta sacada de la mesa: " + card.GetCardName());
    }

    #endregion

    #region Table methods:

    /// <summary>
    /// Devuelve las cartas que hay en la mesa.
    /// </summary>
    /// <returns>Una copia de la lista de las cartas de la mesa para proteger la propia.</returns>
    public List<Card> GetCardsInTable()
    {
        return new List<Card>(_cardsInTable);
    }

    /// <summary>
    /// Limpia la mesa eliminando la lista de cartas.
    /// </summary>
    public void ClearTable()
    {
        _cardsInTable.Clear();
    }

    /// <summary>
    /// Comprueba si hay escobas en la mesa inicial.
    /// </summary>
    /// <returns>El numero de escobas en la mesa.</returns>
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

    #endregion

    #region Write:

    /// <summary>
    /// Escribe la mesa en el Debug de Unity.
    /// </summary>
    public void WriteTable()
    {
        Debug.Log("[MESA] Cartas de la mesa: ");
        for (int i = 0; i < _cardsInTable.Count; i++)
        {
            Debug.Log(_cardsInTable[i].GetCardName());
        }
    }

    #endregion
}