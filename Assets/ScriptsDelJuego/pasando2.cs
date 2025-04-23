using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pasando2 : MonoBehaviour
{
    public Animator laPuerta;

    private void OnTriggerEnter(Collider other)
    {
        laPuerta.Play("abrir2");
    }

    private void OnTriggerExit(Collider other)
    {
        laPuerta.Play("cerrar2");
    }
}
