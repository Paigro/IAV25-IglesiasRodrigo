using System.Collections.Generic;
using System.Data;
using UnityEngine;


/// <summary>
/// Class that contains the player with its hand and its AI model.
/// </summary>
public class Player : MonoBehaviour
{
    #region Parameters:

    /// <summary>
    /// Para saber si esta siendo controlado por el usuario o por una IA.
    /// </summary>
    private bool _isHuman;
    /// <summary>
    /// Referencia a la mano del jugador.
    /// </summary>
    private Hand _hand;
    /// <summary>
    /// El modelo de IA que esta usando el jugador.
    /// </summary>
    private IAModel _model;

    #endregion

    #region Setters:

    public void SetPlayable(bool playable)
    {
        _isHuman = playable;
    }
    public void SetHand(Hand hand)
    {
        _hand = hand;
    }
    public void SetAIModel(IAModel model)
    {
        _model = model;
    }

    #endregion

    #region Getters:

    public bool GetPlayerIsHuman()
    {
        return _isHuman;
    }
    public Hand GetPlayerHand()
    {
        return _hand;
    }
    public IAModel GetAIModel()
    {
        return _model;
    }

    #endregion

    #region Cards management:

    public void ReceiveCard(Card card)
    {
        _hand.AddCardToHand(card);
    }

    public void ResetHand()
    {
        _hand.ResetHand();
    }

    #endregion

    #region Turn:

    public void PlayTurn(List<Card> table)
    {
        List<Card> move;

        move = _model.FindMove(_hand.GetCardsInHand(), table);

        if (move.Count > 1)
        {
            //Debug.Log("[PLAYER] Jugada que coje cartas.");
            for (int i = 0; i < move.Count; i++)
            {
                //Debug.Log(move[i].GetCardName());
            }
        }
        else if (move.Count == 1)
        {
            //Debug.Log("[PLAYER] Jugada que deja carta en mesa.");
            for (int i = 0; i < move.Count; i++)
            {
                //Debug.Log(move[i].GetCardName());
            }
        }
        else
        {
            Debug.LogError("[PLAYER] ERROR al calcular movimiento.");
        }

        if (_model != null)
        {
            //Debug.Log("[PLAYER] Notificacion a LevelManager");
            LevelManager.Instance.NotifiyPlayerEndTurn(move);
        }
        else
        {
            Debug.LogError("[PLAYER] ERROR no hay modelo de IA.");
        }
    }

    #endregion
}