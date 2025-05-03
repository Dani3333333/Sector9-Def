using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrisonerEntryUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider happinessSlider;

    private SliderController sliderController;

    public void Setup(SliderController controller, int index)
    {
        sliderController = controller;
        nameText.text = $"Prisionero {index}";
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (sliderController != null)
        {
            float happiness = sliderController.GetCurrentHappiness();
            happinessSlider.value = Mathf.Clamp01(happiness / 100f);
        }
    }
}
