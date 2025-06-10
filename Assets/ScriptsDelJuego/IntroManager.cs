using System.Collections;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel;
    public TextMeshProUGUI introText;
    public float typeSpeed = 0.04f;

    [Header("Audio ambiente de tipeo")]
    public AudioSource typingAudioSource; // Asigna un AudioSource en el Inspector
    public AudioClip typingClip;          // Clip de sonido ambiente (tecleado suave)

    private string fullIntroText =
@"Has despertado en la <b>Nave del Sector 9</b>.

Tu misión es clara: <b>vigilar a los prisioneros</b>.
No solo evitarás que escapen… también deberás cuidar su bienestar.

Su <b>felicidad</b> es tu mayor activo.

Al tercer día el rey de los traficantes vendrá a <b>evaluar tus progresos</b>.
Y querrá comprar a alguno de ellos.

<b>Revísalos. Aliméntalos. Obsérvalos.</b>
Su valor crecerá… o se desmoronará contigo.

¿Estás preparado?

<b>Pulsa [ENTER] para comenzar tu primer día en el Sector 9...</b>";

    private bool finishedTyping = false;

    void Start()
    {
        introPanel.SetActive(true);

        // Inicia sonido de tipeo en bucle si está todo asignado
        if (typingAudioSource != null && typingClip != null)
        {
            typingAudioSource.clip = typingClip;
            typingAudioSource.loop = true;
            typingAudioSource.Play();
        }

        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (finishedTyping && Input.GetKeyDown(KeyCode.Return))
        {
            introPanel.SetActive(false);
            enabled = false;
        }
    }

    IEnumerator TypeText()
    {
        introText.text = "";
        foreach (char c in fullIntroText)
        {
            introText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        finishedTyping = true;

        // Detiene el sonido ambiente de tipeo
        if (typingAudioSource != null && typingAudioSource.isPlaying)
        {
            typingAudioSource.Stop();
        }
    }
}
