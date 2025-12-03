using UnityEngine;

public class ItemPickupSystem : MonoBehaviour
{
    [Header("Etkileşim Mesafesi")]
    public float interactRange = 3f;

    [Header("Envanter Referansı")]
    public InventoryUI inventory;

    void Update()
    {
        // E → Kapı / Dolap
        if (Input.GetKeyDown(KeyCode.E))
            TryInteractDoor();

        // F → Item alma + Vazo kırma
        if (Input.GetKeyDown(KeyCode.F))
            TryInteractF();
    }

    // ---------------------------------------------------------
    // F tuşu → önce Vazo kırmayı kontrol eder, sonra item alır
    // ---------------------------------------------------------
    void TryInteractF()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange))
            return;

        // -----------------------------------------------------
        // 1) VAZO KIRMA KONTROLÜ (ÖNCELİK)
        // -----------------------------------------------------
        BreakableVase vase = hit.collider.GetComponent<BreakableVase>();
        if (vase == null)
            vase = hit.collider.GetComponentInParent<BreakableVase>();

        if (vase != null)
        {
            PickupItem heldItem = inventory.GetSelectedItem();

            // Hammer seçili değilse kırmaz
            if (vase.TryBreak(heldItem))
                return;

            return; // Hammer yok → item alma kısmına geçilmez
        }

        // -----------------------------------------------------
        // 2) ITEM ALMA
        // -----------------------------------------------------
        TryPickupItem(hit);
    }

    // ---------------------------------------------------------
    // ITEM ALMA
    // ---------------------------------------------------------
    void TryPickupItem(RaycastHit hit)
    {
        PickupItem item = hit.collider.GetComponent<PickupItem>();

        if (item == null)
            item = hit.collider.GetComponentInParent<PickupItem>();

        if (item == null)
            return;

        inventory.AddItem(item);
    }

    // ---------------------------------------------------------
    // E tuşu → Kapı veya dolap açma / kapama
    // ---------------------------------------------------------
    void TryInteractDoor()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange))
            return;

        DoorInteract door = hit.collider.GetComponent<DoorInteract>();
        if (door == null)
            door = hit.collider.GetComponentInParent<DoorInteract>();

        if (door != null)
            door.Toggle();
    }
}
