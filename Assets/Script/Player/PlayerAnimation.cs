using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator _animator;
    SpriteRenderer _spriteRenderer;



    private string _currentState;
    private string _lastState;
    private IPlayerController _playerController;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerController = GetComponent<IPlayerController>();
    }

    private void Update()
    {
        GetCurrentState();
        if(_currentState == _lastState) { return; }
        _animator.Play(_currentState);
        _lastState = _currentState;
    }
    private void GetCurrentState()
    {
        if (!_playerController.Dead)
        {
            if (!_playerController.Hurting) 
            { 

                if (!_playerController.isWalking)
                {
                    _currentState = PlayerAnimState.PLAYER_IDLE;
                }
                if (_playerController.isWalking)
                {
                    _currentState = PlayerAnimState.PLAYER_RUN;
                }
                if (_playerController.Falling || _playerController.Jumping)
                {
                    _currentState = PlayerAnimState.PLAYER_FALL;
                    if (_playerController.Shooting)
                    {
                        _currentState = PlayerAnimState.PLAYER_JUMPATTACK;
                    }
                    if (_playerController.TouchingWall)
                    {
                        _currentState = PlayerAnimState.PLAYER_WALLSLIDE;
                    }
                }
                if (_playerController.Shooting && !(_playerController.Falling || _playerController.Jumping))
                {
                    _currentState = PlayerAnimState.PLAYER_ATTACK;
                }
                if (_playerController.Dashing)
                {
                    _currentState = PlayerAnimState.PLAYER_DASH;
                }
            }
            else
            {
                _currentState = PlayerAnimState.PLAYER_HURT;
            }
        }
        else
        {
            _currentState = PlayerAnimState.PLAYER_DIE;
        }

    }

    public static class PlayerAnimState
    {
        public const string PLAYER_RUN = "run";
        public const string PLAYER_IDLE = "idle";
        public const string PLAYER_FALL = "jump";
        public const string PLAYER_ATTACK = "attack";
        public const string PLAYER_JUMPATTACK = "jump attack";
        public const string PLAYER_WALLSLIDE = "wallslide";
        public const string PLAYER_DIE = "die";
        public const string PLAYER_DASH = "dash";
        public const string PLAYER_HURT = "hurt";
    }
}
