using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    public bool isBeingInspected = false;
    private bool isOutsideCell = false; // Ahora esta es privada
    public Transform cellEntrance;
    private Vector3 initialPosition;

    public Transform inspectionPoint; // Punto donde se inspeccionará al prisionero

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isBeingInspected || waypoints == null || waypoints.Length == 0) return;

        if (isOutsideCell)
        {
            transform.position = Vector3.MoveTowards(transform.position, cellEntrance.position, speed * Time.deltaTime);
            return;
        }

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void ExitCell()
    {
        isOutsideCell = true;
    }

    public void ReturnToCell()
    {
        isOutsideCell = false;
        transform.position = initialPosition;
    }

    // Propiedad pública para acceder a isOutsideCell
    public bool IsOutsideCell
    {
        get { return isOutsideCell; }
        set { isOutsideCell = value; }
    }

    //  NUEVO MÉTODO PARA ASIGNAR WAYPOINTS DESDE EL MANAGER
    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    // Método para establecer el punto de inspección
    public void SetInspectionPoint(Transform newInspectionPoint)
    {
        inspectionPoint = newInspectionPoint;
    }
}
