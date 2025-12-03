using UnityEngine;

public class ItemPickupSystem : MonoBehaviour
{
    [Header("Ayarlar")]
    public float interactRange = 3f;
    public InventoryUI inventory;

    void Update()
    {
        // E → kapı veya kapak açma
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryDoorInteract();
        }

        // F → item alma veya vazo kırma
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickupOrBreak();
        }
    }

    // -----------------------------------------------------------
    // E TUŞU: KAPI / KAPAK AÇMA
    // -----------------------------------------------------------
    void TryDoorInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange))
            return;

        // Kapı scripti
        DoorInteract door = hit.collider.GetComponentInParent<DoorInteract>();
        if (door != null)
        {
            door.Toggle();
            return;
        }

        // Eğer dolap kapağı için ayrı script varsa, aynısını ekleyebilirsin:
        // CabinetDoor cabinet = hit.collider.GetComponentInParent<CabinetDoor>();
        // if (cabinet != null) { cabinet.Toggle(); return; }
    }

    // -----------------------------------------------------------
    // F TUŞU: ITEM ALMA VE VAZO KIRMA
    // -----------------------------------------------------------
    void TryPickupOrBreak()
    {
        Ray ray = Camera.main.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange))
            return;

        // 1) Kırılabilir vazo
        BreakableVase vase = hit.collider.GetComponentInParent<BreakableVase>();
        if (vase != null)
        {
            vase.Break();
            return;
        }

        // 2) Normal item alma
        PickupItem item = hit.collider.GetComponent<PickupItem>();
        if (item != null)
        {
            if (inventory != null)
                inventory.AddItem(item.icon);

            Destroy(item.gameObject);
        }
    }
}
