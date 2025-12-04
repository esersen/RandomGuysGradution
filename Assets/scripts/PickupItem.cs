using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Bilgileri")]
    public string itemName;
    public int worldIndex;

    [Header("HUD İkonu")]
    public Sprite icon;                  // Envanter slotunda görünen PNG ikon

    [Header("Elde Tutma Ayarları")]
    public GameObject heldPrefab;        // Elde gösterilecek model (ayrı prefab)
    public Vector3 heldPositionOffset;   // FPS görünüm pozisyon offset
    public Vector3 heldRotationOffset;   // FPS görünüm rotasyon offset

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    // --------------------------------------------------------
    // Fizik kapatma (envantere alınca)
    // --------------------------------------------------------
    public void DisablePhysics()
    {
        if (rb != null)
            rb.isKinematic = true;

        if (col != null)
            col.enabled = false;
    }

    // --------------------------------------------------------
    // Fizik açma (drop yapınca)
    // --------------------------------------------------------
    public void EnablePhysics()
    {
        if (rb != null)
            rb.isKinematic = false;

        if (col != null)
            col.enabled = true;
    }
}
