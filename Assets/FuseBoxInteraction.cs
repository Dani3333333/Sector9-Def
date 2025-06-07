using UnityEngine;
using UnityEngine.UI;

public class FuseBoxInteraction : MonoBehaviour
{
    public GameObject interactionPrompt; // El texto [E] Abrir panel
    public GameObject cableMinigameUI;   // El panel del minijuego

    private bool playerInRange = false;

    void Start()
    {
        interactionPrompt.SetActive(false);
        cableMinigameUI.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            cableMinigameUI.SetActive(true);
            interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactionPrompt.SetActive(false);
        }
    }
}
