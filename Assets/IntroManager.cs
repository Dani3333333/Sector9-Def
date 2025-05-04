using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel;
    public TextMeshProUGUI introText;
    public GameClock gameClock;
    public float typeSpeed = 0.04f;

    private string fullIntroText =
@"Has despertado en la <b>Nave del Sector 9</b>.

Tu misi�n es clara: <b>vigilar a los prisioneros</b>.
No solo evitar�s que escapen� tambi�n deber�s cuidar su bienestar.

Su <b>felicidad</b> es tu mayor activo.

Al tercer d�a, el comandante supremo vendr� a <b>evaluar</b> tus progresos.
Y querr� comprar a alguno de ellos.

<b>Cu�dalos. Alim�ntalos. Obs�rvalos.</b>
Su valor crecer� o se desmoronar� contigo.

�Est�s preparado?

<b>Pulsa [ENTER] para comenzar tu primer d�a en el Sector 9...</b>";

    private bool finishedTyping = false;

    void Start()
    {
        introPanel.SetActive(true);
        gameClock.enabled = false;
        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (finishedTyping && Input.GetKeyDown(KeyCode.Return))
        {
            introPanel.SetActive(false);
            gameClock.enabled = true;
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
    }
}

