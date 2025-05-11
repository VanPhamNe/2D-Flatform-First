using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool active;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
        {
            return;
        }
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            
            ActiveCheckPoint();
        }
    }
    private void ActiveCheckPoint() {
        active = true;
        anim.SetTrigger("isActive");
        PlayerManager.instance.UpdateCheckpointRespawn(transform);
    }
}
