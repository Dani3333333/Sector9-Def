using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float speed;
    public float moveDistance; // Distancia que se moverá la puerta hacia arriba
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    public bool puedeAbrir;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    void Update()
    {
        if (puedeAbrir && Input.GetMouseButtonDown(0)) // Click izquierdo del ratón
        {
            isOpen = !isOpen; // Alterna entre abierto y cerrado
        }

        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            puedeAbrir = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            puedeAbrir = false;
        }
    }
}


