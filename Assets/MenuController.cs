using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Animator animator; // Animator que controla flotar y despegue
    public Image fadePanel;

    public void PlayGame()
    {
        StartCoroutine(FadeAndStart());
    }

    IEnumerator FadeAndStart()
    {
        // Lanza la animación de despegue
        animator.SetTrigger("TakeOff");

        // Espera que termine la animación (ajusta si es más larga)
        yield return new WaitForSeconds(2f);

        // Fade out
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            fadePanel.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // Carga la siguiente escena
        SceneManager.LoadScene("Sector 9");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
