using UnityEngine;
using TMPro;

public class PrisonerHappinessPanel : MonoBehaviour
{
    public RectTransform panel;
    public float moveSpeed = 1000f;

    public Prisionero prisoner1;
    public Prisionero prisoner2;
    public Prisionero prisoner3;
    public Prisionero prisoner4;

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
            {
                panel.gameObject.SetActive(false);
            }
        }
    }

    void UpdateTexts()
    {
        if (prisoner1 != null && prisoner1.textoFelicidad != null)
            prisoner1.textoFelicidad.text = $"{prisoner1.felicidad:F0}%";

        if (prisoner2 != null && prisoner2.textoFelicidad != null)
            prisoner2.textoFelicidad.text = $"{prisoner2.felicidad:F0}%";

        if (prisoner3 != null && prisoner3.textoFelicidad != null)
            prisoner3.textoFelicidad.text = $"{prisoner3.felicidad:F0}%";

        if (prisoner4 != null && prisoner4.textoFelicidad != null)
            prisoner4.textoFelicidad.text = $"{prisoner4.felicidad:F0}%";
    }
}
