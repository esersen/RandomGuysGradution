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
        }
    }

    void HandleHotbarKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SetSelectedIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SetSelectedIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SetSelectedIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventory.SetSelectedIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) inventory.SetSelectedIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) inventory.SetSelectedIndex(5);
    }
}
