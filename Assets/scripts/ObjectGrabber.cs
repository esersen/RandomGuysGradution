using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Ayarlar")]
    public float grabRange = 3f;
    public float holdDistance = 2f;
    public float moveSpeed = 10f;

    private Rigidbody heldRb;
    private Transform cam;

    int grabbableMask;

    void Start()
    {
        cam = Camera.main.transform;
        grabbableMask = LayerMask.GetMask("Grabbable");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldRb == null)
                TryGrab();
            else
                Drop();
        }

        if (heldRb != null)
            HoldObject();
    }

    void TryGrab()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, grabRange, grabbableMask))
            return;

        // Envanterdeki itemleri grablama
        PickupItem item = hit.collider.GetComponentInParent<PickupItem>();
        if (item != null && !item.gameObject.activeInHierarchy)
            return;

        heldRb = hit.collider.attachedRigidbody;

        if (heldRb != null)
        {
            heldRb.useGravity = false;
            heldRb.linearDamping = 10f;
        }
    }

    void HoldObject()
    {
        Vector3 targetPos = cam.position + cam.forward * holdDistance;
        Vector3 direction = targetPos - heldRb.position;

        heldRb.linearVelocity = direction * moveSpeed;

        // 🔥 BURASI EKLENDİ: DÖNMEYİ ENGELLER
        heldRb.angularVelocity = Vector3.zero;
        heldRb.rotation = Quaternion.identity;
    }

    void Drop()
    {
        if (heldRb != null)
        {
            heldRb.useGravity = true;
            heldRb.linearDamping = 0f;
            heldRb.angularVelocity = Vector3.zero;

            heldRb = null;
        }
    }
}
