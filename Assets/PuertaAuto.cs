using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaAuto : MonoBehaviour
{
    
    public float speed;
    public float moveDistance; // Distancia que se moverá la puerta hacia arriba
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOpen = true; // Abre la puerta cuando detecta al jugador
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOpen = false; // Cierra la puerta cuando el jugador sale del área
        }
    }

}
