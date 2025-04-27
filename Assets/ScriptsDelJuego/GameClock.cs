using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    public TextMeshProUGUI sleepWarningText;
    public TextMeshProUGUI feedingWarningText; // NUEVO: texto para advertencia de comida

    public float secondsPerGameDay = 600f; // 10 minutos reales = 1 día en juego
    private float gameMinutesPerRealSecond;

    private int hour = 6;
    private int minute = 0;

    private float timer = 0f;

    private bool clockStopped = false;
    private bool hasFedPrisoners = false;
    private bool warningShown = false;

    void Start()
    {
        gameMinutesPerRealSecond = (24f * 60f) / secondsPerGameDay;

        if (sleepWarningText != null)
            sleepWarningText.gameObject.SetActive(false);

        if (feedingWarningText != null)
            feedingWarningText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (clockStopped) return;

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

            // Mostrar advertencia de comida de 14:00 a 15:00
            if (hour == 14)
            {
                if (!warningShown)
                {
                    warningShown = true;

                    if (clockText != null)
                        clockText.color = Color.red;

                    if (feedingWarningText != null)
                    {
                        feedingWarningText.text = "¡Hora de alimentar a los humanos, si no, morirán!";
                        feedingWarningText.gameObject.SetActive(true);
                    }
                }
            }
            else if (hour >= 15 && warningShown && !hasFedPrisoners)
            {
                // Ocultamos el mensaje y restauramos color después de las 15:00
                warningShown = false;

                if (clockText != null)
                    clockText.color = Color.white;

                if (feedingWarningText != null)
                    feedingWarningText.gameObject.SetActive(false);
            }

            // Fin del día
            if (hour >= 23)
            {
                hour = 23;
                minute = 0;
                clockStopped = true;

                if (!hasFedPrisoners)
                {
                    KillLeastHappyPrisoner();
                }

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
        hasFedPrisoners = false;
        warningShown = false;

        if (clockText != null)
            clockText.color = Color.white;

        if (sleepWarningText != null)
            sleepWarningText.gameObject.SetActive(false);

        if (feedingWarningText != null)
            feedingWarningText.gameObject.SetActive(false);
    }

    // Llama esta función desde el sistema de alimentación cuando el jugador da de comer
    public void MarkPrisonersFed()
    {
        hasFedPrisoners = true;

        if (feedingWarningText != null)
            feedingWarningText.gameObject.SetActive(false);

        if (clockText != null)
            clockText.color = Color.white;
    }

    private void KillLeastHappyPrisoner()
    {
        // Lógica que selecciona y elimina al prisionero con menor felicidad.
        // Ejemplo: PrisonerManager.Instance.KillLeastHappy();
        Debug.Log("Un prisionero ha muerto por no recibir comida.");
    }
    public bool CanFeedPrisoners()
    {
        return hour >= 6;
    }




}
