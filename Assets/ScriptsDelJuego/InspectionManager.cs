using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Necesario para seleccionar botones

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel;   // Panel de selecci�n de extremidades
    public GameObject itemsPanel;       // Panel donde se muestra la lista de objetos
    public Text itemsText;              // Texto que muestra los objetos

    public Button armButton, torsoButton, legButton;  // Botones de extremidades
    public GameObject itemButtonPrefab;  // Prefab del bot�n para los objetos
    public Transform itemButtonContainer;  // Contenedor donde se instanciar�n los botones

    private Dictionary<string, List<string>> extremitiesItems = new Dictionary<string, List<string>>();
    private List<Button> buttons = new List<Button>();
    private int selectedButtonIndex = 0;
    private bool isInspecting = false;
    private bool nearPrisoner = false; // Para detectar si estamos cerca del prisionero

    

    private List<string> possibleItems = new List<string> //NO PELIGROSOS
    {
        "Cuchillo", "Mechero", "Revista", "Llave", "Celular", "Papel", "Navaja", "Bol�grafo", "Encendedor",
    "Destornillador", "Tijeras", "Cable", "Goma de mascar", "Cuerda", "Piedra", "Tarjeta de cr�dito",
    "Bater�a", "Chicle", "CD", "Espejo", "Llave inglesa", "Aguja", "Cinta adhesiva", "Grapadora"
    };

    private HashSet<string> dangerousItems = new HashSet<string> { "Cuchillo", "Navaja", "Llave", "Destornillador", "Tijeras", "Cable", "Cuerda",
    "Bater�a", "Llave inglesa", "Aguja" };

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

        extremityPanel.SetActive(false); // Inicialmente est� oculto
        itemsPanel.SetActive(false); // Inicialmente est� oculto
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && nearPrisoner && !isInspecting)
        {
            OpenExtremityPanel();
        }

        // Control de navegaci�n por teclado cuando el panel de selecci�n de extremidades est� activo
        if (extremityPanel.activeSelf)
        {
            HandleKeyboardNavigation();
        }

        // Volver atr�s o salir con ESC
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleEscape();
        }

        // Volver atr�s con Backspace cuando estamos en el panel de objetos
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HandleBackspace();
        }
    }

    void OpenExtremityPanel()
    {
        GenerateRandomItems();  //  Genera nuevos objetos aleatorios en cada inspecci�n
        extremityPanel.SetActive(true);  // Activamos el panel de extremidades
        LogicaPersonaje1.isInspecting = true; // Bloqueamos el movimiento del personaje

        //  Seleccionamos el primer bot�n para que las flechas funcionen
        selectedButtonIndex = 0;
        SelectButton(buttons[selectedButtonIndex]);

        // Generar botones en el panel de objetos
        GenerateButtons("Brazos");  // Cambiar seg�n la extremidad seleccionada
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
            // Si estamos en el panel de extremidades, cerramos todo el men� de inspecci�n
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
            buttons[selectedButtonIndex].onClick.Invoke(); // Invoca el evento del bot�n seleccionado
        }
        else if (Input.GetKeyDown(KeyCode.H)) // Eliminar el objeto
        {
            if (itemButtonContainer.childCount > 0)
            {
                // Obtener el primer bot�n
                Transform firstButton = itemButtonContainer.GetChild(0);
                Button button = firstButton.GetComponent<Button>();
                Text buttonText = firstButton.GetComponentInChildren<Text>();
                string itemName = buttonText.text;

                // Llamar a la funci�n de eliminar
                RemoveItem(button, itemName);
            }
        }
    }

    private void RemoveItem(Button button, string item)
    {
        // Eliminar el objeto de la lista de items
        string extremity = buttons[selectedButtonIndex].name; // Obtener el extremity actual
        extremitiesItems[extremity].Remove(item);

        // Eliminar el bot�n visualmente
        Destroy(button.gameObject);
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
        int itemCount = Random.Range(2, 4); // Generamos entre 2 y 3 objetos por extremidad

        // Copias de las listas para evitar repeticiones
        List<string> availableSafeItems = new List<string>(possibleItems);
        List<string> availableDangerousItems = new List<string>(dangerousItems);

        // Asegurar que solo 1 o 2 sean peligrosos si itemCount == 3
        int maxDangerous = (itemCount == 3) ? Random.Range(1, 3) : 1;
        int dangerousAdded = 0;

        for (int i = 0; i < itemCount; i++)
        {
            if (dangerousAdded < maxDangerous && availableDangerousItems.Count > 0)
            {
                // Seleccionamos un objeto peligroso
                int randomIndex = Random.Range(0, availableDangerousItems.Count);
                string selectedItem = availableDangerousItems[randomIndex];
                items.Add(selectedItem);
                availableDangerousItems.RemoveAt(randomIndex); // Lo eliminamos para evitar repetici�n
                dangerousAdded++;
            }
            else if (availableSafeItems.Count > 0)
            {
                // Seleccionamos un objeto seguro
                List<string> safeItemsOnly = availableSafeItems.FindAll(item => !dangerousItems.Contains(item));
                int randomIndex = Random.Range(0, safeItemsOnly.Count);
                string selectedItem = safeItemsOnly[randomIndex];
                items.Add(selectedItem);
                availableSafeItems.Remove(selectedItem); // Lo eliminamos para evitar repetici�n
            }
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

    private void GenerateButtons(string extremity)
    {
        // Limpiar los botones anteriores
        foreach (Transform child in itemButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<string> items = extremitiesItems[extremity];
        foreach (string item in items)
        {
            // Crear un bot�n
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();

            // Establecer el nombre del objeto en el texto del bot�n
            buttonText.text = item;

            // Asignar una acci�n al presionar el bot�n
            button.onClick.AddListener(() => RemoveItem(button, item));
        }
    }

}
