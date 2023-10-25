using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerManager : MonoBehaviour
{
    PlayerController _playerController;
    HookPoint lastHookPoint;
    public int Score;
    internal bool Dead { get; private set; }

    public static PlayerManager instance;

    [SerializeField] MenuMort menuMort;

    
    private void Awake()
    {
        Score = 0;
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
    public float getPlayerLocalScaleSign()
    {
       return Mathf.Sign(_playerController.transform.localScale.x );
    }

    public void EnableGameOverMenu()
    {
        MenuManager.Instance.MenuMort.gameObject.SetActive(true);
    }
}
