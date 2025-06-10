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

Tu misi�n es clara: <b>vigilar a los prisioneros</b>.
No solo evitar�s que escapen� tambi�n deber�s cuidar su bienestar.

Su <b>felicidad</b> es tu mayor activo.

Al tercer d�a el rey de los traficantes vendr� a <b>evaluar tus progresos</b>.
Y querr� comprar a alguno de ellos.

<b>Rev�salos. Alim�ntalos. Obs�rvalos.</b>
Su valor crecer� o se desmoronar� contigo.

�Est�s preparado?

<b>Pulsa [ENTER] para comenzar tu primer d�a en el Sector 9...</b>";

    private bool finishedTyping = false;

    void Start()
    {
        introPanel.SetActive(true);

        // Inicia sonido de tipeo en bucle si est� todo asignado
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
