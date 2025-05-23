using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startpoint : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            anim.SetTrigger("active");
        }
    }
}
