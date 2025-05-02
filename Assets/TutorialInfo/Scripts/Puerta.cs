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

    [Header("Día a partir del cual se puede abrir esta puerta (0 = siempre abierta)")]
    public int diaDisponible = 0;

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
            if (GameManager.Instance.currentDay >= diaDisponible)
            {
                isOpen = !isOpen;

                if (prisonerPatrol != null)
                {
                    if (isOpen && !prisonerPatrol.IsOutsideCell)
                        prisonerPatrol.ExitCell();
                    else if (!isOpen && prisonerPatrol.IsOutsideCell)
                        prisonerPatrol.ReturnToCell();
                }

                if (interactionPrompt != null)
                    interactionPrompt.text = isOpen ? "[Q] Cerrar puerta" : "[Q] Abrir puerta";
            }
            else
            {
                if (interactionPrompt != null)
                    interactionPrompt.text = "Puerta bloqueada";
            }
        }

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
                if (GameManager.Instance.currentDay >= diaDisponible)
                    interactionPrompt.text = isOpen ? "[Q] Cerrar puerta" : "[Q] Abrir puerta";
                else
                    interactionPrompt.text = "Puerta bloqueada";

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
