using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MerchantInteractionManager : MonoBehaviour
{
    public GameObject panelDialogo;
    public GameObject panelVenta;
    public GameObject panelFinal;
    public GameObject vendedorUI;
    public GameObject interactPrompt;

    public TextMeshProUGUI dialogoTexto;
    public Button negociarButton;

    public Transform prisionerosContainer;
    public GameObject prisioneroButtonPrefab;

    public TextMeshProUGUI finalTexto;

    private bool nearVendedor = false;
    private List<Preso> prisioneros = new List<Preso>();

    public SliderController prisoner1;
    public SliderController prisoner2;
    public SliderController prisoner3;
    public SliderController prisoner4;


    private int selectedIndex = 0;
    private CanvasGroup panelFinalCanvasGroup; // << Añadido para fade

    public Button salirButton; // <<<< Añadido


    void Start()
    {
        panelDialogo.SetActive(false);
        panelVenta.SetActive(false);
        panelFinal.SetActive(false);

        if (vendedorUI != null)
            vendedorUI.SetActive(false);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        prisioneros.Add(new Preso("Preso1", 80));
        prisioneros.Add(new Preso("Preso2", 60));
        prisioneros.Add(new Preso("Preso3", 30));
        prisioneros.Add(new Preso("Preso4", 90));

        negociarButton.onClick.AddListener(StartVenta);

        // Preparar fade
        panelFinalCanvasGroup = panelFinal.GetComponent<CanvasGroup>();
        if (panelFinalCanvasGroup == null)
            panelFinalCanvasGroup = panelFinal.AddComponent<CanvasGroup>();
        panelFinalCanvasGroup.alpha = 0;
    }

    void Update()
    {
        if (GameManager.Instance.currentDay == 3)
        {
            if (vendedorUI != null && !vendedorUI.activeSelf)
                vendedorUI.SetActive(true);

            if (nearVendedor)
            {
                if (interactPrompt != null && !interactPrompt.activeSelf)
                    interactPrompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenDialogo();
                    interactPrompt.SetActive(false);
                }
            }
            else
            {
                if (interactPrompt != null && interactPrompt.activeSelf)
                    interactPrompt.SetActive(false);
            }

            if (panelVenta.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectedIndex = (selectedIndex - 1 + prisionerosContainer.childCount) % prisionerosContainer.childCount;
                    HighlightButton(selectedIndex);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectedIndex = (selectedIndex + 1) % prisionerosContainer.childCount;
                    HighlightButton(selectedIndex);
                }
                if (Input.GetKeyDown(KeyCode.V))
                {
                    SellPrisionero(selectedIndex);
                }
            }

            if (panelFinal.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                SalirDelJuego();
            }
        }
        else
        {
            if (vendedorUI != null && vendedorUI.activeSelf)
                vendedorUI.SetActive(false);

            if (interactPrompt != null && interactPrompt.activeSelf)
                interactPrompt.SetActive(false);
        }
    }


    void OpenDialogo()
    {
        LogicaPersonaje1.isTrading = true;
        panelDialogo.SetActive(true);
        dialogoTexto.text = "Si quieres ganar unos buenos dólares... pulsa [Enter] para negociar conmigo.";

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(negociarButton.gameObject);
    }

    void StartVenta()
    {
        panelDialogo.SetActive(false);
        panelVenta.SetActive(true);

        foreach (Transform child in prisionerosContainer)
            Destroy(child.gameObject);

        prisioneros.Clear();

        // Solo añade los prisioneros que existen según el día
        int day = GameManager.Instance.currentDay;

        if (day >= 0)
            prisioneros.Add(new Preso("Prisoner 1", (int)prisoner1.GetCurrentHappiness()));
        if (day >= 1)
            prisioneros.Add(new Preso("Prisoner 2", (int)prisoner2.GetCurrentHappiness()));
        if (day >= 2)
            prisioneros.Add(new Preso("Prisoner 3", (int)prisoner3.GetCurrentHappiness()));
        if (day >= 3)
            prisioneros.Add(new Preso("Prisoner 4", (int)prisoner4.GetCurrentHappiness()));

        for (int i = 0; i < prisioneros.Count; i++)
        {
            GameObject btnObj = Instantiate(prisioneroButtonPrefab, prisionerosContainer);
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            int happiness = prisioneros[i].happiness;
            int precio = happiness * 10;

            btnText.text = $"{prisioneros[i].name}\tFelicidad: {happiness}%\t|\tPrecio: ${precio}";
        }

        HighlightButton(0);
    }


    void HighlightButton(int index)
    {
        for (int i = 0; i < prisionerosContainer.childCount; i++)
        {
            Button btn = prisionerosContainer.GetChild(i).GetComponent<Button>();
            ColorBlock colors = btn.colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white;
            btn.colors = colors;
        }
    }

    void SellPrisionero(int index)
    {
        panelVenta.SetActive(false);
        panelFinal.SetActive(true);

        StartCoroutine(FadeInPanelFinal());

        TypewriterEffect typewriter = panelFinal.GetComponentInChildren<TypewriterEffect>();
        if (typewriter != null)
        {
            typewriter.StartTyping("Felicidades, has vendido tu primer prisionero!\n\nEsto ha sido todo del Vertical Slice.");
        }
        else
        {
            Debug.LogWarning("No se encontró TypewriterEffect en PanelFinal.");
        }

        if (salirButton != null)
        {
            salirButton.gameObject.SetActive(true);
            salirButton.onClick.RemoveAllListeners(); // Limpia por si acaso
            salirButton.onClick.AddListener(SalirDelJuego);
        }
    }


    IEnumerator FadeInPanelFinal()
    {
        float duration = 1.0f; // 1 segundo de fade
        float elapsed = 0f;
        panelFinalCanvasGroup.alpha = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelFinalCanvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
    }

    void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            nearVendedor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            nearVendedor = false;
    }

    class Preso
    {
        public string name;
        public int happiness;

        public Preso(string n, int h)
        {
            name = n;
            happiness = h;
        }
    }
}
