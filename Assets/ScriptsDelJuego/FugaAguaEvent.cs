using UnityEngine;

public class ActivarFugaAgua : MonoBehaviour
{
    public GameObject sistemaFugaAgua;
    public GameObject panelAdvertencia;

    private bool fugaActiva = false;

    void OnEnable()
    {
        GameManager.OnNuevoDia += RevisarDia;
    }

    void OnDisable()
    {
        GameManager.OnNuevoDia -= RevisarDia;
    }

    void RevisarDia()
    {
        if (GameManager.Instance.currentDay == 2)
        {
            sistemaFugaAgua.SetActive(true);
            fugaActiva = true;

            Invoke("MostrarPanelAdvertencia", 7f);
            InvokeRepeating("ReducirFelicidadPresos", 0f, 1f); // Daño cada segundo
        }
    }

    void MostrarPanelAdvertencia()
    {
        panelAdvertencia.SetActive(true);
        Invoke("OcultarPanel", 5f);
    }

    void OcultarPanel()
    {
        panelAdvertencia.SetActive(false);
    }

    void ReducirFelicidadPresos()
    {
        if (!fugaActiva) return;

        GameObject[] presos = GameObject.FindGameObjectsWithTag("Prisionero");

        foreach (GameObject preso in presos)
        {
            SliderController slider = preso.GetComponentInChildren<SliderController>();
            if (slider != null)
            {
                slider.DecreaseHappiness(0.1f); // Ajusta según la dificultad que quieras
            }
        }
    }

    public void DetenerFuga()
    {
        fugaActiva = false;
        CancelInvoke("ReducirFelicidadPresos");
        sistemaFugaAgua.SetActive(false);
    }
}
