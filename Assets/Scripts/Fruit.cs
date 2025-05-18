using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameManager gameManager;
    private Animator anim;
    [SerializeField] private GameObject pickupVFX; 
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        setRandomLook();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            gameManager.AddFruit();
            AudioManager.instance.PlaySFX(7);
            gameObject.SetActive(false);
            GameObject newFX = Instantiate(pickupVFX,transform.position,Quaternion.identity);
            Destroy(newFX, .5f);
        }
    }
    private void setRandomLook() { 
        if(gameManager.FruitHaveRandomLook()==false)
        {
            return;
        }
        int randomIndex =Random.Range(0, 3);
        anim.SetFloat("FruitIndex", randomIndex);
    }
}
