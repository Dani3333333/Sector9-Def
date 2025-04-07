using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    public float secondsPerGameDay = 600f; // 10 minutos reales = 1 día en juego
    private float gameMinutesPerRealSecond;

    private int hour = 6;
    private int minute = 0;

    private float timer = 0f;

    void Start()
    {
        gameMinutesPerRealSecond = (24f * 60f) / secondsPerGameDay;
    }

    void Update()
    {
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
        }

        clockText.text = $"{hour:D2}:{minute:D2}";
    }
}
