using UnityEngine;

public class FuseBoxInteraction : MonoBehaviour
{
    public GameObject interactionPrompt;
    public GameClock gameClock;
    public GameObject cablesPanel; //

    private bool playerInRange = false;

    void Start()
    {
        interactionPrompt.SetActive(false);
        if (cablesPanel != null)
            cablesPanel.SetActive(false); // 
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LogicaPersonaje1.isInspecting = true;

            if (PowerOutageController.Instance != null)
            {
                PowerOutageController.Instance.HidePowerOutageMessage();
            }

            if (gameClock != null)
            {
                gameClock.StopClock();
            }

            if (cablesPanel != null)
            {
                cablesPanel.SetActive(true); // 
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
}
