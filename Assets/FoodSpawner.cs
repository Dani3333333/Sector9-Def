using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodBuenaPrefab;
    public GameObject foodRegularPrefab;
    public GameObject foodMala1Prefab;
    public GameObject foodMala2Prefab;

    public Transform spawnPointBuena;
    public Transform spawnPointRegular;
    public Transform spawnPointMala1;
    public Transform spawnPointMala2;

    private FoodCanvasManager foodCanvasManager;

    void Start()
    {
        foodCanvasManager = FindObjectOfType<FoodCanvasManager>();
    }

    public void SpawnBuena()
    {
        Instantiate(foodBuenaPrefab, spawnPointBuena.position, Quaternion.identity);
        foodCanvasManager.CloseFoodCanvas();
    }

    public void SpawnRegular()
    {
        Instantiate(foodRegularPrefab, spawnPointRegular.position, Quaternion.identity);
        foodCanvasManager.CloseFoodCanvas();
    }

    public void SpawnMala1()
    {
        Instantiate(foodMala1Prefab, spawnPointMala1.position, Quaternion.identity);
        foodCanvasManager.CloseFoodCanvas();
    }

    public void SpawnMala2()
    {
        Instantiate(foodMala2Prefab, spawnPointMala2.position, Quaternion.identity);
        foodCanvasManager.CloseFoodCanvas();
    }
}
  