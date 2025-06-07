using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaCables : MonoBehaviour
{
    public int conexionesActuales;

    public PowerOutageController powerOutageController;

    public void ComprobarVictoria()
    {
        if (conexionesActuales == 4)
        {
            powerOutageController.RestoreLights();
            Destroy(this.gameObject, 1f); // Cierra el panel del minijuego
        }
    }
}

