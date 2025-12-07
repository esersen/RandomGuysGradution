using UnityEngine;

public class ItemPickupSystem : MonoBehaviour
{
    [Header("Etkileşim Mesafesi")]
    public float interactRange = 5f; // Menzili 5 yaptık ki rahat yetişsin

    [Header("Envanter Referansı")]
    public InventoryUI inventory;

    void Update()
    {
        // E Tuşu: Kapılar (Normal ve Bölüm Sonu)
        if (Input.GetKeyDown(KeyCode.E))
            TryInteractE();

        // F Tuşu: Eşya Alma ve Vazo Kırma
        if (Input.GetKeyDown(KeyCode.F))
            TryInteractF();
    }

    void TryInteractE()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // 1. ÖNCE ÇIKIŞ KAPISINA BAK (LevelExitDoor)
            LevelExitDoor exitDoor = hit.collider.GetComponent<LevelExitDoor>();
            if (exitDoor == null) exitDoor = hit.collider.GetComponentInParent<LevelExitDoor>();

            if (exitDoor != null)
            {
                // Çıkış kapısıysa anahtarı dene
                PickupItem heldItem = inventory.GetSelectedItem();
                exitDoor.TryOpenDoor(heldItem);
                return; // Başka şeye bakma
            }

            // 2. NORMAL KAPIYA BAK (DoorInteract)
            DoorInteract door = hit.collider.GetComponent<DoorInteract>();
            if (door == null) door = hit.collider.GetComponentInParent<DoorInteract>();

            if (door != null)
            {
                // Normal kapıysa aç/kapat
                door.Toggle();
            }
        }
    }

    void TryInteractF()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // 1. VAZO KIRMA
            BreakableVase vase = hit.collider.GetComponent<BreakableVase>();
            if (vase == null) vase = hit.collider.GetComponentInParent<BreakableVase>();

            if (vase != null)
            {
                PickupItem heldItem = inventory.GetSelectedItem();
                vase.TryBreak(heldItem);
                return;
            }

            // 2. EŞYA TOPLAMA
            PickupItem item = hit.collider.GetComponent<PickupItem>();
            if (item == null) item = hit.collider.GetComponentInParent<PickupItem>();

            if (item != null)
            {
                inventory.AddItem(item);
            }
        }
    }
}