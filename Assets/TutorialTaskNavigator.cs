using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskNavigator : MonoBehaviour
{
    public static TutorialTaskNavigator Instance; // <-- para poder preguntarle desde fuera
    public List<Toggle> taskToggles; // Toggles de cada tarea
    private int currentIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        // Dejar todos los toggles desmarcados al iniciar
        foreach (var toggle in taskToggles)
        {
            toggle.isOn = false;
        }

        if (taskToggles.Count > 0)
        {
            HighlightToggle(currentIndex);
        }
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return; // No hacer nada si el panel está cerrado

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter
        {
            ToggleCurrent();
        }
    }

    void MoveSelection(int direction)
    {
        UnhighlightToggle(currentIndex);

        currentIndex += direction;
        if (currentIndex < 0) currentIndex = taskToggles.Count - 1;
        if (currentIndex >= taskToggles.Count) currentIndex = 0;

        HighlightToggle(currentIndex);
    }

    void HighlightToggle(int index)
    {
        var colors = taskToggles[index].colors;
        colors.normalColor = Color.yellow; // Resaltar seleccionado
        taskToggles[index].colors = colors;
    }

    void UnhighlightToggle(int index)
    {
        var colors = taskToggles[index].colors;
        colors.normalColor = Color.white; // Quitar resalte
        taskToggles[index].colors = colors;
    }

    void ToggleCurrent()
    {
        var toggle = taskToggles[currentIndex];
        toggle.isOn = !toggle.isOn;
    }

    //Función para consultar si TODAS las tareas están completas
    public bool AreAllTasksCompleted()
    {
        foreach (var toggle in taskToggles)
        {
            if (!toggle.isOn)
                return false;
        }
        return true;
    }
}
