using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController : MonoBehaviour
{
    [SerializeField] Animator door_Ac;

    public void SetTriggerAnimation()
    {
        door_Ac.SetTrigger("isPlayerInZone");
    }
}
