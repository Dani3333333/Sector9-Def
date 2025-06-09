using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SliderController : MonoBehaviour
{
    public Slider slider;
    public Text valueText;
    public Image fillImage;

    public Sprite happyFace;
    public Sprite neutralFace;
    public Sprite sadFace;
    public Image faceImage;

    public float animationSpeed = 5f;

    public float targetHappiness = 100f;
    public float currentHappiness = 100f;

    [HideInInspector]
    public bool wasFedToday = false;

    public GameObject prisonerRoot;

    public TextMeshProUGUI deathMessageText;  // Cambiado a TextMeshProUGUI
    public float deathMessageDuration = 4f;

    private bool hasDied = false;

    private readonly string[] deathMessages = new string[]
    {
        "No has cuidado bien a tu prisionero, ha muerto...",
        "El prisionero se ha rendido ante la desesperación.",
        "Su falta de felicidad le ha llevado al fin."
    };

    void Update()
    {
        transform.LookAt(Camera.main.transform);
        valueText.transform.rotation = Quaternion.LookRotation(valueText.transform.position - Camera.main.transform.position);

        if (Mathf.Abs(slider.value - targetHappiness) > 0.01f)
        {
            slider.value = Mathf.Lerp(slider.value, targetHappiness, Time.deltaTime * animationSpeed);
            UpdateVisuals();
        }

        MoveFaceImage();

        if (!hasDied && targetHappiness <= 0f)
        {
            hasDied = true;
            StartCoroutine(HandleDeath());
        }
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

        if (slider.value >= 80f)
            fillImage.color = Color.green;
        else if (slider.value >= 50f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;

        if (slider.value >= 80f)
            faceImage.sprite = happyFace;
        else if (slider.value >= 50f)
            faceImage.sprite = neutralFace;
        else
            faceImage.sprite = sadFace;
    }

    void MoveFaceImage()
    {
        float xPosition = Mathf.Lerp(-slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.GetComponent<RectTransform>().sizeDelta.x / 2,
                                     slider.normalizedValue);

        faceImage.transform.localPosition = new Vector3(xPosition, faceImage.transform.localPosition.y, faceImage.transform.localPosition.z);
    }

    public float GetCurrentHappiness()
    {
        return slider.value;
    }

    public void CheckIfFedAndPenalize()
    {
        if (!wasFedToday)
        {
            DecreaseHappiness(15f);
        }

        wasFedToday = false;
    }

    IEnumerator HandleDeath()
    {
        if (deathMessageText != null)
        {
            int index = Random.Range(0, deathMessages.Length);
            deathMessageText.text = deathMessages[index];
            deathMessageText.enabled = true;
        }

        yield return new WaitForSeconds(deathMessageDuration);

        if (deathMessageText != null)
        {
            deathMessageText.enabled = false;
        }

        if (prisonerRoot != null)
            Destroy(prisonerRoot);
        else
            Destroy(gameObject);
    }
}