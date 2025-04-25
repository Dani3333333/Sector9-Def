using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelReader : MonoBehaviour
{
    public GameObject detailPanel; // Panel con la info detallada
    public GameObject promptUI;    // Texto tipo "[E] Leer detalladamente"

    private bool playerInZone = false;
    private bool panelOpen = false;

    void Start()
    {
        if (detailPanel != null)
            detailPanel.SetActive(false);

        if (promptUI != null)
            promptUI.SetActive(false);
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            panelOpen = !panelOpen;
            detailPanel.SetActive(panelOpen);

            if (promptUI != null)
                promptUI.SetActive(!panelOpen);
        }

        if (panelOpen && Input.GetKeyDown(KeyCode.Backspace))
        {
            panelOpen = false;
            detailPanel.SetActive(false);

            if (promptUI != null && playerInZone)
                promptUI.SetActive(true);
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
        }
    }
}
