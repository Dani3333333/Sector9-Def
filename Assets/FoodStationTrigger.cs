using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodStationTrigger : MonoBehaviour
{
    public GameObject promptUI;
    private bool playerInRange = false;
    private bool foodTime = false;
    private bool foodGiven = false; // NUEVO: Para evitar volver a abrir si ya diste comida

    void Start()
    {
        promptUI.SetActive(false);
    }

    void Update()
    {
        foodTime = FindObjectOfType<GameClock>().IsFeedingTime();

        if (playerInRange && foodTime && !foodGiven)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<FoodCanvasManager>().OpenFoodCanvas(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && foodTime && !foodGiven)
        {
            promptUI.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptUI.SetActive(false);
            playerInRange = false;
        }
    }

    public void DisablePrompt()
    {
        promptUI.SetActive(false);
        foodGiven = true;
    }
}
