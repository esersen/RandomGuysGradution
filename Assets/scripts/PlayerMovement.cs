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

    // 🔥 EKLENDİ — kamera dönmesini kapatmak için:
    [HideInInspector] public bool cameraFreeze = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // === MOUSE KONTROLÜ ===
        if (!cameraFreeze) // 🔥 Sağ tıkla rotate modundayken kamera kilitlenecek
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // === KLAVYE İLE HAREKET ===
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        move = move.normalized * moveSpeed;

        // Yer çekimi + zıplama
        if (controller.isGrounded)
        {
            yVelocity = -1f;
            if (Input.GetButtonDown("Jump"))
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
