using UnityEngine;

public class LuzPorDia : MonoBehaviour
{
    public int diaActivacion = 0;
    private Light luz;

    void Awake()
    {
        luz = GetComponent<Light>();
        if (luz == null)
        {
            Debug.LogWarning("No se encontró un componente Light en " + gameObject.name);
        }
    }

    void Update()
    {
        if (luz != null)
        {
            // Activar solo si no estaba activada antes y el día ha llegado
            if (GameManager.Instance.currentDay >= diaActivacion && !luz.enabled)
            {
                luz.enabled = true;
            }
        }
    }
}
