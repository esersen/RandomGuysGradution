using UnityEngine;

public class HotbarControl : MonoBehaviour
{
    [Header("Referanslar")]
    public InventoryUI inventoryUI;
    public JournalSystem journalSystem; // <-- YENİ: Günlük sistemini buraya bağlayacağız

    void Update()
    {
        // KONTROL: Eğer günlük sistemi tanımlıysa VE günlük açıksa, hiçbir şey yapma!
        if (journalSystem != null && journalSystem.isUiOpen)
        {
            return; // Kodu burada kes, aşağıya inme
        }

        // --- Tuş Kontrolleri ---
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventoryUI.SetSelectedIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventoryUI.SetSelectedIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventoryUI.SetSelectedIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventoryUI.SetSelectedIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventoryUI.SetSelectedIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) inventoryUI.SetSelectedIndex(5);
    }
}