using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRhino : Enemy
{
    [Header("Rhino info")]
    
    [SerializeField] private Vector2 impactPower;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float SpeedUp = .6f;
    private float defaulSpeed;

    protected override void Start()
    {
        base.Start();
        defaulSpeed =maxSpeed;
    }
    protected override void Update()
    {
        base.Update();
        HandleCharge();
    }
    private void HandleCharge()
    {
        if(canMove==false) return;
        movespeed = movespeed + (Time.deltaTime*SpeedUp);
        if(movespeed > maxSpeed) maxSpeed = movespeed;
        rb.velocity = new Vector2(movespeed * facingDirection,rb.velocity.y);
     
        if (isWallDetected)
        {
            WallHit();
        }
        if (!isGroundedInFront)
        {
            TurnAround();
        }
    }

    private void TurnAround()
    {
        movespeed=defaulSpeed;
        canMove = false;
        rb.velocity = Vector2.zero;
        Flip();
       
    }

    private void WallHit()
    {
        canMove = false;
        movespeed= defaulSpeed;
        anim.SetBool("hitWall", true);
        rb.velocity = new Vector2(impactPower.x * -facingDirection,impactPower.y);
    }
    private void ChargeOver()
    {
        anim.SetBool("hitWall", false);
        Invoke(nameof(Flip), 1);
    }
    protected override void HandleColision()
    {
        base.HandleColision();
        if(IsplayerDetected && isGrounded)
        {
            canMove = true;
        }
    }
}
