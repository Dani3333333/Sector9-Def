using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrisonerHappinessPanel : MonoBehaviour
{
    public RectTransform panel;
    public GameObject prisonerEntryPrefab;
    public Transform container;
    public float moveSpeed = 1000f;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private bool isVisible = false;

    void Start()
    {
        float panelHeight = panel.rect.height;

        // Anclar el panel en la esquina inferior izquierda
        panel.anchorMin = new Vector2(0, 0);
        panel.anchorMax = new Vector2(0, 0);
        panel.pivot = new Vector2(0, 0); // esquina inferior izquierda

        visiblePos = new Vector2(20f, 20f); // antes estaba en 0, ahora a 20 píxeles del borde izquierdo
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
                PopulatePrisonerEntries();
            }
        }

        if (isVisible)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, visiblePos, moveSpeed * Time.deltaTime);
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

    void PopulatePrisonerEntries()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        GameObject[] prisoners = GameObject.FindGameObjectsWithTag("Prisionero");

        int count = 1;
        foreach (GameObject prisoner in prisoners)
        {
            GameObject entry = Instantiate(prisonerEntryPrefab, container);

            // Buscar componentes hijos
            TextMeshProUGUI nameText = entry.GetComponentInChildren<TextMeshProUGUI>();
            Slider slider = entry.GetComponentInChildren<Slider>();

            if (nameText != null)
            {
                nameText.text = $"Prisionero {count}";
            }

            if (slider != null)
            {
                // Puedes obtener un valor de felicidad desde un script en el prisionero si quieres
                float happiness = Random.Range(0.2f, 1f); // temporal
                slider.value = happiness;
            }

            count++;
        }
    }
}
