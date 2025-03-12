using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InspectionManager : MonoBehaviour
{
    public GameObject extremityPanel; // Panel de selección de extremidades
    public GameObject itemsPanel; // Panel donde se mostrará la lista de objetos
    public Text itemsText; // Texto que mostrará los objetos

    public Button armButton, torsoButton, legButton; // Botones de extremidades

    private Dictionary<string, List<string>> extremitiesItems = new Dictionary<string, List<string>>();

    private List<string> possibleItems = new List<string>
    {
        "Cuchillo", "Mechero", "Revista", "Llave", "Celular", "Papel", "Navaja", "Bolígrafo", "Encendedor"
    };

    private HashSet<string> dangerousItems = new HashSet<string> { "Cuchillo", "Navaja", "Llave" };

    void Start()
    {
        GenerateRandomItems();

        // Vinculamos los botones con sus funciones
        armButton.onClick.AddListener(() => ShowItems("Brazos"));
        torsoButton.onClick.AddListener(() => ShowItems("Torso"));
        legButton.onClick.AddListener(() => ShowItems("Piernas"));
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
}

