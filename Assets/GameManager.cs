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

    public void StartGame()
    {
        currentDay = 0;
        isTutorial = true;
    }

    public void EndTutorial()
    {
        isTutorial = false;
        currentDay = 1;
    }

    public void NextDay()
    {
        if (!isTutorial)
            currentDay++;
    }
}
    
