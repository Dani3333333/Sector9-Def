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

    private float targetHappiness = 100f;  // Valor que queremos alcanzar
    private float currentHappiness = 100f; // Valor actual mostrado

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        valueText.transform.rotation = Quaternion.LookRotation(valueText.transform.position - Camera.main.transform.position);


        // Lerp de la barra
        if (Mathf.Abs(slider.value - targetHappiness) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHappiness, Time.deltaTime * animationSpeed);
            UpdateVisuals(); // Actualizar color y texto durante la animación
        }

        // Mover la imagen de la carita de acuerdo con el valor del slider
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

        // Cambiar el color de la barra de felicidad
        if (slider.value >= 80f)
            fillImage.color = Color.green;
        else if (slider.value >= 50f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;

        // Cambiar la imagen de la carita según el valor de la barra
        if (slider.value >= 80f)
        {
            faceImage.sprite = happyFace;  // Carita feliz
        }
        else if (slider.value >= 50f)
        {
            faceImage.sprite = neutralFace;  // Carita neutral
        }
        else
        {
            faceImage.sprite = sadFace;  // Carita triste
        }
    }

    void MoveFaceImage()
    {
        // La imagen de la carita se mueve a lo largo de la barra de felicidad
        float xPosition = Mathf.Lerp(-slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.normalizedValue);

        faceImage.transform.localPosition = new Vector3(xPosition, faceImage.transform.localPosition.y, faceImage.transform.localPosition.z);
    }
}
