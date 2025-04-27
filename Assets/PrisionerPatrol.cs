using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentWaypointIndex = 0;
    public bool isBeingInspected = false;
    public bool isOutsideCell = false; // Variable para saber si el prisionero est� fuera de su celda
    public Transform cellEntrance; // Referencia al lugar donde el prisionero debe quedarse cuando sale de la celda
    private Vector3 initialPosition; // Posici�n inicial dentro de la celda

    void Start()
    {
        // Guardamos la posici�n inicial (de la celda) para que el prisionero vuelva a su celda despu�s de ser inspeccionado
        initialPosition = transform.position;
    }

    void Update()
    {
        // Si est� siendo inspeccionado o no hay waypoints, no se mueve
        if (isBeingInspected || waypoints.Length == 0) return;

        // Si est� fuera de la celda, el prisionero se queda quieto en la entrada esperando ser inspeccionado
        if (isOutsideCell)
        {
            transform.position = Vector3.MoveTowards(transform.position, cellEntrance.position, speed * Time.deltaTime);
            return; // El prisionero no se mover� a los waypoints hasta que termine la inspecci�n
        }

        // Si no est� fuera de la celda, sigue patrullando normalmente
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // M�todo para hacer que el prisionero salga de la celda
    public void ExitCell()
    {
        isOutsideCell = true;
    }

    // M�todo para hacer que el prisionero vuelva a su celda despu�s de la inspecci�n
    public void ReturnToCell()
    {
        isOutsideCell = false;
        transform.position = initialPosition; // Regresa a la posici�n inicial de la celda
    }
}
