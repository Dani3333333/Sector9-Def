using UnityEngine;
using UnityEngine.UI; // <- Añadimos esto para usar Toggle

public class TutorialPanelReader : MonoBehaviour
{
    public GameObject detailPanel; // Panel con la info detallada
    public GameObject promptUI;    // Texto tipo "[E] Leer detalladamente"

    private bool playerInZone = false;
    private bool panelOpen = false;

    void Start()
    {
        if (detailPanel != null)
        {
            detailPanel.SetActive(false);

            //  Desactivar todos los toggles dentro del panel al iniciar
            Toggle[] toggles = detailPanel.GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles)
            {
                toggle.isOn = false;
            }
        }

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        // Primero, comprobar si estamos en el día 1 o superior
        if (GameManager.Instance != null && GameManager.Instance.currentDay >= 1)
        {
            gameObject.SetActive(false); // O Destroy(gameObject); si prefieres eliminarlo
        }

        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panelOpen = !panelOpen;
            detailPanel.SetActive(panelOpen);

            if (promptUI != null)
                promptUI.SetActive(!panelOpen);

            LogicaPersonaje1.isInspecting = panelOpen;
        }

        if (panelOpen && Input.GetKeyDown(KeyCode.Backspace))
        {
            panelOpen = false;
            detailPanel.SetActive(false);

            if (promptUI != null && playerInZone)
                promptUI.SetActive(true);

            LogicaPersonaje1.isInspecting = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;

            if (promptUI != null && !panelOpen)
                promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            panelOpen = false;

            if (promptUI != null)
                promptUI.SetActive(false);

            if (detailPanel != null)
                detailPanel.SetActive(false);

            LogicaPersonaje1.isInspecting = false;
        }
    }
}
