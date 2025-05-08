using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerIdentifier : MonoBehaviour
{
    public int prisonerID; 
    public string prisonerName;

    void Start()
    {
        prisonerName = "Prisionero " + prisonerID;
    }
}
