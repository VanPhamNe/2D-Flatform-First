using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBallSpike : MonoBehaviour
{
    public Rigidbody2D spikerb;
    [SerializeField] private float pushForce;
    [SerializeField] private float velocityThreshold = 0.1f;
    private void Start()
    {
        Vector2 pushVector = new Vector2(pushForce, 0);
        spikerb.AddForce(pushVector, ForceMode2D.Impulse);
        InvokeRepeating("CheckAndPushBall", 0f, 0.1f);
    }
    private void CheckAndPushBall()
    {
        
        if (spikerb.velocity.magnitude < velocityThreshold)
        {
            
            Vector2 pushVector = new Vector2(pushForce, 0);
            spikerb.AddForce(pushVector, ForceMode2D.Impulse);
        }
    }
}
