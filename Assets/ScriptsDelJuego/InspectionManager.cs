using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Necesario para seleccionar botones

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel;   // Panel de selección de extremidades
    public GameObject itemsPanel;       // Panel donde se muestra la lista de objetos
    public Text itemsText;              // Texto que muestra los objetos

    public Button armButton, torsoButton, legButton;  // Botones de extremidades

    private Dictionary<string, List<string>> extremitiesItems = new Dictionary<string, List<string>>();
    private List<Button> buttons = new List<Button>();
    private int selectedButtonIndex = 0;
    private bool isInspecting = false;
    private bool nearPrisoner = false; // Para detectar si estamos cerca del prisionero

    private List<string> possibleItems = new List<string>
    {
        "Cuchillo", "Mechero", "Revista", "Llave", "Celular", "Papel", "Navaja", "Bolígrafo", "Encendedor"
    };

    private HashSet<string> dangerousItems = new HashSet<string> { "Cuchillo", "Navaja", "Llave" };

    void Start()
    {
        // Guardamos los botones en una lista
        buttons.Add(armButton);
        buttons.Add(torsoButton);
        buttons.Add(legButton);

        // Vinculamos botones con sus funciones
        armButton.onClick.AddListener(() => ShowItems("Brazos"));
        torsoButton.onClick.AddListener(() => ShowItems("Torso"));
        legButton.onClick.AddListener(() => ShowItems("Piernas"));

        extremityPanel.SetActive(false); // Inicialmente está oculto
        itemsPanel.SetActive(false); // Inicialmente está oculto
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPrisoner && !isInspecting)
        {
            OpenExtremityPanel();
        }

        // Control de navegación por teclado cuando el panel de selección de extremidades está activo
        if (extremityPanel.activeSelf)
        {
            HandleKeyboardNavigation();
        }

        // Volver atrás o salir con ESC
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleEscape();
        }

        // Volver atrás con Backspace cuando estamos en el panel de objetos
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleBackspace();
        }
    }

    void OpenExtremityPanel()
    {
        GenerateRandomItems();  //  Genera nuevos objetos aleatorios en cada inspección
        extremityPanel.SetActive(true);  // Activamos el panel de extremidades
        LogicaPersonaje1.isInspecting = true; // Bloqueamos el movimiento del personaje

        //  Seleccionamos el primer botón para que las flechas funcionen
        selectedButtonIndex = 0;
        SelectButton(buttons[selectedButtonIndex]);
    }

    void ClosePanels()
    {
        extremityPanel.SetActive(false);  // Ocultamos el panel de extremidades
        itemsPanel.SetActive(false);     // Ocultamos el panel de objetos
        isInspecting = false;             // Indicamos que ya no estamos inspeccionando
        LogicaPersonaje1.isInspecting = false;  // Reactivamos el movimiento del personaje
    }

    void HandleEscape()
    {
        if (itemsPanel.activeSelf)
        {
            // Si estamos en la lista de objetos, volvemos al panel de extremidades
            itemsPanel.SetActive(false);
            extremityPanel.SetActive(true);
        }
        else if (extremityPanel.activeSelf)
        {
            // Si estamos en el panel de extremidades, cerramos todo el menú de inspección
            ClosePanels();
        }
    }

    void HandleBackspace()
    {
        if (itemsPanel.activeSelf)
        {
            // Si estamos en el panel de objetos, volvemos al panel de extremidades
            itemsPanel.SetActive(false);
            extremityPanel.SetActive(true);
        }
    }

    void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Count;
            SelectButton(buttons[selectedButtonIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttons.Count) % buttons.Count;
            SelectButton(buttons[selectedButtonIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Return para aceptar
        {
            buttons[selectedButtonIndex].onClick.Invoke(); // Invoca el evento del botón seleccionado
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
        int itemCount = Random.Range(2, 4);  // Generamos entre 2 y 3 objetos por extremidad

        for (int i = 0; i < itemCount; i++)
        {
            items.Add(possibleItems[Random.Range(0, possibleItems.Count)]);
        }

        return items;
    }

    public void ShowItems(string extremity)
    {
        itemsPanel.SetActive(true);    // Mostramos el panel de objetos
        extremityPanel.SetActive(false);  // Ocultamos el panel de extremidades
        itemsText.text = $"Objetos en {extremity}:\n";

        // Mostramos los objetos aleatorios en el texto
        foreach (var item in extremitiesItems[extremity])
        {
            itemsText.text += $"{item}\n";
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
