using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialObjectivesPanel : MonoBehaviour
{
    public GameObject promptUI; // El mensaje "[E] Leer Panel"
    public GameObject objectivesUI; // El panel que muestra los toggles
    public List<Toggle> objectiveToggles;

    private bool isNearPanel = false;

    void Update()
    {
        if (isNearPanel && Input.GetKeyDown(KeyCode.E))
        {
            objectivesUI.SetActive(!objectivesUI.activeSelf);
        }
    }

    public void MarkObjectiveComplete(int index)
    {
        if (index >= 0 && index < objectiveToggles.Count)
        {
            objectiveToggles[index].isOn = true;
        }
    }

    public bool AllObjectivesComplete()
    {
        foreach (Toggle toggle in objectiveToggles)
        {
            if (!toggle.isOn) return false;
        }
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPanel = true;
            promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearPanel = false;
            promptUI.SetActive(false);
            objectivesUI.SetActive(false);
        }
    }
}
