using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizarCables : MonoBehaviour
{
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cableActual = transform.GetChild(i).gameObject;
            int randomIndex = Random.Range(0, transform.childCount);
            GameObject otroCable = transform.GetChild(randomIndex).gameObject;

            Vector2 tempPos = cableActual.transform.position;
            cableActual.transform.position = otroCable.transform.position;
            otroCable.transform.position = tempPos;
        }
    }
}

