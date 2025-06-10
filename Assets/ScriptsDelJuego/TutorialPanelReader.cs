using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelReader : MonoBehaviour
{
    public GameObject detailPanel; // Panel con la info detallada (padre de los paneles de tareas)
    public GameObject promptUI;    // Texto tipo "[E] Leer detalladamente"

    [HideInInspector]
    public bool panelOpen = false;

    private bool playerInZone = false;

    void Start()
    {
        if (detailPanel != null)
        {
            detailPanel.SetActive(false);

            // Desactivar todos los toggles y paneles hijos al iniciar
            Toggle[] toggles = detailPanel.GetComponentsInChildren<Toggle>(true);
            foreach (var toggle in toggles)
            {
                toggle.isOn = false;
            }

            foreach (Transform child in detailPanel.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentDay >= 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panelOpen = !panelOpen;
            detailPanel.SetActive(panelOpen);

            if (promptUI != null)
                promptUI.SetActive(!panelOpen);

            LogicaPersonaje1.isInspecting = panelOpen;
        }
    }

    public void CloseDetailPanel()
    {
        Debug.Log("CloseDetailPanel llamado");

        panelOpen = false;

        if (detailPanel != null)
        {
            foreach (Transform child in detailPanel.transform)
            {
                Debug.Log("Desactivando panel hijo: " + child.name);
                child.gameObject.SetActive(false);
            }

            detailPanel.SetActive(false);
            Debug.Log("detailPanel desactivado");
        }

        if (promptUI != null && playerInZone)
            promptUI.SetActive(true);

        LogicaPersonaje1.isInspecting = false;

        if (TutorialTaskNavigator.Instance != null)
        {
            TutorialTaskNavigator.Instance.ResetToFirstPanel();
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
