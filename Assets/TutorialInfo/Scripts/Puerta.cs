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
    private bool puedeAbrir;

    public TextMeshProUGUI interactionPrompt;
    public PrisonerPatrol prisonerPatrol;

    [Header("Día a partir del cual se puede abrir esta puerta (0 = siempre abierta)")]
    public int diaDisponible = 0;

    //  Audio
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

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

                if (audioSource != null)
                {
                    if (isOpen && sonidoAbrir != null)
                        audioSource.PlayOneShot(sonidoAbrir);
                    else if (!isOpen && sonidoCerrar != null)
                        audioSource.PlayOneShot(sonidoCerrar);
                }

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

    // Método público para cerrar la puerta desde otro script
    public void CerrarPuerta()
    {
        isOpen = false;

        if (audioSource != null && sonidoCerrar != null)
            audioSource.PlayOneShot(sonidoCerrar);

        if (prisonerPatrol != null && prisonerPatrol.IsOutsideCell)
            prisonerPatrol.ReturnToCell();

        if (interactionPrompt != null)
            interactionPrompt.text = "[Q] Abrir puerta";
    }
}
