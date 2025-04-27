using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantInteraction : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject interactionPrompt; // Un texto que diga "[E] Hablar con el vendedor"
    private bool playerInRange = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialoguePanel.SetActive(true);
            interactionPrompt.SetActive(false);
            Time.timeScale = 0f; // Pausamos el juego mientras hablamos
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
        }
    }
}
