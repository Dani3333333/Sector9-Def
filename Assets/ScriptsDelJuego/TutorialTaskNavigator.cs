using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskNavigator : MonoBehaviour
{
    public static TutorialTaskNavigator Instance;

    public List<GameObject> taskPanels;     // Los paneles que se mostrarán como diapositivas
    public List<Toggle> taskToggles;        // Los toggles que se activan con Enter

    public TutorialPanelReader panelReader; // Referencia al panel lector para cerrar

    private int currentIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        // Desactivar todos los paneles, activar solo el primero
        for (int i = 0; i < taskPanels.Count; i++)
        {
            taskPanels[i].SetActive(i == 0);
        }

        // Desmarcar todos los toggles
        foreach (var toggle in taskToggles)
        {
            toggle.isOn = false;
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToPanel(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToPanel(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleCurrent();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (panelReader != null)
            {
                panelReader.CloseDetailPanel();
            }
        }
    }

    void MoveToPanel(int direction)
    {
        int newIndex = currentIndex + direction;

        if (newIndex < 0 || newIndex >= taskPanels.Count)
        {
            return; // No moverse fuera de los límites
        }

        taskPanels[currentIndex].SetActive(false);
        currentIndex = newIndex;
        taskPanels[currentIndex].SetActive(true);
    }

    void ToggleCurrent()
    {
        if (currentIndex >= 0 && currentIndex < taskToggles.Count)
        {
            var toggle = taskToggles[currentIndex];
            toggle.isOn = !toggle.isOn;
        }
    }

    public bool AreAllTasksCompleted()
    {
        foreach (var toggle in taskToggles)
        {
            if (!toggle.isOn)
                return false;
        }
        return true;
    }

    public void ResetToFirstPanel()
    {
        for (int i = 0; i < taskPanels.Count; i++)
        {
            bool isActive = (i == 0);
            taskPanels[i].SetActive(isActive);
            Debug.Log($"Panel {i} ({taskPanels[i].name}) activado: {isActive}");
        }
        currentIndex = 0;
    }
}
