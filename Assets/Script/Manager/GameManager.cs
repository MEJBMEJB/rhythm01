using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _GameMgrinstance;
    public static GameManager Instance
    {
        get => _GameMgrinstance;
    }

    private bool _isStartGame = false;
    public bool IsStartGame
    {
        get => _isStartGame; 
        set => _isStartGame = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_GameMgrinstance == null)
        {
            _GameMgrinstance = this;
        }
        
    }

}
