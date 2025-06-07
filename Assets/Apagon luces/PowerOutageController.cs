using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerOutageController : MonoBehaviour
{
    public static PowerOutageController Instance; // NUEVO

    public Light[] sceneLights;
    public SliderController[] prisoners;

    public GameObject fuseBox;
    public TextMeshProUGUI powerOutageMessage;
    public float minDelay = 10f;
    public float maxDelay = 20f;
    public bool isInspecting = false;

    private bool lightsOut = false;

    void Awake()
    {
        Instance = this;
    }

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

        powerOutageMessage.text = "";
        fuseBox.SetActive(false);
    }

    // NUEVO: Ocultar mensaje desde otro script
    public void HidePowerOutageMessage()
    {
        powerOutageMessage.text = "";
    }
}
