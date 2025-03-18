using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerBody; // Referencia al cuerpo del jugador
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limita la rotación vertical

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX); // Rota el cuerpo del jugador en el eje Y
    }
}
