using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Bilgileri")]
    public string itemName;
    public int worldIndex;

    [Header("HUD İkonu")]
    public Sprite icon;                  // Envanter slotunda görünen ikon

    [Header("Elde Tutma Ayarları")]
    public GameObject heldPrefab;        // Elde gösterilecek model
    public Vector3 heldPositionOffset;
    public Vector3 heldRotationOffset;

    // --- GÜNLÜK ÖZELLİKLERİ (GÜNCELLENDİ) ---
    [Header("Günlük / Şifre Sistemi")]
    public bool isJournal = false;          // Bu eşya bir günlük mü?
    public string journalPassword = "1234"; // Şifresi ne?
    public bool isLocked = true;            // Kilitli mi?

    // YENİ: Artık tek bir yazı değil, resim sayfaları var
    public Sprite[] journalPages;
    // ---------------------------------------

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