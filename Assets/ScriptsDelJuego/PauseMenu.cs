using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Panel de pausa
    public Image backgroundImage;     // Imagen de fondo (espacio exterior)
    public Button resumeButton;       // Botón de reanudar
    public Button restartButton;      // Botón de reiniciar
    public Button loadCheckpointButton; // Botón de cargar punto de control
    public Button optionsButton;      // Botón de opciones
    public Button quitButton;         // Botón de salir al menú principal

    private bool isPaused = false;    // Estado del juego

    public float animationSpeed = 1f; // Velocidad de animación de la puerta

    void Update()
    {
        // Si presionamos ESC, alternamos el menú de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ClosePauseMenu();  // Cerrar el menú de pausa
            }
            else
            {
                OpenPauseMenu();   // Abrir el menú de pausa
            }
        }
    }

    void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // Bloquear el ratón
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

        // Bloquear el ratón de nuevo al reanudar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   

    void QuitToMainMenu()
    {
        // Aquí iría la funcionalidad de salir al menú principal (que aún no hemos hecho)
        Debug.Log("Salir al menú principal...");
    }
}
