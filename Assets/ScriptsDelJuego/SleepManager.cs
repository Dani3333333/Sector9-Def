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
            // Verificar si puede dormir (hasta el Día 3)
            if (GameManager.Instance.CanSleep())
            {
                StartCoroutine(SleepWithFadeAndVideoRoutine());
            }
            else
            {
                Debug.Log("Ya se han completado los 3 días.");
            }
        }
    }

    IEnumerator SleepWithFadeAndVideoRoutine()
    {
        isSleeping = true;
        hud.SetActive(false);  // Desactivar el HUD
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

        gameClock.ResetClock();  // Reiniciar el reloj

        // Avanzar al siguiente día
        GameManager.Instance.NextDay();

        FindObjectOfType<InspectionManager>().ReplenishItems();


        // Mostrar texto del día y hacer fade del fondo negro
        yield return StartCoroutine(ShowDayTextAndFadeFromBlack("Día " + GameManager.Instance.currentDay));

        FindObjectOfType<DayMessageUI>().ShowDayMessage(GameManager.Instance.currentDay);
        FindObjectOfType<PrisonerManager>().SpawnPrisonersForDay(GameManager.Instance.currentDay);

        isSleeping = false;
        canSleep = true;

    }

    IEnumerator ShowDayTextAndFadeFromBlack(string text)
    {
        dayText.text = text;
        dayText.alpha = 0f;
        dayText.transform.localScale = Vector3.zero;
        dayText.gameObject.SetActive(true);

        // Fade in con escalado
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / 0.3f);
            float scale = Mathf.SmoothStep(0f, 1.1f, t / 0.3f);
            dayText.alpha = alpha;
            dayText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        dayText.transform.localScale = Vector3.one * 1.1f;
        yield return new WaitForSeconds(0.05f);
        dayText.transform.localScale = Vector3.one;

        // Esperar el resto de tiempo antes de desvanecer
        yield return new WaitForSeconds(dayTextDuration - fadeDuration);

        // Fade out del panel y el texto al mismo tiempo
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            dayText.alpha = alpha;
            yield return null;
        }

        fadePanel.color = new Color(0f, 0f, 0f, 0f);
        dayText.gameObject.SetActive(false);

        isSleeping = false;
        // Activar el HUD después de mostrar el día
        hud.SetActive(true);
        canSleep = true;

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
        fadePanel.color = new Color(0f, 0f, 0f, 1f);
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
