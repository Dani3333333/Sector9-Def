using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentDay = 0;
    public bool isTutorial = true;

    // Evento que se lanza al comenzar un nuevo día
    public delegate void NuevoDiaEvent();
    public static event NuevoDiaEvent OnNuevoDia;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (isTutorial)
        {
            currentDay = 0;
        }
    }

    public void EndTutorial()
    {
        isTutorial = false;
        currentDay = 1;
    }

    public void NextDay()
    {
        if (currentDay < 3)
        {
            currentDay++;
            OnNuevoDia?.Invoke(); //  Disparamos el evento de nuevo día
        }
        else
        {
            Debug.Log("Ya se ha completado el ciclo de 3 días.");
        }
    }

    public bool CanSleep()
    {
        return currentDay < 3;
    }
}

