using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region Awake, Start and Update:

    private void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

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

    #endregion

    #region Turn:

    public void PlayTurn()
    {
        //_model.findMove
        //if move guay then pickCards from table
        //else put card in table
    }

    #endregion
}
