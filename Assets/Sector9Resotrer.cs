using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector9Restorer : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("VolverDesdeCables", 0) == 1)
        {
            PlayerPrefs.SetInt("VolverDesdeCables", 0); // Limpiar la bandera

            if (PowerOutageController.Instance != null)
            {
                PowerOutageController.Instance.RestoreLights();
            }

            GameClock reloj = FindObjectOfType<GameClock>();
            if (reloj != null)
            {
                reloj.ResumeClock();
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

