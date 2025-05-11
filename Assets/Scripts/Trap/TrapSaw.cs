using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapSaw : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float cooldown;
    private Vector3[] waypointPosition;
    private SpriteRenderer sr;
    public int WaypointIndex=1;
    private int movingDirection = 1;
    private bool canMove = true;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {

        UpdateWayPointsInfo();
        transform.position = waypointPosition[0];

    }

    private void UpdateWayPointsInfo()
    {
        waypointPosition = new Vector3[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointPosition[i] = waypoints[i].position;
        }
    }

    private void Update()
    {
        anim.SetBool("active", canMove);
        if (canMove==false) {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, waypointPosition[WaypointIndex], moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, waypointPosition[WaypointIndex]) < 0.1f)
        {
            //WaypointIndex++;
            //if (WaypointIndex >= waypoints.Length)
            //{
            //    WaypointIndex = 0;
            //    StartCoroutine(StopMovement(cooldown));
            //}

            //Trap nang cap
            if (WaypointIndex == waypointPosition.Length-1 || WaypointIndex ==0)
            {
                movingDirection = movingDirection * -1;
                StartCoroutine(StopMovement(cooldown));
            }
            WaypointIndex += movingDirection;
        }
    }
    private IEnumerator StopMovement(float delay) {
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        sr.flipX = !sr.flipX;
    }

}
