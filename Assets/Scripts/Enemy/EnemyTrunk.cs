using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrunk : Enemy
{
    [Header("Trunk Details")]
    public float lastTimeAttack;
    private float attackCooldown = 1.5f;
    [SerializeField] private EnemyBullet bulletPrelabs;
    [SerializeField] private Transform gunpoint;
    [SerializeField] private float bulletSpeed = 7;
    protected override void Update()
    {
        base.Update();
        if (isDead)
        {
            return;
        }
        bool canAttack = Time.time > lastTimeAttack + attackCooldown;
        //bi tan cong khi o 5 giay trong game=> last attack la 5
        //thoi gian trong game neu la 8(Time.time) > 5 + 1.5f neu dieu kien nay sai thi ko the tan cong

        if (IsplayerDetected && canAttack)
        {
            Attack();
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

    private void HandleMovement()
    {
        if (idleTime > 0)
        {
            return;
        }
        rb.velocity = new Vector2(movespeed * facingDirection, rb.velocity.y);


    }
    private void Attack()
    {
        idleTime = idleDuration + attackCooldown;
        lastTimeAttack = Time.time;
        anim.SetTrigger("attack");
    }
    private void CreateBullet()
    {
        EnemyBullet newBullet = Instantiate(bulletPrelabs, gunpoint.position, Quaternion.identity);
        Vector2 newVelocity = new Vector2(bulletSpeed * facingDirection, 0);
        newBullet.SetVelocity(newVelocity);
        if(facingDirection ==1)
        {
            newBullet.FlipSprites();
        }
        Destroy(newBullet.gameObject, 10);


    }
}
