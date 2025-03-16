using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel; // Panel de selección de extremidades
    public GameObject itemsPanel; // Panel donde se mostrará la lista de objetos
    public Text itemsText; // Texto que mostrará los objetos

    public Button armButton, torsoButton, legButton; // Botones de extremidades
    private Button[] extremityButtons;
    private int selectedButtonIndex = 0;

    private Dictionary<string, List<string>> extremitiesItems = new Dictionary<string, List<string>>();

    private List<string> possibleItems = new List<string>
    {
        "Cuchillo", "Mechero", "Revista", "Llave", "Celular", "Papel", "Navaja", "Bolígrafo", "Encendedor"
    };

    private HashSet<string> dangerousItems = new HashSet<string> { "Cuchillo", "Navaja", "Llave" };

    private bool nearPrisoner = false; // Para saber si el jugador está cerca del prisionero
    private bool isInspecting = false; // Para saber si está inspeccionando

    void Start()
    {
        GenerateRandomItems();

        // Vinculamos los botones con sus funciones
        armButton.onClick.AddListener(() => ShowItems("Brazos"));
        torsoButton.onClick.AddListener(() => ShowItems("Torso"));
        legButton.onClick.AddListener(() => ShowItems("Piernas"));

        // Guardamos los botones en un array para movernos con las flechas
        extremityButtons = new Button[] { armButton, torsoButton, legButton };

        // Asegurar que los paneles comiencen desactivados
        extremityPanel.SetActive(false);
        itemsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPrisoner && !isInspecting)
        {
            OpenExtremityPanel();
        }

        if (extremityPanel.activeSelf)
        {
            HandleKeyboardNavigation();
        }

        // Si presionas Esc, se cierra todo
        if (Input.GetKeyDown(KeyCode.Escape) && isInspecting)
        {
            ClosePanels();
        }
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
        int itemCount = Random.Range(2, 4); // Entre 2 y 3 objetos

        for (int i = 0; i < itemCount; i++)
        {
            items.Add(possibleItems[Random.Range(0, possibleItems.Count)]);
        }

        return items;
    }

    public void ShowItems(string extremity)
    {
        itemsPanel.SetActive(true);
        extremityPanel.SetActive(false); // Ocultamos el panel de extremidades
        itemsText.text = $"Objetos en {extremity}:\n";

        foreach (var item in extremitiesItems[extremity])
        {
            string color = dangerousItems.Contains(item) ? "<color=red>" : "<color=green>";
            itemsText.text += $"{color}{item}</color>\n";
        }
    }

    void OpenExtremityPanel()
    {
        extremityPanel.SetActive(true);
        LogicaPersonaje1.isInspecting = true; // 🔹 Bloqueamos movimiento
    }

    void ClosePanels()
    {
        extremityPanel.SetActive(false);
        itemsPanel.SetActive(false);
        LogicaPersonaje1.isInspecting = false; // 🔹 Reactivamos movimiento
    }



    void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % extremityButtons.Length;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + extremityButtons.Length) % extremityButtons.Length;
        }

        // Resaltar el botón seleccionado
        EventSystem.current.SetSelectedGameObject(extremityButtons[selectedButtonIndex].gameObject);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            extremityButtons[selectedButtonIndex].onClick.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prisionero"))
        {
            nearPrisoner = true; // Estamos cerca del prisionero
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Prisionero"))
        {
            nearPrisoner = false; // Salimos del área del prisionero
        }
    }
}
