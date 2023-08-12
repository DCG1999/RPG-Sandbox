using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemData inventoryData;
    GameManager gameManager;
    public void OnPickUp()
    {
        Debug.Log(this.gameObject.name + "picked up");
        gameManager = FindObjectOfType<GameManager>();
        InventorySystem inventorySytem = gameManager.inventorySytem;
        inventorySytem.AddToInventory(inventoryData);

        this.gameObject.SetActive(false);
    }
}
