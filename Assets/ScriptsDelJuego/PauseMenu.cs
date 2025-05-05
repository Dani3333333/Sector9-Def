using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Panel de pausa
    public GameObject optionsMenuPanel; // Tu men� de opciones (asegurate de asignarlo en el Inspector)
    public Button resumeButton;       // Bot�n de reanudar
    public Button optionsButton;      // Bot�n de opciones
    public Button quitButton;         // Bot�n de salir del juego

    private bool isPaused = false;
    private bool gameStarted = false;

    void Start()
    {
        // Asignar funciones a botones
        resumeButton.onClick.AddListener(ClosePauseMenu);
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        quitButton.onClick.AddListener(SalirDelJuego);

        // Asegurarse de que el men� de pausa y opciones est�n desactivados al inicio
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ClosePauseMenu()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(false); // por si estaba abierto
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OpenOptionsMenu()
    {
        optionsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
