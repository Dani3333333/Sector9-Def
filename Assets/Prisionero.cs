using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisionero : MonoBehaviour
{
    [Range(0f, 100f)]
    public float felicidad = 100f;

    public string nombre;

    private static int contadorGlobal = 0;

    void Awake()
    {
        contadorGlobal++;
        nombre = $"Prisionero {contadorGlobal}";
    }
}

