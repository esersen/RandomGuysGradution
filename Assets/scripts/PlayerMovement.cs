using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsController : MonoBehaviour
{
    [Header("Hareket")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpForce = 3f;

    [Header("Mouse Ayarları")]
    public float mouseSensitivity = 100f;
    public Transform cam;   // Kamerayı buraya sürükle

    private CharacterController controller;
    private float yVelocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Mouse'u ekrana kilitle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // === MOUSE İLE ETRAFINA BAKMA ===
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yatay dönme (sağa sola) - karakteri döndürür
        transform.Rotate(Vector3.up * mouseX);

        // Dikey dönme (yukarı aşağı) - sadece kamerayı döndürür
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Yukarı-aşağı bakma sınırı
        cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // === KLAVYE İLE YÜRÜME (WASD) ===
        float h = Input.GetAxisRaw("Horizontal"); // A-D
        float v = Input.GetAxisRaw("Vertical");   // W-S

        Vector3 move = transform.right * h + transform.forward * v;
        move = move.normalized * moveSpeed;

        // === YER ÇEKİMİ + ZIPLAMA ===
        if (controller.isGrounded)
        {
            yVelocity = -1f; // Yere yapışık tutsun
            if (Input.GetButtonDown("Jump")) // Space
            {
                yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
