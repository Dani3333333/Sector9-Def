using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform handPoint; // Punto donde se sujetará el objeto
    private GameObject pickedObject; // Objeto actualmente recogido

    void Update()
    {
        // Verificar si tenemos un objeto agarrado
        if (pickedObject != null)
        {
            // Si presionamos "R", soltamos el objeto
            if (Input.GetKey("r"))
            {
                pickedObject.GetComponent<Rigidbody>().useGravity = true;
                pickedObject.GetComponent<Rigidbody>().isKinematic = false;
                pickedObject.transform.SetParent(null);
                pickedObject = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto"))
        {
            if (Input.GetKey("e") && pickedObject == null)
            {
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.transform.position = handPoint.position;
                other.transform.SetParent(handPoint);
                pickedObject = other.gameObject;
            }
        }
    }
}

