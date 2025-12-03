using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Bilgileri")]
    public string itemName;
    public int worldIndex;
    public Sprite icon;   // 🔥 EKLENDİ — HUD ikonu

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void DisablePhysics()
    {
        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
            col.enabled = false;
    }

    public void EnablePhysics()
    {
        if (rb != null)
            rb.isKinematic = false;

        if (col != null)
            col.enabled = true;
    }
}
