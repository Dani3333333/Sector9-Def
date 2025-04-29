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

    private List<PrisonerEntryUI> prisonerEntries = new List<PrisonerEntryUI>();

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
                PopulatePrisonerEntries();
            }
        }

        if (isVisible)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, visiblePos, moveSpeed * Time.deltaTime);

            foreach (var entry in prisonerEntries)
            {
                entry.UpdateUI();
            }
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

        prisonerEntries.Clear();

        GameObject[] prisoners = GameObject.FindGameObjectsWithTag("Prisionero");

        int count = 1;
        foreach (GameObject prisonerGO in prisoners)
        {
            Prisionero prisoner = prisonerGO.GetComponent<Prisionero>();
            if (prisoner != null)
            {
                GameObject entryGO = Instantiate(prisonerEntryPrefab, container);
                PrisonerEntryUI entryUI = entryGO.GetComponent<PrisonerEntryUI>();

                if (entryUI != null)
                {
                    entryUI.Setup(prisoner, count);
                    prisonerEntries.Add(entryUI);
                }

                count++;
            }
        }
    }
}
