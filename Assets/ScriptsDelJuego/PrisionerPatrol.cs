using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;

    public Transform cellEntrance;
    public Transform inspectionPoint;

    private bool isOutsideCell = false;
    private bool isWalkingToInspection = false;
    private bool isWalkingBack = false;

    public bool IsOutsideCell => isOutsideCell;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isWalkingToInspection && inspectionPoint != null)
        {
            MoveTo(inspectionPoint.position, () =>
            {
                isWalkingToInspection = false;
                isOutsideCell = true;

                // Al llegar al inspectionPoint: poner Idle
                animator.SetBool("isWalking", false);

                // Mirar hacia dentro de la celda
                transform.rotation = Quaternion.LookRotation(-inspectionPoint.forward);
            });
        }
        else if (isWalkingBack && cellEntrance != null)
        {
            MoveTo(cellEntrance.position, () =>
            {
                isWalkingBack = false;
                isOutsideCell = false;
                currentWaypointIndex = 0;

                // Al volver a la celda, siempre caminar (no idle)
                animator.SetBool("isWalking", true);
            });
        }
        else if (!isOutsideCell && !isWalkingToInspection && !isWalkingBack && waypoints.Length > 0)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Transform target = waypoints[currentWaypointIndex];
        MoveTo(target.position, () =>
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            // Siempre caminar en patrulla
            animator.SetBool("isWalking", true);
        });

        // Mientras se mueve hacia el waypoint, caminar
        animator.SetBool("isWalking", true);
    }

    void MoveTo(Vector3 destination, System.Action onArrival)
    {
        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            onArrival?.Invoke();
        }
    }

    public void ExitCell()
    {
        isWalkingToInspection = true;
        isWalkingBack = false;
        animator.SetBool("isWalking", true); // Comienza a caminar hacia inspectionPoint
    }

    public void ReturnToCell()
    {
        isWalkingBack = true;
        isWalkingToInspection = false;
        animator.SetBool("isWalking", true); // Camina de vuelta a la celda
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    public void SetInspectionPoint(Transform point)
    {
        inspectionPoint = point;
    }
}
