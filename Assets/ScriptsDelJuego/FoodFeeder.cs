using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;





public class FoodFeeder : MonoBehaviour
{
    public GameObject foodPanelUI; // El panel que aparece cuando el jugador entra
    public GameObject[] foodObjects; // Los 4 objetos de comida
    public GameClock gameClock; // Referencia al reloj del juego

    private bool playerInRange = false;
    private bool foodSpawned = false;

    private void Start()
    {
        if (foodPanelUI != null)
            foodPanelUI.SetActive(false);

        // Al inicio, ocultamos las comidas
        foreach (var food in foodObjects)
        {
            food.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && gameClock != null && gameClock.CanFeedPrisoners() && !foodSpawned)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Si el jugador pulsa "E"
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

            // Solo mostrar el panel si todavía no se ha dado comida
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
        foreach (var food in foodObjects)
        {
            food.SetActive(true); // Aparecen los objetos
        }

        if (foodPanelUI != null)
            foodPanelUI.SetActive(false); // Ocultamos el panel después

        foodSpawned = true; // Ya hemos dado de comer, no queremos volver a mostrar el panel
    }
}

