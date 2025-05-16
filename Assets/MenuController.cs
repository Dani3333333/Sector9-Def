using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Animator animator;       // Animator que controla flotar y despegue
    public Image fadePanel;         // Imagen negra para el fade out
    public float fadeDuration = 1f; // Duraci�n del fade
    public string sceneToLoad = "Sector 9"; // Nombre de la escena siguiente
    public GameObject settingsPanel;  // Asigna en el inspector el panel de configuraci�n

    public void PlayGame()
    {
        // Activa el trigger que inicia la animaci�n de despegue
        animator.SetTrigger("TakeOff");
        StartCoroutine(FadeAndStart());
    }

    IEnumerator FadeAndStart()
    {
        // Espera el tiempo de despegue (ajusta seg�n duraci�n real del clip)
        yield return new WaitForSeconds(2f);

        // Aseg�rate de que el fadePanel est� al frente
        fadePanel.transform.SetAsLastSibling();

        // Fade out gradual
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Carga la siguiente escena
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
