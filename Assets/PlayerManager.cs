using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController _playerController;
    PlayerInputSystem _inputSystem;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _inputSystem = GetComponent<PlayerInputSystem>();
    }
    public void ChangePlayerEnabledStatus()
    {
        _playerController.enabled = !_playerController.enabled;
        _inputSystem.enabled = !_inputSystem.enabled; 
    }
}
