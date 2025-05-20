using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChicken : Enemy
{
    [SerializeField] private float aggroDuration;
    private float aggroTimer;
    private bool canFlip=true;
    protected override void Update()
    {
        base.Update();
        aggroTimer -= Time.deltaTime;

        if (isDead)
        {
            return;
        }
        if (IsplayerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;
        }
        if(aggroTimer < 0)
        {
            canMove=false;
        }
        if (isGrounded)
        {
            HandleTurnAround();
        }
        HandleMovement();


    }

    private void HandleTurnAround()
    {
        if (!isGroundedInFront || isWallDetected)
        {
            Flip();
            idleTime = idleDuration;
            rb.velocity = Vector2.zero;
        }
    }
    protected override void FlipController(float xValue)
    {
        if (xValue > transform.position.x && !facingRight || xValue < transform.position.x && facingRight)
        {
            if (canFlip) { 
                canFlip = false;
                Invoke(nameof(Flip),.3f);
            }
        }
    }
    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }
    private void HandleMovement()
    {
        if (canMove == false)
        {
            return;
        }
        FlipController(player.transform.position.x);
        rb.velocity = new Vector2(movespeed * facingDirection, rb.velocity.y);


    }
}
