using UnityEngine;

public class AlertaEmergencia : MonoBehaviour
{
    public AudioSource sonidoEmergencia;
    public Light luzRoja; // Arrastra aquí la luz roja dentro de "caja de luces"
    public string tagJugador = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador) && luzRoja.enabled)
        {
            if (!sonidoEmergencia.isPlaying)
                sonidoEmergencia.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
        {
            if (sonidoEmergencia.isPlaying)
                sonidoEmergencia.Stop();
        }
    }
}
