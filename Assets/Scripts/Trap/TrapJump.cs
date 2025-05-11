using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TrapJump : MonoBehaviour
{
    [SerializeField] private float pushPower = 10f;
    [SerializeField] private float duration=.5f;
    protected Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Push(transform.up*pushPower,duration);
        }
        anim.SetTrigger("active");
    }
}
