using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject Bandito;

    public void InstantiateBandito()
    {
        Instantiate(Bandito, transform.position, transform.rotation, null);
    }
}
