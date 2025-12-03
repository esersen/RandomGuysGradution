using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public enum HingeAxis { X, Y, Z }

    [Header("Açılma Ayarları")]
    public HingeAxis hingeAxis = HingeAxis.Y;
    public float openAngle = -90f;
    public float speed = 5f;
    public bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;

        Vector3 euler = transform.localEulerAngles;

        switch (hingeAxis)
        {
            case HingeAxis.X: euler.x += openAngle; break;
            case HingeAxis.Y: euler.y += openAngle; break;
            case HingeAxis.Z: euler.z += openAngle; break;
        }

        openRotation = Quaternion.Euler(euler);
    }

    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target,
            Time.deltaTime * speed
        );
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }
}
