using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puerta : MonoBehaviour
{
    public float speed;
    public float moveDistance;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isOpen = false;
    public bool puedeAbrir;

    public TextMeshProUGUI interactionPrompt;

    public PrisonerPatrol prisonerPatrol;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;

        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (puedeAbrir && Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;

            // Mover prisionero una sola vez al cambiar el estado
            if (prisonerPatrol != null)
            {
                if (isOpen && !prisonerPatrol.IsOutsideCell)  // Utilizando la propiedad IsOutsideCell
                {
                    prisonerPatrol.ExitCell();
                }
                else if (!isOpen && prisonerPatrol.IsOutsideCell)  // Utilizando la propiedad IsOutsideCell
                {
                    prisonerPatrol.ReturnToCell();
                }
            }

            if (interactionPrompt != null)
                interactionPrompt.text = isOpen ? "[Q] Cerrar puerta" : "[Q] Abrir puerta";
        }

        // Movimiento físico de la puerta
        Vector3 target = isOpen ? targetPosition : initialPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puedeAbrir = true;

            if (interactionPrompt != null && !interactionPrompt.gameObject.activeSelf)
            {
                interactionPrompt.text = isOpen ? "[Q] Cerrar puerta" : "[Q] Abrir puerta";
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
                interactionPrompt.gameObject.SetActive(false);
        }
    }
}
