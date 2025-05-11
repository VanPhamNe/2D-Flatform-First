using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class EnemyPlant : Enemy
{
    [Header("Plant Details")]
    public float lastTimeAttack;
    private float attackCooldown= 1.5f;
    [SerializeField] private EnemyBullet bulletPrelabs;
    [SerializeField] private Transform gunpoint;
    [SerializeField] private float bulletSpeed = 7;
    protected override void Update()
    {
        base.Update();
        bool canAttack = Time.time > lastTimeAttack+ attackCooldown;
        //bi tan cong khi o 5 giay trong game=> last attack la 5
        //thoi gian trong game neu la 8(Time.time) > 5 + 1.5f neu dieu kien nay sai thi ko the tan cong

        if (IsplayerDetected && canAttack)
        {
            Attack();
        }

    }
    private void Attack()
    {
        lastTimeAttack = Time.time;
        anim.SetTrigger("attack");
    }
    private void CreateBullet()
    {
        EnemyBullet newBullet = Instantiate(bulletPrelabs, gunpoint.position, Quaternion.identity);
        Vector2 newVelocity = new Vector2(bulletSpeed * facingDirection, 0);
        newBullet.SetVelocity(newVelocity);
        Destroy(newBullet.gameObject, 10);
        

    }
    protected override void HandleAnimation()
    {
        //Giu trong tru khi muon update gi them cho plant

    }
}
