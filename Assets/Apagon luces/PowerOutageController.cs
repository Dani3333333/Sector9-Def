using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PowerOutageController : MonoBehaviour
{
    public Light[] sceneLights;
    public SliderController[] prisoners;

    public GameObject fuseBox;                 // El objeto de la caja de fusibles
    public TextMeshProUGUI powerOutageMessage;
    public float minDelay = 120f;
    public float maxDelay = 300f;
    public bool isInspecting = false;

    private bool lightsOut = false;

    void Start()
    {
        powerOutageMessage.text = "";
        fuseBox.SetActive(false);
        StartCoroutine(TriggerPowerOutageRoutine());
    }

    IEnumerator TriggerPowerOutageRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (!isInspecting && !lightsOut)
            {
                TriggerLightsOut();
            }
        }
    }

    void TriggerLightsOut()
    {
        lightsOut = true;

        foreach (Light light in sceneLights)
        {
            light.enabled = false;
        }

        // Mostrar mensaje y caja de cambios
        powerOutageMessage.text = "¡Apagón! Ayuda a arreglar los cables o los prisioneros perderán felicidad.";
        fuseBox.SetActive(true);

        StartCoroutine(LoseHappinessWhileDark());
    }

    IEnumerator LoseHappinessWhileDark()
    {
        while (lightsOut)
        {
            foreach (SliderController sc in prisoners)
            {
                if (sc.gameObject.activeInHierarchy)
                {
                    sc.DecreaseHappiness(5f);
                }
            }
            yield return new WaitForSeconds(10f);
        }
    }

    public void RestoreLights()
    {
        lightsOut = false;

        foreach (Light light in sceneLights)
        {
            light.enabled = true;
        }

        foreach (SliderController sc in prisoners)
        {
            if (sc.gameObject.activeInHierarchy)
            {
                sc.IncreaseHappiness(10f);
            }
        }

        // Limpiar UI
        powerOutageMessage.text = "";
        fuseBox.SetActive(false);
    }
}
