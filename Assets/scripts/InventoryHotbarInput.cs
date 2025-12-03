using UnityEngine;

public class InventoryHotbarInput : MonoBehaviour
{
    private InventoryUI inventory;

    void Awake()
    {
        // Aynı objede duran InventoryUI component'ini bul
        inventory = GetComponent<InventoryUI>();
    }

    void Update()
    {
        if (inventory == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventory.SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventory.SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) inventory.SelectSlot(5);
    }
}
