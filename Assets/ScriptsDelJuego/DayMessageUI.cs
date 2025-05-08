using System.Collections;
using TMPro;
using UnityEngine;

public class DayMessageUI : MonoBehaviour
{
    public TextMeshProUGUI dayMessageText;

    public void ShowDayMessage(int day)
    {
        StartCoroutine(ShowMessageRoutine(day));
    }

    IEnumerator ShowMessageRoutine(int day)
    {
        dayMessageText.text = "D�a " + day;
        dayMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        dayMessageText.gameObject.SetActive(false);
    }
}
