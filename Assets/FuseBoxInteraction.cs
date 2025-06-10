using UnityEngine;

public class FuseBoxInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;

    private bool playerInRange = false;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        // Solo permitir interacci�n si las luces est�n apagadas y el jugador est� cerca
        if (playerInRange && PowerOutageController.Instance != null && PowerOutageController.Instance.IsLightsOut())
        {
            if (!interactionPrompt.activeSelf)
                interactionPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                PowerOutageController.Instance.RegisterFuseBoxHit();
            }
        }
        else
        {
            // Si el jugador no est� cerca o las luces est�n encendidas, ocultar prompt
            if (interactionPrompt.activeSelf)
                interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Mostrar prompt solo si luces apagadas
            if (PowerOutageController.Instance != null && PowerOutageController.Instance.IsLightsOut())
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
