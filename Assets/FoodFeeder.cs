using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class FoodFeeder : MonoBehaviour
{
    public GameObject foodPanel; // Panel del Canvas para escoger comida
    public Button foodButton;    // Botón para confirmar comida
    public GameObject[] foodObjects; // Los 4 tipos de comida (en la escena)

    private bool isPlayerNear = false; // Para saber si el jugador está cerca
    private bool canFeed = false; // Para saber si ya se puede alimentar

    private GameClock gameClock; // Referencia al reloj

    void Start()
    {
        foodPanel.SetActive(false); // Ocultar el panel de comida al iniciar

        // Hacemos invisibles todos los alimentos
        foreach (GameObject food in foodObjects)
        {
            food.SetActive(false);
        }

        foodButton.onClick.AddListener(MakeFoodVisible); // Asociamos el botón a la función
        gameClock = FindObjectOfType<GameClock>(); // Buscamos el GameClock en la escena
    }

    void Update()
    {
        if (gameClock != null && gameClock.GetHour() >= 14)
        {
            canFeed = true;
        }

        if (isPlayerNear && canFeed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                foodPanel.SetActive(true);
            }
        }
        else
        {
            if (!isPlayerNear)
            {
                foodPanel.SetActive(false); // Ocultamos el panel si el jugador se aleja
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            foodPanel.SetActive(false); // Cerramos el panel también al salir
        }
    }

    void MakeFoodVisible()
    {
        // Activar todos los alimentos
        foreach (GameObject food in foodObjects)
        {
            food.SetActive(true);
        }

        foodPanel.SetActive(false); // Cerrar el panel de comida
    }
}
