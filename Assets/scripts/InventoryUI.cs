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

    [Header("İkon Büyüklük Ayarı")]
    [Range(0.5f, 2f)]
    public float iconScale = 1.2f; // <-- YENİ: Buradan büyüklüğü ayarlayacaksın!

    private PickupItem[] inventory = new PickupItem[6];
    private int selectedIndex = -1;

    [Header("Elde Tutulan Eşya")]
    public Transform heldItemPoint;
    private GameObject heldInstance;

    void Start()
    {
        RefreshUI();
    }

    public void SetSelectedIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= inventory.Length) return;
        selectedIndex = newIndex;
        RefreshUI();
        UpdateHeldItem();
    }

    public void AddItem(PickupItem item)
    {
        int index = item.worldIndex;
        if (index < 0 || index >= inventory.Length) return;

        inventory[index] = item;
        item.gameObject.SetActive(false);
        item.DisablePhysics();

        RefreshUI();
    }

    public void DropSelectedItem()
    {
        if (selectedIndex == -1) return;
        if (inventory[selectedIndex] == null) return;

        PickupItem item = inventory[selectedIndex];
        inventory[selectedIndex] = null;

        if (Camera.main != null)
        {
            item.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1.5f;
            item.transform.rotation = Quaternion.identity;
        }

        item.gameObject.SetActive(true);
        item.EnablePhysics();

        ClearHeld();
        RefreshUI();
    }

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
            if (slotImages[i] != null)
                slotImages[i].color = (i == selectedIndex) ? selectedColor : normalColor;

            if (slotIcons != null && i < slotIcons.Length && slotIcons[i] != null)
            {
                // --- İKON DÜZELTME VE BÜYÜTME ---
                RectTransform iconRect = slotIcons[i].rectTransform;

                // Tam Ortala
                iconRect.anchorMin = Vector2.zero;
                iconRect.anchorMax = Vector2.one;
                iconRect.offsetMin = Vector2.zero;
                iconRect.offsetMax = Vector2.zero;

                // Büyütme Çarpanını Uygula (Yeni Kısım)
                iconRect.localScale = Vector3.one * iconScale;
                // ---------------------------------

                if (inventory[i] == null)
                {
                    slotIcons[i].enabled = false;
                    slotIcons[i].sprite = null;
                }
                else
                {
                    slotIcons[i].enabled = true;
                    slotIcons[i].sprite = inventory[i].icon;
                    slotIcons[i].preserveAspect = true;
                }
            }
        }
    }

    public void UpdateHeldItem()
    {
        ClearHeld();
        PickupItem item = GetSelectedItem();
        if (item == null || item.heldPrefab == null) return;

        heldInstance = Instantiate(item.heldPrefab);

        Rigidbody[] rbs = heldInstance.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs) rb.isKinematic = true;

        Collider[] cols = heldInstance.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols) col.enabled = false;

        heldInstance.SetActive(true);
    }

    public void ClearHeld()
    {
        if (heldInstance != null) Destroy(heldInstance);
    }

    void LateUpdate()
    {
        if (heldInstance == null) return;
        PickupItem item = GetSelectedItem();
        if (item == null) return;

        Transform cam = Camera.main.transform;
        Vector3 targetPos = cam.position + cam.forward * 0.55f + cam.right * item.heldPositionOffset.x + cam.up * item.heldPositionOffset.y;
        heldInstance.transform.position = Vector3.Lerp(heldInstance.transform.position, targetPos, Time.deltaTime * 20f);
        Quaternion targetRot = cam.rotation * Quaternion.Euler(item.heldRotationOffset);
        heldInstance.transform.rotation = Quaternion.Slerp(heldInstance.transform.rotation, targetRot, Time.deltaTime * 20f);
    }
}