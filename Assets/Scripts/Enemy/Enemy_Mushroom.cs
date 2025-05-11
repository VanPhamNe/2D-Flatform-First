using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    protected override void Update()
    {
        base.Update();
        if(isDead)
        {
            return;
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

    private void HandleMovement() {
        if (idleTime > 0)
        {
            return;
        }
        rb.velocity = new Vector2(movespeed * facingDirection, rb.velocity.y);
        
        
    }
}
