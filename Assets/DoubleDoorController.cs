using UnityEngine;

public class DoubleDoorController : MonoBehaviour
{
    public Transform doorLeft;
    public Transform doorRight;

    public float openDistance = 2f;
    public float speed = 2f;

    private Vector3 leftClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightClosedPos;
    private Vector3 rightOpenPos;

    private bool isOpen = false;

    void Start()
    {
        leftClosedPos = doorLeft.position;
        rightClosedPos = doorRight.position;

        leftOpenPos = leftClosedPos + Vector3.left * openDistance;
        rightOpenPos = rightClosedPos + Vector3.right * openDistance;
    }

    void Update()
    {
        if (isOpen)
        {
            doorLeft.position = Vector3.MoveTowards(doorLeft.position, leftOpenPos, speed * Time.deltaTime);
            doorRight.position = Vector3.MoveTowards(doorRight.position, rightOpenPos, speed * Time.deltaTime);
        }
        else
        {
            doorLeft.position = Vector3.MoveTowards(doorLeft.position, leftClosedPos, speed * Time.deltaTime);
            doorRight.position = Vector3.MoveTowards(doorRight.position, rightClosedPos, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
        }
    }
}
