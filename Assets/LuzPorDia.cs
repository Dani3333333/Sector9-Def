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
            Debug.LogWarning("No se encontr� un componente Light en " + gameObject.name);
        }
    }

    void Update()
    {
        if (luz != null)
        {
            // Activar solo si no estaba activada antes y el d�a ha llegado
            if (GameManager.Instance.currentDay >= diaActivacion && !luz.enabled)
            {
                luz.enabled = true;
            }
        }
    }
}
