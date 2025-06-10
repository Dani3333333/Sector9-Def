using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PowerOutageController : MonoBehaviour
{
    public static PowerOutageController Instance;

    [Header("Luces de la escena")]
    public Light[] sceneLights;

    [Header("Prisioneros")]
    public SliderController[] prisoners;

    [Header("Caja de fusibles")]
    public GameObject fuseBox;
    public GameObject fuseBoxLight; // Luz delante de la caja que se enciende en apag�n

    [Header("UI")]
    public TextMeshProUGUI powerOutageMessage;

    [Header("Temporizadores")]
    public float minDelay = 180f;    // 3 minutos m�nimo para el apag�n
    public float maxDelay = 180f;    // Puede dejar fijo a 180, o variar si quieres

    private float outageTimer;
    private bool lightsOut = false;

    [Header("P�rdida de felicidad")]
    public float happinessLossInterval = 5f;
    public float happinessLossAmount = 3f;
    private float happinessLossTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Si necesitas persistir entre escenas
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
            if (fuseBoxLight != null)
                fuseBoxLight.SetActive(false);

            outageTimer = Random.Range(minDelay, maxDelay);
        }
    }

    void Update()
    {
        // No iniciar apag�n si el jugador est� inspeccionando o durmiendo
        if (LogicaPersonaje1.isInspecting || IsSleeping())
        {
            return;
        }

        if (!lightsOut)
        {
            outageTimer -= Time.deltaTime;
            if (outageTimer <= 0f)
            {
                TriggerLightsOut();
                outageTimer = Random.Range(minDelay, maxDelay);
            }
        }
        else
        {
            // Reducir felicidad cada cierto intervalo
            happinessLossTimer -= Time.deltaTime;
            if (happinessLossTimer <= 0f)
            {
                happinessLossTimer = happinessLossInterval;

                foreach (SliderController sc in prisoners)
                {
                    if (sc.gameObject.activeInHierarchy)
                    {
                        sc.DecreaseHappiness(happinessLossAmount);
                    }
                }
            }
        }
    }

    void TriggerLightsOut()
    {
        lightsOut = true;

        foreach (Light l in sceneLights)
            l.enabled = false;

        powerOutageMessage.text = "�Apag�n! Pulsa E para arreglar la caja de cables.";
        fuseBox.SetActive(true);

        if (fuseBoxLight != null)
            fuseBoxLight.SetActive(true);

        happinessLossTimer = happinessLossInterval;  // reiniciar timer p�rdida felicidad
    }

    public void RegisterFuseBoxHit()
    {
        if (!lightsOut)
            return;

        // Aqu� debes implementar la l�gica para contar las pulsaciones E
        fuseHits++;

        if (fuseHits >= 5)
        {
            RestoreLights();
        }
    }

    private int fuseHits = 0;

    void RestoreLights()
    {
        lightsOut = false;
        fuseHits = 0;

        foreach (Light l in sceneLights)
            l.enabled = true;

        powerOutageMessage.text = "";
        fuseBox.SetActive(false);

        if (fuseBoxLight != null)
            fuseBoxLight.SetActive(false);

        foreach (SliderController sc in prisoners)
        {
            if (sc.gameObject.activeInHierarchy)
            {
                sc.IncreaseHappiness(10f);
            }
        }
    }

    public bool IsLightsOut()
    {
        return lightsOut;
    }

    // M�todo para comprobar si el jugador est� durmiendo (busca SleepManager activo y sleeping)
    private bool IsSleeping()
    {
        SleepManager sleepManager = FindObjectOfType<SleepManager>();
        if (sleepManager != null)
            return sleepManager.isSleeping;  // Aseg�rate que isSleeping sea p�blico o tiene getter
        return false;
    }

    public void HidePowerOutageMessage()
    {
        powerOutageMessage.text = "";
    }
}
