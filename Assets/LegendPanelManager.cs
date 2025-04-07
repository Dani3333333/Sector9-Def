using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendPanelController : MonoBehaviour
{
    public RectTransform panel;
    public float moveSpeed = 1000f;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private bool isVisible = false;

    void Start()
    {
        float panelHeight = panel.rect.height;
        visiblePos = new Vector2(-20f, 20f); // 20px desde el borde derecho
        hiddenPos = new Vector2(-20f, -panelHeight);

        panel.anchoredPosition = hiddenPos;
        panel.gameObject.SetActive(false);

        // Asegúrate de que el anchor del panel esté en Bottom Right
        panel.anchorMin = new Vector2(1, 0);
        panel.anchorMax = new Vector2(1, 0);
        panel.pivot = new Vector2(1, 0); // esquina inferior derecha
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isVisible = !isVisible;
            panel.gameObject.SetActive(true);
        }

        if (isVisible)
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, visiblePos, moveSpeed * Time.deltaTime);
        }
        else
        {
            panel.anchoredPosition = Vector2.MoveTowards(panel.anchoredPosition, hiddenPos, moveSpeed * Time.deltaTime);
        }

        if (!isVisible && panel.anchoredPosition == hiddenPos)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
