using UnityEngine;

public class VolanteInteractivo : MonoBehaviour
{
    public bool fueGirado = false;
    public float velocidadRotacion = 180f;
    private bool enRango = false;
    private bool estaGirando = false;

    public PuzzleSecuenciaManager manager;

    void Update()
    {
        if (enRango && Input.GetKeyDown(KeyCode.E) && !fueGirado)
        {
            StartCoroutine(GirarVolante());
        }
    }

    private System.Collections.IEnumerator GirarVolante()
    {
        estaGirando = true;

        float rotacionTotal = 0f;
        while (rotacionTotal < 180f)
        {
            float rotacionFrame = velocidadRotacion * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotacionFrame);
            rotacionTotal += rotacionFrame;
            yield return null;
        }

        fueGirado = true;
        estaGirando = false;

        if (manager != null)
            Debug.Log(" Volante girado correctamente.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            enRango = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            enRango = false;
    }
}
