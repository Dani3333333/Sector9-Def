using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel;
    public GameObject itemsPanel;
    public Text itemsText;
    public Button armButton, torsoButton, legButton;
    public GameObject itemButtonPrefab;
    public Transform itemButtonContainer;
    public TextMeshProUGUI interactionPrompt;

    private Dictionary<PrisonerPatrol, Dictionary<string, List<string>>> prisonerItems = new Dictionary<PrisonerPatrol, Dictionary<string, List<string>>>();
    private List<Button> buttons = new List<Button>();
    private List<Button> itemButtons = new List<Button>();
    private int selectedButtonIndex = 0;
    private bool isInspecting = false;
    private bool nearPrisoner = false;
    private string currentExtremity = "";
    private PrisonerPatrol currentPrisoner;

    private List<string> possibleItems = new List<string>
    {
        // PELIGROSOS
        "Cuchillo", "Navaja", "Llave inglesa", "Destornillador", "Tijeras", "Cuerda", "Cable", "Batería", "Aguja afilada", "Barra metálica",

        // NO PELIGROSOS
        "Mechero", "Encendedor", "Chicle", "Goma de mascar", "Revista", "Papel", "Bolígrafo", "Espejo roto", "CD", "Tarjeta de crédito falsa",
        "Carta escondida", "Dinero falsificado", "Piedra pequeña", "Cinta adhesiva", "Grapadora", "Papel de fumar",
        "Comida escondida", "Cepillo de dientes", "Vaso de plástico", "Calcetines extra", "Papel higiénico",
        "Peine", "Pequeño espejo", "Móvil escondido", "Tabaco", "Barra de jabón", "Pastillas sospechosas"
    };

    private HashSet<string> dangerousItems = new HashSet<string>
    {
        "Cuchillo", "Navaja", "Llave inglesa", "Destornillador", "Tijeras", "Cuerda", "Cable", "Batería", "Aguja afilada", "Barra metálica"
    };

    void Start()
    {
        buttons.Add(armButton);
        buttons.Add(torsoButton);
        buttons.Add(legButton);

        armButton.onClick.AddListener(() => ShowItems("Brazos"));
        torsoButton.onClick.AddListener(() => ShowItems("Torso"));
        legButton.onClick.AddListener(() => ShowItems("Piernas"));

        extremityPanel.SetActive(false);
        itemsPanel.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPrisoner)
        {
            OpenExtremityPanel();
            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
        }

        if (extremityPanel.activeSelf)
            HandleKeyboardNavigation(buttons);
        else if (itemsPanel.activeSelf)
            HandleKeyboardNavigation(itemButtons);

        if (Input.GetKeyDown(KeyCode.H))
            RemoveSelectedItem();

        if (Input.GetKeyDown(KeyCode.Backspace))
            HandleEscape();
    }

    void OpenExtremityPanel()
    {
        if (currentPrisoner == null) return;

        if (!prisonerItems.ContainsKey(currentPrisoner))
        {
            prisonerItems[currentPrisoner] = GenerateRandomItemsPerPrisoner();
        }

        extremityPanel.SetActive(true);
        LogicaPersonaje1.isInspecting = true;
        isInspecting = true;
        selectedButtonIndex = 0;
        SelectButton(buttons[selectedButtonIndex]);
    }

    void ClosePanels()
    {
        extremityPanel.SetActive(false);
        itemsPanel.SetActive(false);
        isInspecting = false;
        LogicaPersonaje1.isInspecting = false;

        if (nearPrisoner && interactionPrompt != null)
        {
            interactionPrompt.text = "[E] Cachear";
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    void HandleEscape()
    {
        if (itemsPanel.activeSelf)
        {
            itemsPanel.SetActive(false);
            extremityPanel.SetActive(true);
            selectedButtonIndex = 0;
            SelectButton(buttons[selectedButtonIndex]);
        }
        else if (extremityPanel.activeSelf)
        {
            ClosePanels();
        }
    }

    void HandleKeyboardNavigation(List<Button> buttonList)
    {
        if (buttonList.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonList.Count;
            SelectButton(buttonList[selectedButtonIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonList.Count) % buttonList.Count;
            SelectButton(buttonList[selectedButtonIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            buttonList[selectedButtonIndex].onClick.Invoke();
        }
    }

    void SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    private Dictionary<string, List<string>> GenerateRandomItemsPerPrisoner()
    {
        Dictionary<string, List<string>> itemsPerExtremity = new Dictionary<string, List<string>>();
        itemsPerExtremity["Brazos"] = GetRandomItems();
        itemsPerExtremity["Torso"] = GetRandomItems();
        itemsPerExtremity["Piernas"] = GetRandomItems();
        return itemsPerExtremity;
    }

    private List<string> GetRandomItems()
    {
        List<string> items = new List<string>();
        List<string> availableItems = new List<string>(possibleItems);

        while (items.Count < 3 && availableItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            items.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex);
        }

        return items;
    }

    public void ShowItems(string extremity)
    {
        if (currentPrisoner == null || !prisonerItems.ContainsKey(currentPrisoner)) return;

        currentExtremity = extremity;
        itemsPanel.SetActive(true);
        extremityPanel.SetActive(false);
        GenerateButtons(currentPrisoner, extremity);
    }

    void GenerateButtons(PrisonerPatrol prisoner, string extremity)
    {
        foreach (Button btn in itemButtons)
        {
            Destroy(btn.gameObject);
        }
        itemButtons.Clear();

        List<string> items = prisonerItems[prisoner][extremity];

        foreach (string item in items)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = item;
            itemButtons.Add(button);
        }

        if (itemButtons.Count > 0)
        {
            selectedButtonIndex = 0;
            SelectButton(itemButtons[selectedButtonIndex]);
        }
    }

    void RemoveSelectedItem()
    {
        if (!itemsPanel.activeSelf || itemButtons.Count == 0) return;

        Button selectedButton = itemButtons[selectedButtonIndex];
        string itemName = selectedButton.GetComponentInChildren<Text>().text;

        // Si el objeto NO es peligroso, bajar la felicidad del prisionero inspeccionado
        if (!dangerousItems.Contains(itemName))
        {
            if (currentPrisoner != null)
            {
                SliderController sc = currentPrisoner.GetComponentInChildren<SliderController>();
                if (sc != null)
                {
                    sc.DecreaseHappiness(10f);
                }
            }
        }

        // Eliminar visualmente el botón del ítem
        itemButtons.RemoveAt(selectedButtonIndex);
        Destroy(selectedButton.gameObject);

        // Quitar el ítem de la lista de ese prisionero
        if (currentPrisoner != null && prisonerItems.ContainsKey(currentPrisoner))
        {
            prisonerItems[currentPrisoner][currentExtremity].Remove(itemName);
        }

        // Reajustar selección
        if (itemButtons.Count == 0)
        {
            selectedButtonIndex = 0;
        }
        else
        {
            selectedButtonIndex = Mathf.Clamp(selectedButtonIndex, 0, itemButtons.Count - 1);
            SelectButton(itemButtons[selectedButtonIndex]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prisionero"))
        {
            nearPrisoner = true;
            currentPrisoner = other.GetComponent<PrisonerPatrol>();

            if (!isInspecting && interactionPrompt != null)
            {
                interactionPrompt.text = "[E] Cachear";
                interactionPrompt.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Prisionero"))
        {
            if (other.GetComponent<PrisonerPatrol>() == currentPrisoner)
            {
                nearPrisoner = false;
                currentPrisoner = null;

                if (interactionPrompt != null)
                {
                    interactionPrompt.gameObject.SetActive(false);
                }
            }
        }
    }

    public void ReplenishAllPrisonerItems()
    {
        List<PrisonerPatrol> allPrisoners = new List<PrisonerPatrol>(prisonerItems.Keys);

        foreach (var prisoner in allPrisoners)
        {
            prisonerItems[prisoner] = GenerateRandomItemsPerPrisoner();
        }
    }
}
