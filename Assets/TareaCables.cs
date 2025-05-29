using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaCables : MonoBehaviour
{
    public int conexionesActuales;
    public void ComprobarVictoria()
    {
        if (conexionesActuales == 4) ;
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
