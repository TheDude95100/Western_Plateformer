using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerController _playerController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }
    public void ChangePlayerEnabledStatus()
    {
        _playerController.playerInput.enabled = !_playerController.playerInput.enabled;
        _playerController.enabled = !_playerController.enabled;
    }
}
