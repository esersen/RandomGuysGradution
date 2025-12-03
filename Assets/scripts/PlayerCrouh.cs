using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Referanslar")]
    public CharacterController controller;
    public Transform cameraTransform;   // FPS kamera

    [Header("Yükseklik ayarları")]
    public float standHeight = 2f;
    public float crouchHeight = 1.2f;

    [Header("Hız ayarları")]
    public float crouchSpeedMultiplier = 0.5f;

    bool isCrouched = false;

    float originalHeight;
    Vector3 originalCenter;
    float originalBottomY;     // alt nokta
    float originalCamLocalY;

    FpsController moveScript;
    float originalMoveSpeed;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        originalHeight = controller.height;
        originalCenter = controller.center;

        // Alt nokta = center.y - height/2
        originalBottomY = originalCenter.y - originalHeight * 0.5f;

        if (cameraTransform != null)
            originalCamLocalY = cameraTransform.localPosition.y;

        moveScript = GetComponent<FpsController>();
        if (moveScript != null)
            originalMoveSpeed = moveScript.moveSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCrouch();
        }
    }

    void ToggleCrouch()
    {
        isCrouched = !isCrouched;

        if (isCrouched)
        {
            // Alt noktayı sabit tutarak height ve center ayarla
            controller.height = crouchHeight;
            float newCenterY = originalBottomY + crouchHeight * 0.5f;
            controller.center = new Vector3(originalCenter.x, newCenterY, originalCenter.z);

            // Kamera biraz aşağı
            if (cameraTransform != null)
            {
                float delta = (standHeight - crouchHeight) * 0.5f;
                cameraTransform.localPosition = new Vector3(
                    cameraTransform.localPosition.x,
                    originalCamLocalY - delta,
                    cameraTransform.localPosition.z
                );
            }

            if (moveScript != null)
                moveScript.moveSpeed = originalMoveSpeed * crouchSpeedMultiplier;
        }
        else
        {
            // Yine alt noktayı sabit tutarak eski yüksekliğe dön
            controller.height = originalHeight;
            float newCenterY = originalBottomY + originalHeight * 0.5f;
            controller.center = new Vector3(originalCenter.x, newCenterY, originalCenter.z);

            if (cameraTransform != null)
            {
                cameraTransform.localPosition = new Vector3(
                    cameraTransform.localPosition.x,
                    originalCamLocalY,
                    cameraTransform.localPosition.z
                );
            }

            if (moveScript != null)
                moveScript.moveSpeed = originalMoveSpeed;
        }
    }
}
