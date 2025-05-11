using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : MonoBehaviour
{
    private Animator anim;
    [Header("Fire timer")]
    [SerializeField] private float activeDelay = 1.0f; 
    [SerializeField] private float activeTime = 2.0f;  
    private bool isTriggered; 
    private bool isActive;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Player player = collision.GetComponent<Player>();
    //    if (player != null)
    //    {
    //        if(!isTriggered)
    //        {
    //            StartCoroutine(ActiveFireTrap());
    //        }
    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Player player = collision.GetComponent<Player>();
    //    if (player != null && isActive)
    //    {
    //        player.Knock(transform.position.x);
    //    }
    //}
    private void Start()
    {
        StartCoroutine(FireTrapLoop());
    }

    private IEnumerator FireTrapLoop()
    {
        while (true)
        {
           
            yield return new WaitForSeconds(activeDelay);

            
            isActive = true;
            anim.SetBool("active", true);

           
            yield return new WaitForSeconds(activeTime);

            
            isActive = false;
            anim.SetBool("active", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && isActive)
        {
            player.Knock(transform.position.x,1);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && isActive)
        {
            player.Knock(transform.position.x,1);
        }
    }

    //private IEnumerator ActiveFireTrap() {
    //    isTriggered = true;
    //    yield return new WaitForSeconds(activeDelay);
    //    isActive = true;
    //    anim.SetBool("active", true);
    //    yield return new WaitForSeconds(activeTime);
    //    isActive = false;
    //    anim.SetBool("active", false);
    //    isTriggered = false;
    //}



}
