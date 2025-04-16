using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class SleepManager : MonoBehaviour
{
    [Header("Referencias principales")]
    public GameClock gameClock;
    public GameObject hud;
    public GameObject sleepPromptText;

    [Header("Video")]
    public GameObject sleepVideoImage; // RawImage
    public VideoPlayer videoPlayer;

    [Header("Fade")]
    public Image fadePanel;
    public float fadeDuration = 1f;

    private bool canSleep = false;
    private bool isSleeping = false;

    public GameObject sleepText;


    void Update()
    {
        if (canSleep && Input.GetKeyDown(KeyCode.E) && !isSleeping)
        {
            StartCoroutine(SleepWithFadeAndVideoRoutine());
        }
    }

    IEnumerator SleepWithFadeAndVideoRoutine()
    {
        isSleeping = true;

        // Ocultar UI
        hud.SetActive(false);
        sleepPromptText.SetActive(false);

        // FUNDIDO A NEGRO
        yield return StartCoroutine(FadeToBlack());

        // Mostrar RawImage con el video
        sleepVideoImage.SetActive(true);
        videoPlayer.gameObject.SetActive(true);

        // PREPARAR VIDEO (muy importante)
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        // Reproducir el video
        videoPlayer.Play();

        // Esperar a que el video termine
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        // FUNDIDO A NEGRO (por si termina en blanco o hay salto)
        yield return StartCoroutine(FadeToBlack());

        // Ocultar video
        videoPlayer.Stop();
        sleepVideoImage.SetActive(false);
        videoPlayer.gameObject.SetActive(false);

        // Reiniciar el reloj
        gameClock.ResetClock();

        // Mostrar UI
        hud.SetActive(true);

        // FUNDIDO DESDE NEGRO
        yield return StartCoroutine(FadeFromBlack());

        isSleeping = false;
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    IEnumerator FadeFromBlack()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = true;
            sleepPromptText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSleep = false;
            sleepPromptText.SetActive(false);
        }
    }
}
