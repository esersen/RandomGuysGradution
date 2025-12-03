using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Image[] slots;  // slotların arka plan Image component'i
    public Image[] icons;  // slot içindeki itemicon Image component'leri

    [Header("Seçili Slot Görünümü")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    public int SelectedIndex { get; private set; } = -1;

    public void AddItem(Sprite sprite)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            // Boş slot: icon henüz aktif değilse
            if (!icons[i].enabled)
            {
                icons[i].sprite = sprite;
                icons[i].enabled = true;

                // İlk item geldiyse otomatik o slota geç
                if (SelectedIndex == -1)
                {
                    SelectSlot(i);
                }

                return;
            }
        }

        Debug.Log("Envanter dolu!");
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        // Önce eski seçili slotun rengini sıfırla
        if (SelectedIndex >= 0 && SelectedIndex < slots.Length)
        {
            slots[SelectedIndex].color = normalColor;
        }

        // Yeni slotu işaretle
        slots[index].color = selectedColor;
        SelectedIndex = index;

        Debug.Log("Slot seçildi: " + (index + 1));
    }
}
