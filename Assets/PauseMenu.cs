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

    private bool isPaused = false;    // Estado del juego

    public float animationSpeed = 1f; // Velocidad de animaci�n de la puerta

    void Update()
    {
        // Si presionamos ESC, alternamos el men� de pausa
        if (Input.GetKeyDown(KeyCode.Escape))
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

    void OpenPauseMenu()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);

        // Bloquear el rat�n
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Animaci�n de apertura del men� (puerta de nave espacial)
        StartCoroutine(OpenMenuAnimation());

        // Hacer que los botones respondan
        resumeButton.onClick.AddListener(ClosePauseMenu);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    void ClosePauseMenu()
    {
        isPaused = false;

        // Animaci�n de cierre del men� (puerta de nave espacial)
        StartCoroutine(CloseMenuAnimation());

        // Bloquear el rat�n de nuevo al reanudar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator OpenMenuAnimation()
    {
        RectTransform panelRect = pauseMenuPanel.GetComponent<RectTransform>();

        // Empezamos desde el centro del panel hacia afuera (efecto "puerta de nave espacial")
        panelRect.localScale = Vector3.zero; // Empieza desde el centro
        Vector3 targetScale = Vector3.one;

        // Animaci�n
        float elapsedTime = 0f;
        while (elapsedTime < animationSpeed)
        {
            panelRect.localScale = Vector3.Lerp(Vector3.zero, targetScale, (elapsedTime / animationSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelRect.localScale = targetScale;
    }

    IEnumerator CloseMenuAnimation()
    {
        RectTransform panelRect = pauseMenuPanel.GetComponent<RectTransform>();

        // Animaci�n de cierre (de afuera al centro)
        Vector3 targetScale = Vector3.zero;

        // Animaci�n
        float elapsedTime = 0f;
        while (elapsedTime < animationSpeed)
        {
            panelRect.localScale = Vector3.Lerp(Vector3.one, targetScale, (elapsedTime / animationSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        panelRect.localScale = targetScale;
        pauseMenuPanel.SetActive(false); // Desactivamos el panel cuando la animaci�n termine
    }

    void QuitToMainMenu()
    {
        // Aqu� ir�a la funcionalidad de salir al men� principal (que a�n no hemos hecho)
        Debug.Log("Salir al men� principal...");
    }
}
