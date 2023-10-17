using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController _playerController;
    HookPoint lastHookPoint;

    public static PlayerManager instance;

    
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        instance = this;
       
    }
    public void ChangePlayerEnabledStatus()
    {
        _playerController.playerInput.enabled = !_playerController.playerInput.enabled;
        _playerController.enabled = !_playerController.enabled;
    }

    public void setHookPoint(HookPoint hookPoint)
    {
        lastHookPoint = hookPoint;
    }

    public HookPoint getHookPoint()
    {
        return lastHookPoint;
    }
}
