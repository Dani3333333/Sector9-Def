using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PowerOutageController : MonoBehaviour
{
    public static PowerOutageController Instance;

    public Light[] sceneLights;
    public SliderController[] prisoners;
    public GameObject fuseBox;
    public TextMeshProUGUI powerOutageMessage;
    public float minDelay = 10f;
    public float maxDelay = 20f;

    private bool lightsOut = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir si aún necesitas entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Sector 9")
        {
            powerOutageMessage.text = "";
            fuseBox.SetActive(false);
            StartCoroutine(TriggerPowerOutageRoutine());
        }
    }

    System.Collections.IEnumerator TriggerPowerOutageRoutine()
    {
        yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        TriggerLightsOut();
    }

    void TriggerLightsOut()
    {
        lightsOut = true;

        foreach (Light l in sceneLights)
            l.enabled = false;

        powerOutageMessage.text = "¡Apagón! Pulsa E para arreglar los cables.";
        fuseBox.SetActive(true);
    }

    public void RestoreLights()
    {
        lightsOut = false;

        foreach (Light l in sceneLights)
            l.enabled = true;

        powerOutageMessage.text = "";
        fuseBox.SetActive(false);

        // Restaurar felicidad a todos los prisioneros
        foreach (SliderController sc in prisoners)
        {
            if (sc.gameObject.activeInHierarchy)
            {
                sc.IncreaseHappiness(10f);
            }
        }
    }

    public void HidePowerOutageMessage()
    {
        powerOutageMessage.text = "";
    }

}
