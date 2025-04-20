using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentDay = 0;
    public bool isTutorial = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Inicializamos el juego. El tutorial es el d�a 0
        if (isTutorial)
        {
            currentDay = 0;
        }
    }

    public void EndTutorial()
    {
        // Al finalizar el tutorial, se avanza al D�a 1
        isTutorial = false;
        currentDay = 1;
    }

    public void NextDay()
    {
        // Avanza al siguiente d�a, pero no pasa del D�a 3
        if (currentDay < 3)
        {
            currentDay++;
        }
        else
        {
            Debug.Log("Ya se ha completado el ciclo de 3 d�as.");
        }
    }

    // Para verificar si ya se pas� al D�a 3
    public bool CanSleep()
    {
        // Solo se puede dormir si el d�a es menor a 3
        return currentDay < 3;
    }
}
