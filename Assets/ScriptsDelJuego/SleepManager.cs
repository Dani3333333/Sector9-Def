using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class SleepManager : MonoBehaviour
{
    public GameClock gameClock;

    public GameObject hud;
    public GameObject sleepVideoImage; // RawImage que muestra el video
    public VideoPlayer videoPlayer;

    private bool canSleep = false;
    private bool isSleeping = false;

    void Update()
    {
        if (canSleep && Input.GetKeyDown(KeyCode.E) && !isSleeping)
        {
            StartCoroutine(SleepWithVideoRoutine());
        }
    }

    IEnumerator SleepWithVideoRoutine()
    {
        isSleeping = true;

        // Ocultar HUD
        hud.SetActive(false);

        // Mostrar video
        sleepVideoImage.SetActive(true);
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        // Esperar a que el video termine
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // Reiniciar reloj
        gameClock.ResetClock();

        // Ocultar video
        videoPlayer.Stop();
        sleepVideoImage.SetActive(false);
        videoPlayer.gameObject.SetActive(false);

        // Mostrar HUD
        hud.SetActive(true);

        isSleeping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = false;
        }
    }
}
    