using UnityEngine;

public class InventoryHotbarInput : MonoBehaviour
{
    public InventoryUI inventory;
    public Transform dropPoint;

    void Update()
    {
        HandleHotbarKeys();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.DropSelectedItem();
            inventory.UpdateHeldItem(); // item atınca elindeki temizlensin
        }
    }

    void HandleHotbarKeys()
    {
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                inventory.SetSelectedIndex(i);
                inventory.UpdateHeldItem(); // elindeki item güncelle
            }
        }
    }
}
