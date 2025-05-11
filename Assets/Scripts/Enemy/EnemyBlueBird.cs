using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueBird : Enemy
{
    [Header("BlueBird details")]
    [SerializeField] private float travelDistance = 8;
    private Vector3[] waypoints;
    [SerializeField] private float flyForce= 1.5f;
    private bool inPlayMode;
    private int wayIndex;
    protected override void Start()
    {
        base.Start();
        waypoints = new Vector3[2];
        waypoints[0] = new Vector3(transform.position.x - travelDistance / 2, transform.position.y);
        waypoints[1] = new Vector3(transform.position.x + travelDistance / 2, transform.position.y);
        wayIndex = Random.Range(0, waypoints.Length);
        canMove = true;
        inPlayMode = true;
    }
    protected override void Update()
    {
        base.Update();
        HandleMovement();
    }
    private void FlyUp() => rb.velocity = new Vector2(rb.velocity.x, flyForce);
     private void HandleMovement()
    {
        if(canMove == false)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[wayIndex],movespeed * Time.deltaTime);
        FlipController(waypoints[wayIndex].x);
        if (Vector2.Distance(transform.position, waypoints[wayIndex]) < .1f){
            wayIndex++;
            if (wayIndex >= waypoints.Length)
            {
                wayIndex = 0;
            }
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(inPlayMode == false)
        {
            float distance = travelDistance / 2;
            Vector3 leftPos=new Vector3(transform.position.x - distance,transform.position.y);
            Vector3 rightPos = new Vector3(transform.position.x + distance, transform.position.y);
            Gizmos.DrawLine(leftPos, rightPos);
            Gizmos.DrawWireSphere(leftPos, .5f);
            Gizmos.DrawWireSphere(leftPos, .5f);
        }
        else
        {
            Gizmos.DrawLine(transform.position, waypoints[0]);
            Gizmos.DrawLine(transform.position, waypoints[1]);
            Gizmos.DrawWireSphere(waypoints[0], .5f);
            Gizmos.DrawWireSphere(waypoints[1], .5f);
        }
    }
    public override void Die()
    {
        base.Die();
        canMove = false;
    }
}
