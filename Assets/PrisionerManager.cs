using System.Collections.Generic;
using UnityEngine;

public class PrisonerManager : MonoBehaviour
{
    public GameObject prisonerPrefab;
    public Transform[] spawnPoints; // Asigna 4 puntos en el Inspector

    private List<GameObject> currentPrisoners = new List<GameObject>();

    public void SpawnPrisonersForDay(int day)
    {
        ClearAllPrisoners();

        int prisonerCount = Mathf.Min(1 + (day - 1), 4); // Día 1: 2, Día 2: 3, Día 3: 4
        for (int i = 0; i < prisonerCount; i++)
        {
            GameObject newPrisoner = Instantiate(prisonerPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            currentPrisoners.Add(newPrisoner);
        }
    }

    public void ClearAllPrisoners()
    {
        foreach (var prisoner in currentPrisoners)
        {
            Destroy(prisoner);
        }
        currentPrisoners.Clear();
    }
}
