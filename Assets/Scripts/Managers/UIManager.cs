using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{





    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterUIManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
