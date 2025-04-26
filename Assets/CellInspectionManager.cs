using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CellInspectionManager : MonoBehaviour
{
    public GameObject itemsPanel;
    public Text itemsText;
    public GameObject itemButtonPrefab;
    public Transform itemButtonContainer;

    public TextMeshProUGUI interactionPrompt;

    private Dictionary<int, Dictionary<string, List<string>>> cellItems = new Dictionary<int, Dictionary<string, List<string>>>();
    private List<Button> itemButtons = new List<Button>();
    private int selectedButtonIndex = 0;
    private bool isInspecting = false;
    private bool nearInspectionZone = false;
    private CellInspectionZone currentZone;

    public SliderController[] sliderControllers; // 1 sliderController por prisionero

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
        itemsPanel.SetActive(false);
        if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearInspectionZone && !isInspecting)
        {
            OpenItemsPanel();
            if (interactionPrompt != null) interactionPrompt.gameObject.SetActive(false);
        }

        if (itemsPanel.activeSelf)
        {
            HandleKeyboardNavigation(itemButtons);

            if (Input.GetKeyDown(KeyCode.H))
            {
                RemoveSelectedItem();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                CloseItemsPanel();
            }
        }
    }

    void OpenItemsPanel()
    {
        if (currentZone == null) return;

        isInspecting = true;
        itemsPanel.SetActive(true);
        GenerateButtons();
    }

    void CloseItemsPanel()
    {
        itemsPanel.SetActive(false);
        isInspecting = false;
        if (interactionPrompt != null && nearInspectionZone)
        {
            interactionPrompt.text = "[E] Inspeccionar " + currentZone.zoneType;
            interactionPrompt.gameObject.SetActive(true);
        }
    }

    void GenerateButtons()
    {
        foreach (Button btn in itemButtons)
        {
            Destroy(btn.gameObject);
        }
        itemButtons.Clear();

        List<string> items = GetItemsList(currentZone.cellId, currentZone.zoneType);

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

    List<string> GetItemsList(int cellId, string zoneType)
    {
        if (!cellItems.ContainsKey(cellId))
            cellItems[cellId] = new Dictionary<string, List<string>>();

        if (!cellItems[cellId].ContainsKey(zoneType))
            cellItems[cellId][zoneType] = GetRandomItems();

        return cellItems[cellId][zoneType];
    }

    List<string> GetRandomItems()
    {
        List<string> items = new List<string>();
        List<string> availableItems = new List<string>(possibleItems);

        while (items.Count < 4 && availableItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            items.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex);
        }

        return items;
    }

    void RemoveSelectedItem()
    {
        if (itemButtons.Count == 0) return;

        Button selectedButton = itemButtons[selectedButtonIndex];
        string itemName = selectedButton.GetComponentInChildren<Text>().text;

        if (!dangerousItems.Contains(itemName))
        {
            if (sliderControllers != null && currentZone != null)
            {
                sliderControllers[currentZone.cellId - 1].DecreaseHappiness(10f);
            }
        }

        itemButtons.RemoveAt(selectedButtonIndex);
        Destroy(selectedButton.gameObject);

        if (cellItems.ContainsKey(currentZone.cellId) && cellItems[currentZone.cellId].ContainsKey(currentZone.zoneType))
        {
            cellItems[currentZone.cellId][currentZone.zoneType].Remove(itemName);
        }

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

    public void ReplenishItems()
    {
        foreach (var cell in cellItems)
        {
            foreach (var zone in cell.Value)
            {
                List<string> currentItems = zone.Value;
                List<string> availableItems = new List<string>(possibleItems);

                foreach (string existingItem in currentItems)
                {
                    availableItems.Remove(existingItem);
                }

                while (currentItems.Count < 4 && availableItems.Count > 0)
                {
                    int randomIndex = Random.Range(0, availableItems.Count);
                    currentItems.Add(availableItems[randomIndex]);
                    availableItems.RemoveAt(randomIndex);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CellInspectionZone zone = other.GetComponent<CellInspectionZone>();
        if (zone != null)
        {
            nearInspectionZone = true;
            currentZone = zone;

            if (!isInspecting && interactionPrompt != null)
            {
                interactionPrompt.text = "[E] Inspeccionar " + zone.zoneType;
                interactionPrompt.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CellInspectionZone zone = other.GetComponent<CellInspectionZone>();
        if (zone != null && zone == currentZone)
        {
            nearInspectionZone = false;
            currentZone = null;

            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}

