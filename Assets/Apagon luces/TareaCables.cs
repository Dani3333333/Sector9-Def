using UnityEngine;

public class TareaCables : MonoBehaviour
{
    public int conexionesActuales;
    public GameObject cablesPanel; // 

    public void ComprobarVictoria()
    {
        if (conexionesActuales == 4)
        {
            CerrarMinijuego();
        }
    }

    void CerrarMinijuego()
    {
        if (PowerOutageController.Instance != null)
        {
            PowerOutageController.Instance.RestoreLights();
        }

        if (cablesPanel != null)
        {
            cablesPanel.SetActive(false); // 
        }

        GameClock clock = FindObjectOfType<GameClock>();
        if (clock != null)
        {
            clock.ResumeClock();
        }

        LogicaPersonaje1.isInspecting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
