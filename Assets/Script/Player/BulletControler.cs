using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BulletControler : MonoBehaviour
{
    [SerializeField] float bulletForce;
    private void Awake()
    {
        float sign = PlayerManager.instance.getPlayerLocalScaleSign();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(sign*bulletForce, 0));
        transform.parent = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
