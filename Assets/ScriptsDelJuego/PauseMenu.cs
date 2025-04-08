using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Panel de pausa
    public Image backgroundImage;     // Imagen de fondo (espacio exterior)
    public Button resumeButton;       // Bot�n de reanudar
    public Button restartButton;      // Bot�n de reiniciar
    public Button loadCheckpointButton; // Bot�n de cargar punto de control
    public Button optionsButton;      // Bot�n de opciones
    public Button quitButton;         // Bot�n de salir al men� principal
    public Button playButton;         // Bot�n de jugar (debe ser asignado desde el men� principal)

    private bool isPaused = false;    // Estado del juego
    private bool gameStarted = false; // Estado de si el juego ha comenzado

    public float animationSpeed = 1f; // Velocidad de animaci�n de la puerta

    void Start()
    {
        // Asegurarse de que el bot�n "Jugar" tenga la funci�n correcta
        playButton.onClick.AddListener(StartGame);
    }

    void Update()
    {
        // Si el juego ya ha comenzado, permitir abrir el men� con ESC
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePauseMenu();  // Cerrar el men� de pausa
            }
            else
            {
                OpenPauseMenu();   // Abrir el men� de pausa
            }
        }
    }

    void StartGame()
    {
        // Marcar que el juego ha comenzado y deshabilitar el men� de pausa temporalmente
        gameStarted = true;
        Debug.Log("Juego iniciado!");
    }

    void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // Bloquear el rat�n
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Hacer que los botones respondan
        resumeButton.onClick.AddListener(ClosePauseMenu);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    void ClosePauseMenu()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);

        // Bloquear el rat�n de nuevo al reanudar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void QuitToMainMenu()
    {
        // Aqu� ir�a la funcionalidad de salir al men� principal (que a�n no hemos hecho)
        Debug.Log("Salir al men� principal...");
    }
}
