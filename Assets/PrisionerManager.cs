using System.Collections.Generic;
using UnityEngine;

public class PrisonerManager : MonoBehaviour
{
    [Header("Prefabs y Spawn Points")]
    public GameObject[] prisonerPrefabs; // Prisionero2, Prisionero3, Prisionero4
    public Transform[] spawnPoints;      // Sus posiciones de aparición

    [Header("Waypoints")]
    public Transform[] patrolParents; // Cada uno tiene hijos que son los puntos de patrulla

    [Header("Entradas de celda")]
    public Transform[] cellEntrances; // Punto de espera al salir de la celda (uno por prisionero instanciado)

    private int maxPrisoners = 4;

    public void SpawnPrisonersForDay(int day)
    {
        // Día 0 ya está Prisonero1 en la escena, así que empezamos desde el Día 1
        if (day <= 0 || day >= maxPrisoners)
            return;

        int index = day - 1;

        GameObject prisonerToSpawn = prisonerPrefabs[index];
        Transform spawnPoint = spawnPoints[index];
        Transform patrolParent = patrolParents[index];
        Transform cellEntrance = (cellEntrances.Length > index) ? cellEntrances[index] : null;

        GameObject prisonerInstance = Instantiate(prisonerToSpawn, spawnPoint.position, spawnPoint.rotation);
        prisonerInstance.tag = "Prisionero";

        PrisonerPatrol patrol = prisonerInstance.GetComponent<PrisonerPatrol>();
        if (patrol != null)
        {
            // Asignar waypoints de patrulla
            if (patrolParent != null)
            {
                Transform[] waypoints = new Transform[patrolParent.childCount];
                for (int i = 0; i < patrolParent.childCount; i++)
                    waypoints[i] = patrolParent.GetChild(i);

                patrol.SetWaypoints(waypoints);
            }

            // Asignar punto de salida de la celda
            if (cellEntrance != null)
            {
                patrol.SetInspectionPoint(cellEntrance);
            }
            else
            {
                Debug.LogWarning($"No se asignó cellEntrance para el prisionero {prisonerInstance.name}");
            }

        }
    }
}
