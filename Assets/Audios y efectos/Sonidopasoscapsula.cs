using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SonidoPasosCapsula : MonoBehaviour
{
    public AudioSource audioPasos;
    public float umbralMovimiento = 0.1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocidadHorizontal = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (velocidadHorizontal.magnitude > umbralMovimiento)
        {
            if (!audioPasos.isPlaying)
                audioPasos.Play();
        }
        else
        {
            if (audioPasos.isPlaying)
                audioPasos.Stop();
        }
    }
}

