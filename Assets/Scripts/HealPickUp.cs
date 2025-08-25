using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickUp : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject pickupVFX;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHeal = collision.GetComponent<PlayerHealth>();
        if (playerHeal != null)
        {
            
            AudioManager.instance.PlaySFX(7);
            playerHeal.AddHeart(1);
            gameObject.SetActive(false);
            GameObject newFX = Instantiate(pickupVFX, transform.position, Quaternion.identity);
            Destroy(newFX, .5f);
        }
    }
}
