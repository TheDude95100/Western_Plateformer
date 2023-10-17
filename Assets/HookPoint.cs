using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoint : MonoBehaviour
{

    SpringJoint2D joint;
    PlayerController player;
    LineRenderer lineRenderer;

    private void Start()
    {
        joint = GetComponentInParent<SpringJoint2D>();
        lineRenderer = GetComponentInParent<LineRenderer>();
    }

    public void Update()
    {
        if(lineRenderer.enabled == true)
        {
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, transform.position);
            if (player.isSwinging == false)
            {
                BreakJoint();
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player = col.GetComponent<PlayerController>();
            joint.connectedBody = col.GetComponent<Rigidbody2D>();
            player.Swinging = true;
            PlayerManager.instance.setHookPoint(this);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.Swinging = false;
            PlayerManager.instance.setHookPoint(null);
        }
    }

    public void BreakJoint()
    {
        joint.enabled = false;
        lineRenderer.enabled = false;
    }

    public void CreateJoint()
    {
        joint.enabled = true;
        lineRenderer.enabled= true;
        player.isSwinging = true;
        
    }
   
}
