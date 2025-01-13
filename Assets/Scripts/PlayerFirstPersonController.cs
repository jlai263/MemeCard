using UnityEngine;

public class PlayerFirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float verticalRotation = 0f;

    void Start()
    {
        // Lock and hide the cursor when the game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle Player Movement
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move, Space.World);

        // Handle Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f); // Rotate Player horizontally
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Prevent over-rotation
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public void ToggleCursor(bool showCursor)
    {
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
