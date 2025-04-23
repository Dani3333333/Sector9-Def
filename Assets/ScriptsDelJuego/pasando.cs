using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pasando : MonoBehaviour
{
    public Animator laPuerta;

    private void OnTriggerEnter(Collider other)
    {
        laPuerta.Play("abrir");
    }

    private void OnTriggerExit(Collider other)
    {
        laPuerta.Play("cerrar");
    }
}
