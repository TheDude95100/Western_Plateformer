using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision avec " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Bullet"))
        {
            PlayerManager.instance.Score += 100;
            Destroy(this.gameObject);

        }
    }
}
