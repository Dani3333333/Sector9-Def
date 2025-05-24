using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeldaTrigger : MonoBehaviour
{
    public SliderController sliderController; // Drag & drop desde el Inspector

    private void OnTriggerEnter(Collider other)
    {
        Food comida = other.GetComponent<Food>();
        if (comida != null && !comida.yaUsado)
        {
            switch (comida.tipoDeComida)
            {
                case Food.TipoDeComida.Malo:
                    sliderController.DecreaseHappiness(40f);
                    break;
                case Food.TipoDeComida.Regular:
                    // No afecta felicidad
                    break;
                case Food.TipoDeComida.Bueno:
                    sliderController.IncreaseHappiness(20f);
                    break;
            }

            // 🔥 IMPORTANTE: Marcar que fue alimentado hoy
            sliderController.wasFedToday = true;

            comida.yaUsado = true;
            Destroy(other.gameObject); // O desactivarlo si prefieres
        }
    }
}
