using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControler : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    public void ShootBullet()
    {
        Instantiate(bullet, this.transform);
    }
}
