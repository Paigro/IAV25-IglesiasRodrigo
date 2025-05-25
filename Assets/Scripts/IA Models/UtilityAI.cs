using System.Collections.Generic;
using UnityEngine;

public class UtilityAI : IAModel
{
    [SerializeField] private float BROOM_COST = 2f; // The cost added when making a Scopa (clean the table).
    [SerializeField] private float GOLDEN_SEVEN_COST = 0.7f; // The cost added when the golden seven is in the move.
    [SerializeField] private float SEVEN_COST = 0.4f; // The cost added when sevens are in the move.
    [SerializeField] private float GOLD_COST = 0.2f; // The cost added when golds are in the move.
    [SerializeField] private float CARD_COST = 0.1f; // The cost added for every card in the move.
    [SerializeField] private float CARD_LEAVE_COST = 0.2f; // The cost for leaving a card in the table.

    public override List<Card> FindMove(List<Card> hand, List<Card> table)
    {
        float bestScore = float.MinValue;
        List<Card> bestMove = null;

        for (int i = 0; i < hand.Count; i++)
        {
            int target = 15 - hand[i].GetCardNumber();
            List<List<Card>> allCombinatios = GetAllCombinations(table, target); // Cogemos todas las combinaciones con todas las cartas de la mano.

            // Si no hay combinaciones que consigan 15 entonces ponemos la carta.
            if (allCombinatios.Count == 0)
            {
                float score = EvaluateLeaveCard(hand[i], table); // Buscamos el escore que deje los menos ventajoso para el oponente.
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = new List<Card>() { hand[i] };
                }
            }
            else // Si hay combinaciones de 15 entonces buscamos la mejor.
            {
                for (int j = 0; j < allCombinatios.Count; j++)
                {
                    float score = EvaluatePickedCards(hand[i], allCombinatios[j], table); // Pedimos el escore de esa combinacion.
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new List<Card>() { hand[i] };
                        bestMove.AddRange(allCombinatios[j]);
                    }
                }
            }
        }

        return bestMove;
    }
    /// <summary>
    /// Evalua la carta dada con lsa cartas de la mesa.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="table"></param>
    /// <returns>El valor de dejar esa carta en la mesa.</returns>
    private float EvaluateLeaveCard(Card card, List<Card> table)
    {
        float score = 0;

        score -= card.GetCardNumber() * CARD_LEAVE_COST;

        // Dejar un siete.
        if (card.GetCardNumber() == 7)
        {
            score -= SEVEN_COST;
            // Dejar el siete de oros.
            if (card.GetCardSuit() == 'O')
            {
                score -= GOLDEN_SEVEN_COST;
            }
        }
        // Dejar un oro
        if (card.GetCardSuit() == 'O')
        {
            score -= GOLD_COST;
        }
        // Dejar escoba.
        int acc = 0;
        for (int i = 0; i < table.Count; i++)
        {
            acc += table[i].GetCardNumber();
        }
        if (acc + card.GetCardNumber() == 15)
        {
            score -= BROOM_COST;
        }

        return score;
    }
    /// <summary>
    /// Evalua el valor de hacer una dada combinacion de cartas con las cartas de la mesa.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="picked"></param>
    /// <param name="table"></param>
    /// <returns>El valor de dicha combiancion.</returns>
    private float EvaluatePickedCards(Card card, List<Card> picked, List<Card> table)
    {
        float score = 0;

        score += picked.Count * CARD_COST; ;

        for (int i = 0; i < picked.Count; i++)
        {
            // Coger un oro.
            if (picked[i].GetCardSuit() == 'O')
            {
                score += GOLD_COST;
            }
            // Coger un siete.
            if (picked[i].GetCardNumber() == 7)
            {
                score += SEVEN_COST;
                // Coger el siete de oros.
                if (picked[i].GetCardSuit() == 'O')
                {
                    score += GOLDEN_SEVEN_COST;
                }
            }
        }
        // Hacer escoba.
        int acc = 0;
        for (int i = 0; i < table.Count; i++)
        {
            acc += table[i].GetCardNumber();
        }
        if (card.GetCardNumber() + acc == 15)
        {
            score += BROOM_COST;
        }

        return score;
    }

    /// <summary>
    /// Crea todas las combinaciones posibles dada la mesa y el valor que se quiere conseguir (15 - cartaSeleccionada).
    /// </summary>
    /// <param name="table"></param>
    /// <param name="target"></param>
    /// <returns>LAs combinaciones de cartas en un lista de listas.</returns>
    private List<List<Card>> GetAllCombinations(List<Card> table, int target)
    {
        List<List<Card>> result = new();
        void Backtrack(int start, List<Card> current, int currentSum)
        {
            if (currentSum == target)
            {
                result.Add(new List<Card>(current));
                return;
            }
            if (currentSum > target) return;

            for (int i = start; i < table.Count; i++)
            {
                current.Add(table[i]);
                Backtrack(i + 1, current, currentSum + table[i].GetCardNumber());
                current.RemoveAt(current.Count - 1);
            }
        }
        Backtrack(0, new List<Card>(), 0);

        return result;
    }
}