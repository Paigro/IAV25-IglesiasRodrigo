using System.Collections.Generic;

public abstract class IAModel
{
    /// <summary>
    /// Dada la mano y la mesa, busca el mejor movimiento y devuelve una lista con las cartas que usa. 
    /// La primera es la propia de la mano y el resto son cartas de la mesa.
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="table"></param>
    /// <returns>Una lista de cartas a usar en la jugada.</returns>
    public virtual List<Card> FindMove(List<Card> hand, List<Card> table)
    {
        return new List<Card>();
    }
}