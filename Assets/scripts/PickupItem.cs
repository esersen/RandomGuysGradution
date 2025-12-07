using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Bilgileri")]
    public string itemName;
    public int worldIndex;

    [Header("HUD İkonu")]
    public Sprite icon;

    [Header("Elde Tutma Ayarları")]
    public GameObject heldPrefab;
    public Vector3 heldPositionOffset;
    public Vector3 heldRotationOffset;

    [Header("Günlük / Şifre Sistemi")]
    public bool isJournal = false;          // Bu eşya bir günlük mü? (Mektup için de bunu seç)
    public string journalPassword = "";     // Şifresi ne? (Mektup için boş bırak)
    public bool isLocked = false;           // Kilitli mi? (Mektup için False yap)

    public Sprite[] journalPages;           // Sayfa resimleri buraya

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void DisablePhysics()
    {
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
    }

    public void EnablePhysics()
    {
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;
    }
}