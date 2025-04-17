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
    public GameObject sleepVideoImage;
    public VideoPlayer videoPlayer;

    [Header("Fade")]
    public Image fadePanel;
    public float fadeDuration = 1f;

    [Header("Texto de Día")]
    public TextMeshProUGUI dayText;
    public float dayTextDuration = 2.5f;

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
        hud.SetActive(false);
        sleepPromptText.SetActive(false);

        yield return StartCoroutine(FadeToBlack());

        sleepVideoImage.SetActive(true);
        videoPlayer.gameObject.SetActive(true);

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();

        if (sleepText != null) sleepText.SetActive(true);

        while (videoPlayer.isPlaying)
            yield return null;

        if (sleepText != null) sleepText.SetActive(false);

        yield return StartCoroutine(FadeToBlack());

        videoPlayer.Stop();
        sleepVideoImage.SetActive(false);
        videoPlayer.gameObject.SetActive(false);

        // Reiniciar el reloj
        gameClock.ResetClock();

        //  ACTUALIZAR DÍA DESDE EL GAME MANAGER
        if (GameManager.Instance.isTutorial)
        {
            GameManager.Instance.EndTutorial(); // Salimos del tutorial  Día 1
        }
        else
        {
            GameManager.Instance.NextDay(); // Avanzamos un día
        }

        //  Mostrar texto de día (si se quiere mantener además del UI DayMessageUI)
        yield return StartCoroutine(ShowDayText("Día " + GameManager.Instance.currentDay));

        //  Mostrar texto del sistema externo DayMessageUI
        FindObjectOfType<DayMessageUI>().ShowDayMessage(GameManager.Instance.currentDay);

        //  Instanciar prisioneros para ese día
        FindObjectOfType<PrisonerManager>().SpawnPrisonersForDay(GameManager.Instance.currentDay);

        hud.SetActive(true);

        yield return StartCoroutine(FadeFromBlack());

        isSleeping = false;
    }

    IEnumerator ShowDayText(string text)
    {
        dayText.text = text;
        dayText.gameObject.SetActive(true);
        yield return new WaitForSeconds(dayTextDuration);
        dayText.gameObject.SetActive(false);
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
