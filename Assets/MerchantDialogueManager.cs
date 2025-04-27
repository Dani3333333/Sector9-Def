using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantDialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject salePanel;

    public void StartNegotiation()
    {
        dialoguePanel.SetActive(false);
        salePanel.SetActive(true);
    }
}
