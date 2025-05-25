using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FugaAguaEvent : MonoBehaviour
{
    public ParticleSystem fugaAgua;
    public float duracionFuga = 10f; // Duración en segundos
    private bool fugaYaOcurrio = false;

    void OnEnable()
    {
        GameManager.OnNuevoDia += VerificarFuga;
    }

    void OnDisable()
    {
        GameManager.OnNuevoDia -= VerificarFuga;
    }

    void VerificarFuga()
    {
        if (!fugaYaOcurrio && GameManager.Instance.currentDay == 2)
        {
            fugaYaOcurrio = true;
            ActivarFuga();
        }
    }

    void ActivarFuga()
    {
        Debug.Log("¡Fuga de agua iniciada!");
        fugaAgua.Play();
        StartCoroutine(DetenerFugaTrasTiempo());
    }

    IEnumerator DetenerFugaTrasTiempo()
    {
        yield return new WaitForSeconds(duracionFuga);
        fugaAgua.Stop();
        Debug.Log("Fuga de agua reparada.");
    }
}
