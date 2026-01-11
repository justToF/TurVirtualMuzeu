using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public Transform playerCamera;

    [Header("Input lock (set from MuseumInteractor)")]
    public bool inputEnabled = true;

    CharacterController controller;
    Vector3 velocity;
    float rx = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>()?.transform;
    }

    void Update()
    {
        if (!inputEnabled) return;
        if (playerCamera == null) return;

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
