using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI sleepWarningText; // NUEVO: referencia al texto de advertencia

    public float secondsPerGameDay = 600f; // 10 minutos reales = 1 día en juego
    private float gameMinutesPerRealSecond;

    private int hour = 6;
    private int minute = 0;

    private float timer = 0f;

    private bool clockStopped = false; // NUEVO: para saber si ya se detuvo el reloj

    void Start()
    {
        gameMinutesPerRealSecond = (24f * 60f) / secondsPerGameDay;
        if (sleepWarningText != null)
            sleepWarningText.gameObject.SetActive(false); // Ocultar mensaje al inicio
    }

    void Update()
    {
        if (clockStopped) return; // Si ya se detuvo, no seguimos

        timer += Time.deltaTime * gameMinutesPerRealSecond;

        if (timer >= 1f)
        {
            int minutesToAdd = Mathf.FloorToInt(timer);
            timer -= minutesToAdd;

            minute += minutesToAdd;
            if (minute >= 60)
            {
                hour += minute / 60;
                minute = minute % 60;

                if (hour >= 24)
                    hour = hour % 24;
            }

            // Verificamos si llegamos a las 23:00
            if (hour >= 23)
            {
                hour = 23;
                minute = 0;
                clockStopped = true;

                if (sleepWarningText != null)
                {
                    sleepWarningText.text = "Debes ir a dormir, mañana será otro día.";
                    sleepWarningText.gameObject.SetActive(true);
                }
            }
        }

        clockText.text = $"{hour:D2}:{minute:D2}";
    }

    public void ResetClock()
    {
        hour = 6;
        minute = 0;
        timer = 0f;
        clockStopped = false;

        if (sleepWarningText != null)
            sleepWarningText.gameObject.SetActive(false);
    }
}
