using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public float delay = 0.05f;

    public void StartTyping(string fullText)
    {
        StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string text)
    {
        targetText.text = "";
        foreach (char c in text)
        {
            targetText.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}
