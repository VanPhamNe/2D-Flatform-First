using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string groundLayerName = "Ground";
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void FlipSprites() => sr.flipX = !sr.flipX;
    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
        rb.gravityScale = 0f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            collision.GetComponent<Player>().Knock(transform.position.x,1);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            Destroy(gameObject);
        }
    }
}
