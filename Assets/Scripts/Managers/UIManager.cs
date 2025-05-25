using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    

    #region Awake, Start and Update:

    private void Awake()
    {
        
    }
    void Start()
    {
        GameManager.Instance.RegisterUIManager(this);
    }

    void Update()
    {

    }

    #endregion



}
