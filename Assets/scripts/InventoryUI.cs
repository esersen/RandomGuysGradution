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

    private PickupItem[] inventory = new PickupItem[6];
    private int selectedIndex = -1;

    void Start()
    {
        RefreshUI();
    }

    public void SetSelectedIndex(int newIndex)
    {
        if (newIndex < 0 || newIndex >= inventory.Length)
            return;

        selectedIndex = newIndex;
        RefreshUI();
    }

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

        RefreshUI();
    }

    public PickupItem GetSelectedItem()
    {
        if (selectedIndex < 0) return null;
        return inventory[selectedIndex];
    }

    void RefreshUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == selectedIndex) ? selectedColor : normalColor;

            if (slotIcons != null && i < slotIcons.Length)
            {
                if (inventory[i] == null)
                {
                    slotIcons[i].enabled = false;
                }
                else
                {
                    slotIcons[i].enabled = true;
                    slotIcons[i].sprite = inventory[i].icon;  // 🔥 PNG ikon HUD’a basılıyor
                }
            }
        }
    }
}
