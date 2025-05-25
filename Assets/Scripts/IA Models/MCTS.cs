using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MCTS : IAModel
{
    public override List<Card> FindMove(List<Card> hand, List<Card> table)
    {
        Debug.Log("[MCTS] Inicio de FindMove");

        MCTSState initState = new MCTSState(hand, table); // Creamos el estado inicial.

        MCTSState bestState = FindMove(initState); // Buscamos le mejor jugada.

        Debug.Log("[MCTS] Fin de FindMove. Jugada elegida: " + string.Join(", ", bestState.pickedCards.Select(c => c.ToString())));

        return bestState.pickedCards; // Devolvemos las cartas usadas,
    }

    private MCTSState FindMove(MCTSState initState)
    {
        MCTSNode initNode = new MCTSNode(initState, null); // Creamos el nodo inicial.

        for (int i = 0; i < 500; i++) // Recorremos todas las posibilidades. 500 veces puede ser un buen calculo.
        {
            MCTSNode selectedNode = SelectNode(initNode); // Fase de seleccion de nodo.
            MCTSNode expandedNode = ExpandNode(selectedNode); // Fase de expansion del nodo seleccionado.
            float score = Simulate(expandedNode); // Fase de simular. Nos quedamos con la puntuacion.
            Backpropagate(expandedNode, score); // Fase de propagar hacia atras.

            //Debug.Log("[MCTS] Iteracion " + i + ": Score=" + score);
        }

        var best = GetBestChild(initNode).currentState;
        //Debug.Log("[MCTS] Mejor jugada tras iterar: " + string.Join(", ", best.pickedCards.Select(c => c.ToString())));
        return best;
    }

    // Fase del MCTS de seleccionar un nodo dado uno inicial.
    private MCTSNode SelectNode(MCTSNode initNode)
    {
        while (initNode.children.Count > 0)
        {
            initNode = initNode.children.OrderByDescending(child =>
            {
                float ucb1 = (child.wins / (child.visits + 1e-6f)) + (Mathf.Sqrt(2 * Mathf.Log(initNode.visits + 1) / (child.visits + 1e-6f)));
                return ucb1;
            }).First();
        }

        //Debug.Log("[MCTS] Nodo seleccionado sin hijos: " + initNode);
        return initNode;
    }

    // Fase del MCTS de expandir dado un nodo. Le metemos sus posibles hijos.
    private MCTSNode ExpandNode(MCTSNode nodeToExpand)
    {
        List<MCTSState> moves = nodeToExpand.currentState.GetLegalMoves();

        //Debug.Log("[MCTS] Expandiendo nodo con: " + moves.Count + " movimientos ");

        // Metemos al nodo los posibles hijos.
        for (int i = 0; i < moves.Count; i++)
        {
            nodeToExpand.children.Add(new MCTSNode(moves[i], nodeToExpand));
        }

        // Devolvemos o un hijo aleatorio del nodo que hemos expandido o el propio nodo si no se han generado hijos.
        MCTSNode finalNode = nodeToExpand.children.Count > 0 ? nodeToExpand.children[Random.Range(0, nodeToExpand.children.Count)] : nodeToExpand;

        //Debug.Log("[MCTS] Nodo expandido: " + finalNode);
        return finalNode;
    }

    // Fase del MCTS de simular dado un nodo. Cogemos la puntuacion que tenga y la devolvemos.
    private float Simulate(MCTSNode simulatedNode)
    {
        float score = simulatedNode.currentState.GetScore();
        //Debug.Log("[MCTS] Simulacion devuelve score: " + score);
        return score;
    }

    // Fase del MCTS de propagar hacia atras el nodo.
    private void Backpropagate(MCTSNode node, float score)
    {
        while (node != null)
        {
            node.visits++;
            node.wins += score;
            node = node.parentNode;
        }
        //Debug.Log("[MCTS] Backpropagation completada con score: " + score);
    }

    // Ordena la lista de hijos del nodo por win rate y devuelve el primero.
    private MCTSNode GetBestChild(MCTSNode initNode)
    {
        var best = initNode.children.OrderByDescending(child => child.wins / child.visits).FirstOrDefault();
        //Debug.Log("[MCTS] Mejor hijo con winrate: " + (best?.wins / best?.visits));
        return best;
    }

    //------------Clases necesarias para el MCTS------------//
    private class MCTSState
    {
        public List<Card> playerHand; // Cartas de la mano del jugador.
        public List<Card> tableCards; // Cartas que hay en la mesa.
        public List<Card> pickedCards = new List<Card>(); // Cartas de la mesa seleccionadas.
        public Card selectedCard; // Carta de la mano seleccionada.

        // Constructoras.
        public MCTSState() { }
        public MCTSState(List<Card> hand, List<Card> table)
        {
            playerHand = new List<Card>(hand);
            tableCards = new List<Card>(table);
        }

        public List<MCTSState> GetLegalMoves()
        {
            List<MCTSState> result = new List<MCTSState>();

            for (int i = 0; i < playerHand.Count; i++)
            {
                int target = 15 - playerHand[i].GetCardNumber();
                List<List<Card>> allCombinatiosPossible = GetCombinations(target);

                // Si no hay conbinaciones entonces no quitamos cartas de la mesa y simplemente metemos el estado con la carta quitada a la mano.
                if (allCombinatiosPossible.Count == 0)
                {
                    MCTSState nextState = CloneState();
                    nextState.selectedCard = playerHand[i];
                    nextState.playerHand.Remove(playerHand[i]);
                    nextState.tableCards.Add(playerHand[i]);
                    nextState.pickedCards = new List<Card>() { playerHand[i] };
                    result.Add(nextState);
                }
                else
                {
                    for (int j = 0; j < allCombinatiosPossible.Count; j++)
                    {
                        MCTSState nextState = CloneState(); // Clonamos el estado para no modificar cosas que no.
                        nextState.selectedCard = playerHand[i]; // Cambiamos la carta seleccionada.
                        nextState.playerHand.Remove(playerHand[i]); // Quitamos de la mano del nuevo estado la carta seleccionada para que no la tenga en cuenta.
                        for (int k = 0; k < allCombinatiosPossible[j].Count; k++) // Quitamos de la mesa las cartas ya cogidas.
                        {
                            nextState.tableCards.Remove(allCombinatiosPossible[j][k]);
                        }
                        nextState.pickedCards = new List<Card>() { playerHand[i] }; // Metemos la carta seleccionada,
                        nextState.pickedCards.AddRange(allCombinatiosPossible[j]); // y el resto de cartas de la conbinacion.
                        result.Add(nextState); // Y metemos el nuevo estado en el resultado final.
                    }
                }
            }

            //Debug.Log("[MCTS] Generadas : " + result.Count + " jugadas.");
            return result;
        }

        // Asignamos puntos dependiendo de las cartas que haya cogido.
        public float GetScore()
        {
            float score = pickedCards.Count;
            for (int i = 0; i < pickedCards.Count; i++)
            {
                // Oros.
                if (pickedCards[i].GetCardSuit() == 'O')
                {
                    score += 5;
                }
                // Sietes.
                if (pickedCards[i].GetCardNumber() == 7)
                {
                    score += 10;
                    // Siete de oros.
                    if (pickedCards[i].GetCardSuit() == 'O')
                    {
                        score += 50;
                    }
                }
            }
            // Escoba.
            if (tableCards.Count == 0 && pickedCards.Count > 0)
            {
                score += 100;
            }

            // Dejar carta en mesa es malo, pero tiene que modificar al score sino peta.
            if (pickedCards.Count == 0)
            {
                score += 1f;
            }

            //Debug.Log("[MCTS] Score calculado: " + score + " para pickedCards: " + string.Join(", ", pickedCards.Select(c => c.GetCardName())));
            return score;
        }

        // Devuelve un estado copia del original.
        public MCTSState CloneState()
        {
            return new MCTSState
            {
                playerHand = new List<Card>(this.playerHand),
                tableCards = new List<Card>(this.tableCards),
                pickedCards = new List<Card>(this.pickedCards),
                selectedCard = this.selectedCard
            };
        }

        // Da todas las combinaciones posibles dado un target objetivo (15 - la carta seleccionada).
        private List<List<Card>> GetCombinations(int target)
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

                for (int i = start; i < tableCards.Count; i++)
                {
                    current.Add(tableCards[i]);
                    Backtrack(i + 1, current, currentSum + tableCards[i].GetCardNumber());
                    current.RemoveAt(current.Count - 1);
                }
            }

            Backtrack(0, new List<Card>(), 0);
            return result;
        }
    }

    private class MCTSNode
    {
        public MCTSState currentState;
        public MCTSNode parentNode;
        public List<MCTSNode> children = new List<MCTSNode>();
        public float visits = 0;
        public float wins = 0;

        // Constructora.
        public MCTSNode(MCTSState state, MCTSNode parent)
        {
            currentState = state;
            parentNode = parent;
        }
    }
}
