using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Slot Arka Planları")]
    public Image[] slotImages;

    [Header("Slot İkonları")]
    public Image[] slotIcons;

    [Header("Renk Ayarları")]
    public Color normalColor = Color.gray;
    public Color selectedColor = Color.yellow;

    // Envanter
    private PickupItem[] inventory = new PickupItem[6];
    private int selectedIndex = -1;

    // --- Held Item Sistemi ---
    [Header("Elde Tutulan Eşya")]
    public Transform heldItemPoint;     // Kameraya göre pozisyon alan nokta
    private GameObject heldInstance;    // Ekte görünen model

    void Start()
    {
        RefreshUI();
    }

    // ===================================================================
    // SLOT SEÇME
    // ===================================================================
    public void SetSelectedIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= inventory.Length)
            return;

        selectedIndex = newIndex;

        RefreshUI();
        UpdateHeldItem();
    }

    // ===================================================================
    // ENVANTERE ITEM EKLEME
    // ===================================================================
    public void AddItem(PickupItem item)
    {
        int index = item.worldIndex;

        if (index < 0 || index >= inventory.Length)
            return;

        inventory[index] = item;
        item.gameObject.SetActive(false);
        item.DisablePhysics();

        RefreshUI();
    }

    // ===================================================================
    // ITEM DROPLAMA
    // ===================================================================
    public void DropSelectedItem()
    {
        if (selectedIndex == -1) return;
        if (inventory[selectedIndex] == null) return;

        PickupItem item = inventory[selectedIndex];
        inventory[selectedIndex] = null;

        item.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
        item.transform.rotation = Quaternion.identity;

        item.gameObject.SetActive(true);
        item.EnablePhysics();

        ClearHeld();
        RefreshUI();
    }

    // ===================================================================
    // SEÇİLİ ITEM VER
    // ===================================================================
    public PickupItem GetSelectedItem()
    {
        if (selectedIndex < 0) return null;
        return inventory[selectedIndex];
    }

    // ===================================================================
    // UI GÜNCELLEME
    // ===================================================================
    void RefreshUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color =
                (i == selectedIndex) ? selectedColor : normalColor;

            if (slotIcons != null && i < slotIcons.Length)
            {
                if (inventory[i] == null)
                {
                    slotIcons[i].enabled = false;
                }
                else
                {
                    slotIcons[i].enabled = true;
                    slotIcons[i].sprite = inventory[i].icon;
                }
            }
        }
    }

    // ===================================================================
    // ELDESİNDE GÖRÜNECEK ITEMİ OLUŞTUR
    // ===================================================================
    public void UpdateHeldItem()
    {
        ClearHeld();

        PickupItem item = GetSelectedItem();
        if (item == null) return;

        // Prefab oluştur (child yapmıyoruz!)
        heldInstance = Instantiate(item.heldPrefab);

        // Fizik kapat
        if (heldInstance.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
        if (heldInstance.TryGetComponent(out Collider col)) col.enabled = false;

        heldInstance.SetActive(true);
    }

    // ===================================================================
    // ELDEN SİL
    // ===================================================================
    public void ClearHeld()
    {
        if (heldInstance != null)
            Destroy(heldInstance);
    }

    // ===================================================================
    // 🔥 FPS STYLE: Item kamerayı takip eder ama dönmez
    // ===================================================================
    void LateUpdate()
    {
        if (heldInstance == null) return;

        PickupItem item = GetSelectedItem();
        if (item == null) return;

        Transform cam = Camera.main.transform;

        // hedef pozisyon
        Vector3 targetPos =
            cam.position +
            cam.forward * 0.55f +
            cam.right * item.heldPositionOffset.x +
            cam.up * item.heldPositionOffset.y;

        heldInstance.transform.position = Vector3.Lerp(
            heldInstance.transform.position,
            targetPos,
            Time.deltaTime * 12f
        );

        // hedef rotasyon = kamera rotasyonu + offset
        Quaternion targetRot =
            cam.rotation *
            Quaternion.Euler(item.heldRotationOffset);

        heldInstance.transform.rotation = Quaternion.Slerp(
            heldInstance.transform.rotation,
            targetRot,
            Time.deltaTime * 12f
        );
    }
}
