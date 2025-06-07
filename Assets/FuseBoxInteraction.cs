using UnityEngine;

public class FuseBoxInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public GameObject cableMinigameUI;
    public GameClock gameClock; // Arrastra el GameClock desde el inspector

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

            LogicaPersonaje1.isInspecting = true;

            if (PowerOutageController.Instance != null)
            {
                PowerOutageController.Instance.HidePowerOutageMessage();
            }

            if (gameClock != null)
            {
                gameClock.StopClock();
            }
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

    // LLAMAR DESDE BOTÓN "CERRAR"
    public void CloseMinigamePanel()
    {
        cableMinigameUI.SetActive(false);
        LogicaPersonaje1.isInspecting = false;

        if (gameClock != null)
        {
            gameClock.ResumeClock();
        }
    }
}
