using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public Text valueText;
    public Image fillImage;

    // Para las caritas
    public Sprite happyFace;         // Carita feliz
    public Sprite neutralFace;       // Carita neutral
    public Sprite sadFace;           // Carita triste
    public Image faceImage;          // Imagen de la carita (feliz, neutral, triste) que se mueve

    public float animationSpeed = 5f;

    public float targetHappiness = 100f;  // Valor que queremos alcanzar
    public float currentHappiness = 100f; // Valor actual mostrado

    void Update()
    {
        // Mantener siempre de cara a la cámara
        transform.LookAt(Camera.main.transform);
        valueText.transform.rotation = Quaternion.LookRotation(valueText.transform.position - Camera.main.transform.position);

        // Animar suavemente el valor del slider hacia el objetivo
        if (Mathf.Abs(slider.value - targetHappiness) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHappiness, Time.deltaTime * animationSpeed);
            UpdateVisuals();
        }

        MoveFaceImage();
    }

    public void DecreaseHappiness(float amount)
    {
        targetHappiness = Mathf.Clamp(targetHappiness - amount, 0f, 100f);
    }

    public void IncreaseHappiness(float amount)
    {
        targetHappiness = Mathf.Clamp(targetHappiness + amount, 0f, 100f);
    }

    void UpdateVisuals()
    {
        valueText.text = $"Felicidad: {Mathf.RoundToInt(slider.value)}%";

        // Color de la barra
        if (slider.value >= 80f)
            fillImage.color = Color.green;
        else if (slider.value >= 50f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;

        // Cambiar la carita
        if (slider.value >= 80f)
        {
            faceImage.sprite = happyFace;
        }
        else if (slider.value >= 50f)
        {
            faceImage.sprite = neutralFace;
        }
        else
        {
            faceImage.sprite = sadFace;
        }
    }

    void MoveFaceImage()
    {
        float xPosition = Mathf.Lerp(-slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.normalizedValue);

        faceImage.transform.localPosition = new Vector3(xPosition, faceImage.transform.localPosition.y, faceImage.transform.localPosition.z);
    }

    // Método para obtener el valor actual de felicidad
    public float GetCurrentHappiness()
    {
        return slider.value;
    }
}
