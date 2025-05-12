using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FoodFeeder : MonoBehaviour
{
    public GameObject foodPanelUI;
    public GameObject[] foodPrefabs; // NUEVO: Prefabs de comida
    public Transform[] foodSpawnPoints; // NUEVO: Puntos donde instanciar la comida
    public GameClock gameClock;

    private bool playerInRange = false;
    private bool foodSpawned = false;
    private List<GameObject> spawnedFoodObjects = new List<GameObject>();

    private void Start()
    {
        if (foodPanelUI != null)
            foodPanelUI.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && gameClock != null && gameClock.CanFeedPrisoners() && !foodSpawned)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnFood();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameClock != null && gameClock.CanFeedPrisoners())
        {
            playerInRange = true;

            if (foodPanelUI != null && !foodSpawned)
                foodPanelUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (foodPanelUI != null)
                foodPanelUI.SetActive(false);

            playerInRange = false;
        }
    }

    private void SpawnFood()
    {
        ClearOldFood();

        for (int i = 0; i < foodPrefabs.Length && i < foodSpawnPoints.Length; i++)
        {
            GameObject food = Instantiate(foodPrefabs[i], foodSpawnPoints[i].position, foodSpawnPoints[i].rotation);
            spawnedFoodObjects.Add(food);
        }

        if (foodPanelUI != null)
            foodPanelUI.SetActive(false);

        foodSpawned = true;
    }

    public void ResetFoodSpawn()
    {
        ClearOldFood();
        foodSpawned = false;

        if (playerInRange && foodPanelUI != null)
            foodPanelUI.SetActive(true);
    }

    private void ClearOldFood()
    {
        foreach (var obj in spawnedFoodObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedFoodObjects.Clear();
    }
}

