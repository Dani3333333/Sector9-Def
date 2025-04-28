using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MerchantInteractionManager : MonoBehaviour
{
    public GameObject panelDialogo;
    public GameObject panelVenta;
    public GameObject panelFinal;
    public GameObject vendedorUI; // El panel que pone "VENDEDOR" en el mundo

    public TextMeshProUGUI dialogoTexto;
    public Button negociarButton;

    public Transform prisionerosContainer;
    public GameObject prisioneroButtonPrefab; // Prefab de botón para prisioneros

    public TextMeshProUGUI finalTexto;

    private bool nearVendedor = false;
    private int diaActual = 3; // Asumimos día 3

    private List<Preso> prisioneros = new List<Preso>();
    private int selectedIndex = 0;

    void Start()
    {
        panelDialogo.SetActive(false);
        panelVenta.SetActive(false);
        panelFinal.SetActive(false);

        // Rellenar prisioneros fake (puedes conectar con tus datos reales)
        prisioneros.Add(new Preso("Preso1", 80));
        prisioneros.Add(new Preso("Preso2", 60));
        prisioneros.Add(new Preso("Preso3", 30));
        prisioneros.Add(new Preso("Preso4", 90));

        negociarButton.onClick.AddListener(StartVenta);
    }

    void Update()
    {
        if (diaActual != 3) return;

        if (nearVendedor && Input.GetKeyDown(KeyCode.E))
        {
            OpenDialogo();
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
    }

    void OpenDialogo()
    {
        LogicaPersonaje1.isTrading = true;
        panelDialogo.SetActive(true);
        dialogoTexto.text = "Hola, soy el comerciante. Espero que hayas cuidado bien de los prisioneros. ¿Quieres negociar?";
    }

    void StartVenta()
    {
        panelDialogo.SetActive(false);
        panelVenta.SetActive(true);

        // Crear botones
        foreach (Transform child in prisionerosContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < prisioneros.Count; i++)
        {
            GameObject btnObj = Instantiate(prisioneroButtonPrefab, prisionerosContainer);
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            int happiness = prisioneros[i].happiness;
            int precio = happiness * 10; // Precio = felicidad x10 dólares

            btnText.text = $"{prisioneros[i].name} - Felicidad: {happiness}% - Precio: ${precio}";
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

        // Activar máquina de escribir
        TypewriterEffect typewriter = panelFinal.GetComponent<TypewriterEffect>();
        typewriter.StartTyping("¡Felicidades, has vendido tu primer prisionero!\n\nEsto ha sido todo del Vertical Slice, esperamos que lo hayas disfrutado.");

        // Opcionalmente, podrías poner pantalla en negro después de unos segundos
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == vendedorUI)
        {
            nearVendedor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == vendedorUI)
        {
            nearVendedor = false;
        }
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
