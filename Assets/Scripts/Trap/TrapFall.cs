using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFall : MonoBehaviour
{
    public Vector3[] wayPoints;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D[] colliders;
    [SerializeField]private float travelDistance;
    [SerializeField] private float speed = 0.75f;
    private int wayPointIndex ;
    private bool canMove;
    [Header("Fall details")]
    [SerializeField] private float impactSpeed = 3;
    [SerializeField] private float impactDuration = .1f;
    private float impactTime;
    private bool impactHappen;
    [SerializeField] private float fallDelay = .5f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        colliders = GetComponents<BoxCollider2D>();
    }
    private void Start()
    {
        SetWayPoints();
        float randomDelay = Random.Range(0, 0.6f);
        Invoke("ActivePlatform", randomDelay);
    }
    private void SetWayPoints()
    {
        wayPoints = new Vector3[2];
        float yOffset = travelDistance /2;
        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);

    }
    private void Update()
    {
        HandleImpact();
        HandleMovement();
    }
    private void HandleMovement() {
        if (!canMove) return;
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex], speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoints[wayPointIndex]) < 0.1f)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impactHappen)
        {
            return;
        }
         Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Invoke("OffPlatform", fallDelay);
            impactTime = impactDuration;
            impactHappen = true;
        }
    }
    private void OffPlatform()
    {
        anim.SetTrigger("deactive");
        canMove = false;
        rb.isKinematic = false;
        rb.gravityScale = 3.5f;
        rb.drag = 0.5f;
        foreach (BoxCollider2D col in colliders)
        {
            col.enabled = false;
        }
    }
    private void ActivePlatform() => canMove = true;
    private void HandleImpact()
    {
        if (impactTime <0)
        {
            return;
        }
        impactTime -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,transform.position+(Vector3.down*10),impactSpeed*Time.deltaTime);
    }

}
