using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrisonerEntryUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider happinessSlider;

    private Prisionero targetPrisoner;

    public void Setup(Prisionero prisoner, int index)
    {
        targetPrisoner = prisoner;
        nameText.text = string.IsNullOrEmpty(prisoner.nombre) ? $"Prisionero {index}" : prisoner.nombre;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (targetPrisoner != null)
        {
            happinessSlider.value = Mathf.Clamp01(targetPrisoner.felicidad / 100f);
        }
    }
}
