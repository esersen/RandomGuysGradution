using UnityEngine;

public class BreakableVase : MonoBehaviour
{
    [Header("Vazo Nesneleri")]
    public GameObject normalVase;
    public GameObject brokenVase;
    public GameObject hiddenItem; // içindeki anahtar vs.

    [Header("Gerekli Item Adı")]
    public string requiredItemName = "Hammer";

    private bool isBroken = false;

    // Hammer seçili mi kontrol et – ItemPickupSystem buradan çağırıyor
    public bool TryBreak(PickupItem selectedItem)
    {
        if (isBroken)
            return false;

        if (selectedItem == null)
        {
            Debug.Log("Hiç item seçili değil.");
            return false;
        }

        if (selectedItem.itemName != requiredItemName)
        {
            Debug.Log("Bu vazo sadece Hammer ile kırılabilir!");
            return false;
        }

        BreakVase();
        return true;
    }

    void BreakVase()
    {
        isBroken = true;

        if (normalVase != null)
            normalVase.SetActive(false);

        if (brokenVase != null)
            brokenVase.SetActive(true);

        if (hiddenItem != null)
            hiddenItem.SetActive(true);

        Debug.Log("Vazo kırıldı!");
    }
}
