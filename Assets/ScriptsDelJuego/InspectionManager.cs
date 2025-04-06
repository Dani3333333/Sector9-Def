using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel;
    public GameObject itemsPanel;
    public Text itemsText;
    public Button armButton, torsoButton, legButton;
    public GameObject itemButtonPrefab;
    public Transform itemButtonContainer;

    private Dictionary<string, List<string>> extremitiesItems = new Dictionary<string, List<string>>();
    private List<Button> buttons = new List<Button>();
    private List<Button> itemButtons = new List<Button>();
    private int selectedButtonIndex = 0;
    private bool isInspecting = false;
    private bool nearPrisoner = false;
    private string currentExtremity = "";

    public SliderController sliderController;


    private List<string> possibleItems = new List<string>
    {
        "Cuchillo", "Mechero", "Revista", "Llave", "Celular", "Papel", "Navaja", "Bolígrafo", "Encendedor",
        "Destornillador", "Tijeras", "Cable", "Goma de mascar", "Cuerda", "Piedra", "Tarjeta de crédito",
        "Batería", "Chicle", "CD", "Espejo", "Llave inglesa", "Aguja", "Cinta adhesiva", "Grapadora"
    };

    private HashSet<string> dangerousItems = new HashSet<string>
    {
        "Cuchillo", "Navaja", "Llave", "Destornillador", "Tijeras", "Cable", "Cuerda", "Batería", "Llave inglesa", "Aguja"
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

        GenerateRandomItems(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPrisoner && !isInspecting)
        {
            OpenExtremityPanel();
        }

        if (extremityPanel.activeSelf)
        {
            HandleKeyboardNavigation(buttons);
        }
        else if (itemsPanel.activeSelf)
        {
            HandleKeyboardNavigation(itemButtons);
        }

        // Eliminar ítems con H
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveSelectedItem();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleEscape();
        }
    }

    void OpenExtremityPanel()
    {
  
        extremityPanel.SetActive(true);
        LogicaPersonaje1.isInspecting = true;
        selectedButtonIndex = 0;
        SelectButton(buttons[selectedButtonIndex]);
    }

    void ClosePanels()
    {
        extremityPanel.SetActive(false);
        itemsPanel.SetActive(false);
        isInspecting = false;
        LogicaPersonaje1.isInspecting = false;
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

    private void GenerateRandomItems()
    {
        extremitiesItems["Brazos"] = GetRandomItems();
        extremitiesItems["Torso"] = GetRandomItems();
        extremitiesItems["Piernas"] = GetRandomItems();
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
        currentExtremity = extremity;
        itemsPanel.SetActive(true);
        extremityPanel.SetActive(false);
        GenerateButtons(extremity);
    }

    void GenerateButtons(string extremity)
    {
        // Limpiar botones anteriores
        foreach (Button btn in itemButtons)
        {
            Destroy(btn.gameObject);
        }
        itemButtons.Clear();

        List<string> items = extremitiesItems[extremity];

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

        if (!dangerousItems.Contains(itemName))
        {
            if (sliderController != null)
            {
                sliderController.DecreaseHappiness(10f);
            }
        }

        // Eliminar de lista de botones y destruir objeto
        itemButtons.RemoveAt(selectedButtonIndex);
        Destroy(selectedButton.gameObject);

        // Eliminar del diccionario (lista de ítems)
        if (extremitiesItems.ContainsKey(currentExtremity))
        {
            extremitiesItems[currentExtremity].Remove(itemName);
        }

        // Ajustar índice
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Prisionero"))
        {
            nearPrisoner = false;
        }
    }
}
