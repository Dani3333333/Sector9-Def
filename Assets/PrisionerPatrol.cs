using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    public bool isBeingInspected = false;
    public bool isOutsideCell = false; // Variable para saber si el prisionero está fuera de su celda
    public Transform cellEntrance; // Referencia al lugar donde el prisionero debe quedarse cuando sale de la celda
    private Vector3 initialPosition; // Posición inicial dentro de la celda

    void Start()
    {
        // Guardamos la posición inicial (de la celda) para que el prisionero vuelva a su celda después de ser inspeccionado
        initialPosition = transform.position;
    }

    void Update()
    {
        // Si está siendo inspeccionado o no hay waypoints, no se mueve
        if (isBeingInspected || waypoints.Length == 0) return;

        // Si está fuera de la celda, el prisionero se queda quieto en la entrada esperando ser inspeccionado
        if (isOutsideCell)
        {
            transform.position = Vector3.MoveTowards(transform.position, cellEntrance.position, speed * Time.deltaTime);
            return; // El prisionero no se moverá a los waypoints hasta que termine la inspección
        }

        // Si no está fuera de la celda, sigue patrullando normalmente
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // Método para hacer que el prisionero salga de la celda
    public void ExitCell()
    {
        isOutsideCell = true;
    }

    // Método para hacer que el prisionero vuelva a su celda después de la inspección
    public void ReturnToCell()
    {
        isOutsideCell = false;
        transform.position = initialPosition; // Regresa a la posición inicial de la celda
    }
}
