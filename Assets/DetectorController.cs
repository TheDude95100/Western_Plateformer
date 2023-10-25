using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorController : MonoBehaviour
{
    DoorAnimationController doorAnimationController;
    private void Awake()
    {
        doorAnimationController = GetComponentInParent<DoorAnimationController>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("NVJKSNBGRIBNTG");
            doorAnimationController.SetTriggerAnimation();
        }
    }
}
