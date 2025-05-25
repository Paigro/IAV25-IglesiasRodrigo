using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAModel : MonoBehaviour
{
    /// <summary>
    /// Dada la mano y la mesa, busca el mejor movimiento y devuelve una lista con las cartas que usa. 
    /// La primera es la propia de la mano y el resto son cartas de la mesa.
    /// </summary>
    /// <param name="hand">Mano del jugador.</param>
    /// <param name="table">Cartas que hay en la mesa.</param>
    public virtual List<Card> FindMove(List<Card> hand, List<Card> table)
    {
        return new List<Card>();
    }
    //public abstract Card ChooseHandCard(List<Card> hand, List<Card> table);
}
