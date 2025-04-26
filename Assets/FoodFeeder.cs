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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameClock != null && gameClock.CanFeedPrisoners())
        {
            foodPanelUI.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foodPanelUI.SetActive(false);
            playerInRange = false;
        }
    }

    public void SpawnFood()
    {
        if (!playerInRange) return;

        foreach (var food in foodObjects)
        {
            food.SetActive(true); // Aparecen los objetos
        }

        foodPanelUI.SetActive(false); // Ocultamos el panel después
    }
}

