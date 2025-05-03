using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrisonerHappinessPanel : MonoBehaviour
{
    public RectTransform panel; // El panel que se mueve

    public TextMeshProUGUI prisoner1Name;
    public TextMeshProUGUI prisoner2Name;
    public TextMeshProUGUI prisoner3Name;
    public TextMeshProUGUI prisoner4Name;

    public TextMeshProUGUI prisoner1Happiness;
    public TextMeshProUGUI prisoner2Happiness;
    public TextMeshProUGUI prisoner3Happiness;
    public TextMeshProUGUI prisoner4Happiness;

    public SliderController prisoner1;
    public SliderController prisoner2;
    public SliderController prisoner3;
    public SliderController prisoner4;

    public float moveSpeed = 1000f;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private bool isVisible = false;

    void Start()
    {
        float panelHeight = panel.rect.height;

        panel.anchorMin = new Vector2(0, 0);
        panel.anchorMax = new Vector2(0, 0);
        panel.pivot = new Vector2(0, 0);

        visiblePos = new Vector2(20f, 20f);
        hiddenPos = new Vector2(20f, -panelHeight);

        panel.anchoredPosition = hiddenPos;
        panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isVisible = !isVisible;
            if (isVisible)
            {
                panel.gameObject.SetActive(true);
                UpdateTexts();
            }
        }

        if (isVisible)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, visiblePos, moveSpeed * Time.deltaTime);
            UpdateTexts();
        }
        else
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, hiddenPos, moveSpeed * Time.deltaTime);
            if (panel.anchoredPosition == hiddenPos)
                panel.gameObject.SetActive(false);
        }
    }

    void UpdateTexts()
    {
        prisoner1Name.text = "Prisoner 1";
        prisoner2Name.text = "Prisoner 2";
        prisoner3Name.text = "Prisoner 3";
        prisoner4Name.text = "Prisoner 4";

        prisoner1Happiness.text = $"{prisoner1.GetCurrentHappiness():F0}%";
        prisoner2Happiness.text = $"{prisoner2.GetCurrentHappiness():F0}%";
        prisoner3Happiness.text = $"{prisoner3.GetCurrentHappiness():F0}%";
        prisoner4Happiness.text = $"{prisoner4.GetCurrentHappiness():F0}%";
    }
}
