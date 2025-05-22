using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> _cardList;

    public Deck()
    {
        _cardList = new List<Card>();
        createDeck();
        //shuffle(2);
    }

    private void createDeck()
    {
        _cardList.Clear();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                switch (i)
                {
                    case 0:
                        _cardList.Add(new Card('O', j));
                        break;
                    case 1:
                        _cardList.Add(new Card('E', j));
                        break;
                    case 2:
                        _cardList.Add(new Card('C', j));
                        break;
                    case 3:
                        _cardList.Add(new Card('B', j));
                        break;
                }
            }
        }
    }

    public void shuffle(int nTimes = 1)
    {
        int n = _cardList.Count;
        for (int i = 0; i < nTimes; i++)
        {
            for (int j = 0; j < n; j++)
            {
                int k = Random.Range(0, j);
                Card aux = _cardList[j];
                _cardList[j] = _cardList[k];
                _cardList[k] = aux;
            }
        }
    }

    public void resetDeck()
    {
        createDeck();
        shuffle(10);
    }

    public void writeDeck()
    {
        Debug.Log("Numero de cartas: "+_cardList.Count);
        for (int i = 0; i < _cardList.Count; i++)
        {
            Debug.Log("Carta: " + _cardList[i].getSuit() + _cardList[i].getNumber());
        }
    }
}