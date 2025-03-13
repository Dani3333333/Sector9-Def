using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel; // Panel de selecci�n de extremidades
    public GameObject itemsPanel; // Panel donde se mostrar�n los objetos
    public TextMeshProUGUI itemsText; // Texto donde se listar�n los objetos
    public Button[] extremityButtons; // Lista de botones (brazos, torso, piernas)

    private int selectedButtonIndex = 0; // �ndice del bot�n seleccionado
    private bool isInspecting = false; // Si estamos inspeccionando

    void Update()
    {
        // Si presionamos "E" y no estamos inspeccionando, mostramos el panel
        if (Input.GetKeyDown(KeyCode.E) && !isInspecting)
        {
            OpenExtremityPanel();
        }

        // Si el panel est� activo, permitir la navegaci�n con teclado
        if (extremityPanel.activeSelf)
        {
            HandleKeyboardNavigation();
        }
    }

    // Funci�n para abrir el panel de selecci�n
    void OpenExtremityPanel()
    {
        extremityPanel.SetActive(true); // Mostrar panel
        itemsPanel.SetActive(false); // Ocultar panel de objetos
        selectedButtonIndex = 0; // Seleccionar el primer bot�n
        UpdateButtonSelection();
        isInspecting = true; // Ahora estamos inspeccionando
    }

    // Funci�n para manejar la navegaci�n con el teclado
    void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % extremityButtons.Length;
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + extremityButtons.Length) % extremityButtons.Length;
            UpdateButtonSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Si presionamos ENTER
        {
            extremityButtons[selectedButtonIndex].onClick.Invoke(); // Simular clic en el bot�n seleccionado
        }
    }

    // Funci�n para actualizar la selecci�n visual del bot�n
    void UpdateButtonSelection()
    {
        for (int i = 0; i < extremityButtons.Length; i++)
        {
            ColorBlock colors = extremityButtons[i].colors;
            colors.normalColor = (i == selectedButtonIndex) ? Color.yellow : Color.white;
            extremityButtons[i].colors = colors;
        }
    }

    // Funci�n para mostrar los objetos de una extremidad
    public void ShowItems(string extremity)
    {
        extremityPanel.SetActive(false); // Ocultar panel de selecci�n
        itemsPanel.SetActive(true); // Mostrar panel de objetos
        itemsText.text = $"Objetos en {extremity}:\n";

        string[] possibleItems = { "Cuchillo", "Mechero", "Revista", "M�vil", "Llaves" };
        int itemCount = Random.Range(2, 4); // 2 o 3 objetos aleatorios

        for (int i = 0; i < itemCount; i++)
        {
            string item = possibleItems[Random.Range(0, possibleItems.Length)];
            string color = (item == "Cuchillo" || item == "M�vil") ? "<color=red>" : "<color=green>";
            itemsText.text += $"{color}{item}</color>\n";
        }
    }
}
