using UnityEngine;

public class LogicaPersonaje1 : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = 9.81f;
    private Rigidbody rb;
    private bool isGrounded;
    public Transform cameraTransform; // Referencia a la cámara

    public static bool isInspecting = false; // Variable para bloquear movimiento
    public static bool isTrading = false; // Variable para bloquear movimiento cuando comerciamos

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que el personaje se incline
    }

    void Update()
    {
        if (isTrading) return;  

        if (!isInspecting) // SOLO SE MUEVE SI NO ESTÁ INSPECCIONANDO
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Mueve el personaje en la dirección en la que mira la cámara
            Vector3 move = cameraTransform.forward * moveZ + cameraTransform.right * moveX;
            move.y = 0; // Evita que el personaje se mueva en el eje Y
            rb.velocity = move.normalized * speed + new Vector3(0, rb.velocity.y, 0);

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            // Si está inspeccionando, solo dejamos gravedad
            rb.velocity = new Vector3(0, rb.velocity.y - gravity * Time.deltaTime, 0);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
