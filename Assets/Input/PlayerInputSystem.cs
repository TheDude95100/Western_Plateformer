using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerController.Jump();
        }
    }
    
    public void WalkRight(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            playerController.SetWalkingRight(true);
        }
        else if (context.canceled)
        {
            playerController.SetWalkingRight(false);
        }
    }
    public void WalkLeft(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            playerController.SetWalkingLeft(true);

        }
        else if (context.canceled)
        {
            playerController.SetWalkingLeft(false);
        }
    }
    
    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //playerController.TakeDamage();
        }
    }

    public void PauseBoutton(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            MenuManager.Instance.MenuPause.gameObject.SetActive(!MenuManager.Instance.MenuPause.gameObject.activeSelf);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            StartCoroutine(playerController.Dash());
        }
    }
}
