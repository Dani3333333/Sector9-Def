using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puerta : MonoBehaviour
{
    public float speed;
    public float moveDistance; // Distancia que se moverá la puerta hacia arriba
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    public bool puedeAbrir;

    public TextMeshProUGUI interactionPrompt; // <- NUEVO

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;

        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false); // Oculta el texto al iniciar
    }

    void Update()
    {
        if (puedeAbrir && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;

            // Actualiza el texto del prompt si sigue visible
            if (interactionPrompt != null)
                interactionPrompt.text = isOpen ? "[E] Cerrar puerta" : "[E] Abrir puerta";
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
        if (other.CompareTag("Player"))
        {
            puedeAbrir = true;

            if (interactionPrompt != null && !interactionPrompt.gameObject.activeSelf)
            {
                interactionPrompt.text = isOpen ? "[E] Cerrar puerta" : "[E] Abrir puerta";
                interactionPrompt.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeAbrir = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}
