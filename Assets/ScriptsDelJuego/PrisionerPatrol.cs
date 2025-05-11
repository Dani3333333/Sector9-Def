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
    public bool isBeingInspected = false;

    private Animator animator; // <-- NUEVO
    private Vector3 lastPosition; // <-- NUEVO

    void Start()
    {
        animator = GetComponent<Animator>(); // <-- NUEVO
        lastPosition = transform.position;   // <-- NUEVO
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movement = currentPosition - lastPosition;

        if (animator != null)
        {
            animator.SetBool("isWalking", movement.magnitude > 0.01f); // <-- CAMBIO DE ANIMACIÓN
        }

        lastPosition = currentPosition;

        if (isWalkingToInspection && inspectionPoint != null)
        {
            MoveTo(inspectionPoint.position, () =>
            {
                isWalkingToInspection = false;
                isOutsideCell = true;

                // Girar 180º para mirar hacia la celda
                transform.rotation = Quaternion.LookRotation(-inspectionPoint.forward); // <-- NUEVO
            });
        }
        else if (isWalkingBack && cellEntrance != null)
        {
            MoveTo(cellEntrance.position, () =>
            {
                isWalkingBack = false;
                isOutsideCell = false;
                currentWaypointIndex = 0;
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
        });
    }

    void MoveTo(Vector3 destination, System.Action onArrival)
    {
        // ROTAR HACIA EL DESTINO
        Vector3 direction = (destination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f); // <-- ROTACIÓN SUAVE
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
    }

    public void ReturnToCell()
    {
        isWalkingBack = true;
        isWalkingToInspection = false;
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

