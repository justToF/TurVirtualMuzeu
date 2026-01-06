using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public Transform playerCamera; // Adaugam asta pentru a seta camera manual

    CharacterController controller;
    Vector3 velocity;
    float rx = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Daca am uitat sa tragem camera in inspector, incearca sa o gaseasca automat
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        if (playerCamera == null) return; // Opreste eroarea daca tot nu gaseste camera

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rx -= mouseY;
        rx = Mathf.Clamp(rx, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(rx, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += -9.81f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}