using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerInteraction : MonoBehaviour
{
    public GameObject inspectionPanel; // Panel UI de selección de extremidades
    private bool isNearPrisoner = false;

    private PrisonerPatrol patrolScript;

    void Start()
    {
        patrolScript = GetComponent<PrisonerPatrol>();
    }

    void Update()
    {
        if (isNearPrisoner && Input.GetKeyDown(KeyCode.E))
        {
            OpenInspectionPanel();
        }
    }

    private void OpenInspectionPanel()
    {
        inspectionPanel.SetActive(true);
        // Ya no controlamos isBeingInspected ni movimiento
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPrisoner = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPrisoner = false;
            inspectionPanel.SetActive(false);
        }
    }
}
