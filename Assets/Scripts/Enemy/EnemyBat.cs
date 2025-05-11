using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBat : Enemy
{
    [Header("Bat Details")]
    [SerializeField] private float agroRadius = 7;
    private Vector3 orginalPos;
    private Vector3 destination;
    private bool canDetectedPlayer;
    public Collider2D target;
    [SerializeField] private float chaseDuration = 1;
    private float chaseTime;
   

    protected override void Awake()
    {
        base.Awake();
        
        orginalPos = transform.position;
        canMove = false;
    }
    protected override void Update()
    {
        base.Update();
        chaseTime -= Time.deltaTime;
        if (idleTime < 0) { 
            canDetectedPlayer = true;
        }
        HandlePlayerDetected();
        HandleMovement();
    }
    private void HandleMovement()
    {
        if (canMove == false) return;
        FlipController(destination.x);
        transform.position = Vector2.MoveTowards(transform.position,destination,movespeed * Time.deltaTime);
        if (chaseTime > 0 && target!=null)
        {
            destination =target.transform.position;
        }
        if(Vector2.Distance(transform.position,destination) < .1f){
            if (destination == orginalPos)
            {
                idleTime = idleDuration;
                canDetectedPlayer = false;
                canMove = false;
                anim.SetBool("moving",false);
                target = null;
            }
            else
            {
                destination = orginalPos;
                
            }
        }
    }
    private void AllowMove()
    {
        canMove=true;
    }
    public override void Die()
    {
        base.Die();
        canMove = false;
    }
    private void HandlePlayerDetected()
    {
        if(target == null && canDetectedPlayer)
        {
            target = Physics2D.OverlapCircle(transform.position, agroRadius, whatIsPlayer);
            if (target != null) { 
                chaseTime =chaseDuration;
                destination = target.transform.position;
                canDetectedPlayer = false;
                anim.SetBool("moving", true);

            }
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, agroRadius);
    }
}
