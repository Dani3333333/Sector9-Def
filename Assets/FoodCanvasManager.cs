using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class FoodCanvasManager : MonoBehaviour
{
    public GameObject foodCanvas;
    private FoodStationTrigger foodStation;

    void Start()
    {
        foodCanvas.SetActive(false);
    }

    public void OpenFoodCanvas(FoodStationTrigger station)
    {
        foodCanvas.SetActive(true);
        foodStation = station;
        Time.timeScale = 0f; // Pausar el juego si quieres
    }

    public void CloseFoodCanvas()
    {
        foodCanvas.SetActive(false);
        Time.timeScale = 1f;

        if (foodStation != null)
        {
            foodStation.DisablePrompt(); // Ocultar el prompt
        }
    }
}
