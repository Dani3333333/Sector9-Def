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

    public PrisonerPatrol prisonerPatrol; // Referencia al script de patrol del prisionero

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;

        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false); // Oculta el texto al iniciar
    }

    void Update()
    {
        if (puedeAbrir && Input.GetKeyDown(KeyCode.P))
        {
            isOpen = !isOpen;

            // Actualiza el texto del prompt si sigue visible
            if (interactionPrompt != null)
                interactionPrompt.text = isOpen ? "[P] Cerrar puerta" : "[P] Abrir puerta";
        }

        if (isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Si la puerta se abre, hacemos que el prisionero salga de la celda
            if (!prisonerPatrol.isOutsideCell)
            {
                prisonerPatrol.ExitCell();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);

            // Si la puerta se cierra y el prisionero está fuera de la celda, lo hacemos regresar
            if (prisonerPatrol.isOutsideCell)
            {
                prisonerPatrol.ReturnToCell();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeAbrir = true;

            if (interactionPrompt != null && !interactionPrompt.gameObject.activeSelf)
            {
                interactionPrompt.text = isOpen ? "[P] Cerrar puerta" : "[P] Abrir puerta";
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
