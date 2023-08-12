using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySystem
{
    public Dictionary<ItemData, Item> itemDictionary = new Dictionary<ItemData, Item>();
    public List<Item> inventory;

    GameManager gameManager;
    GameObject inventoryUI = null;

    List<Button> itemInInventoryBtns;

    Button equipBtn;

    public InventorySystem()
    {
        itemInInventoryBtns = new List<Button>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        itemDictionary = new Dictionary<ItemData, Item>();
        inventory = new List<Item>();
        
    }
    public void AddToInventory(ItemData _itemData)
    {

        if (itemDictionary.TryGetValue(_itemData, out Item item))
        {
            Debug.Log("item added to stack");
            item.AddToStack();
        }

        else
        {
            Debug.Log("new item created");
            Item newItem = new Item(_itemData);
            inventory.Add(newItem);
            itemDictionary.Add(_itemData, newItem);
        }
    }

    public void RemoveFromInventory(ItemData inventoryData)
    {
        if(itemDictionary.TryGetValue(inventoryData, out Item item))
        {
            item.RemoveFromStack();
        }

        if(item.stackSize == 0)
        {
            Button btnFound = null;
            foreach(Button btns in itemInInventoryBtns)
            {
                if(btns.name == item.itemData.displayName)
                {
                    btnFound = btns;
                    GameObject.Destroy(btns.gameObject);                   
                }
            }
            itemInInventoryBtns.Remove(btnFound);
            inventory.Remove(item);
            itemDictionary.Remove(inventoryData);
        }      
    }


    public void UpdateInventory(GameObject _inventoryUI, bool displayMode)
    {
        if (inventoryUI == null)
        {
            inventoryUI = _inventoryUI;
        }

        if (displayMode)
        {
            Transform inventoryContainer = _inventoryUI.transform.Find("InventoryPanel").
                transform.Find("ActiveInventory").
                transform.Find("InventoryContainer").gameObject.transform;


            foreach (Item item in inventory)
            {
                Button btn = GenerateItemButton(item);
                Text stackText = GenerateStackText(item, btn);
                btn.transform.SetParent(inventoryContainer);
                btn.transform.localScale = Vector3.one;
            }
        }
        else
        {
            foreach(Button btn in itemInInventoryBtns)
            {
                Debug.Log("destroying item");
                GameObject.Destroy(btn.gameObject);
            }
            itemInInventoryBtns.Clear();
        }
        
    }

    private Button GenerateItemButton(Item item)
    {
        GameObject itemBtn = new GameObject(item.itemData.displayName);
        itemBtn.AddComponent<RectTransform>();
        Button btn = itemBtn.AddComponent<Button>();
        Image icon = itemBtn.AddComponent<Image>();
        icon.sprite = item.itemData.Icon;
        btn.targetGraphic = icon;
        btn.onClick.AddListener(delegate { ShowInventoryInfo(item,true); });
        itemInInventoryBtns.Add(btn);
        return btn;
    }

    private Text GenerateStackText(Item item, Button parent)
    {
        GameObject stackSizeText = new GameObject("AmountText");
        Text stackText = stackSizeText.AddComponent<Text>();
        stackText.font = Font.CreateDynamicFontFromOSFont("Arial", 1);
        stackText.fontSize = 45;
        stackSizeText.transform.SetParent(parent.transform);
        RectTransform textRect = stackSizeText.GetComponent<RectTransform>();
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        stackText.text = item.stackSize.ToString();
        return stackText;
    }
    public void ShowInventoryInfo(Item item, bool displayMode)
    {
        GameObject InfoPanel = inventoryUI.transform.Find("InventoryPanel").transform.Find("InfoPanel").transform.Find("infoPanel").gameObject;
        equipBtn = InfoPanel.transform.Find("EquipBtn").GetComponent<Button>();
        Image icon = InfoPanel.transform.Find("ItemImage").GetComponent<Image>();
        TextMeshProUGUI itemNameText = InfoPanel.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemDescription = InfoPanel.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();

        if (displayMode)
        {
            switch (item.itemData.ItemType)
            {
                case ItemData.itemType.ARTEFACT:
                    equipBtn.interactable = false;
                    break;
                case ItemData.itemType.EQUIPABLE:
                    equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                    equipBtn.interactable = true;
                    equipBtn.onClick.RemoveAllListeners();
                    equipBtn.onClick.AddListener(delegate { EquipItem(item); });

                    break;
                case ItemData.itemType.USABLE:
                    equipBtn.interactable = true;
                    equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
                    equipBtn.onClick.RemoveAllListeners();
                    equipBtn.onClick.AddListener(delegate { UseItem(item); });
                    break;
                default:
                    break;
            }

            icon.sprite = item.itemData.Icon;
            itemNameText.text = item.itemData.displayName;
            string itemDescriptionText = "";
            foreach (StatToEnhance stat in item.itemData.statToEnhance)
            {
                string tempStat = stat.statToEnhance.ToString() + " : " + stat.statEnhance.ToString();
                itemDescriptionText += tempStat + "\n";
            }
            itemDescription.text = itemDescriptionText;

            Debug.Log("Item Selected : " + item.itemData.name);
        }

        else
        {
            equipBtn.interactable = false;
            icon.sprite = null;
            itemNameText.text = "";
            itemDescription.text = "";
        }        
    }

    void EquipItem(Item item)
    {
        equipBtn.interactable = false;
        Button spotBtn = null;
        TextMeshProUGUI spotText = null;
        GameObject equippedPanel = inventoryUI.transform.Find("InventoryPanel").
            transform.Find("EquippedPanel").transform.Find("equippedPanel").gameObject;
        switch (item.itemData.EquipSpot)
        {
            case ItemData.equipSpot.HAND:
                spotBtn = equippedPanel.transform.Find("Hand").transform.Find("Button").GetComponent<Button>();
                spotText = spotBtn.GetComponentInChildren<TextMeshProUGUI>();              
                break;
            case ItemData.equipSpot.MELEE:
                spotBtn = equippedPanel.transform.Find("Melee").transform.Find("Button").GetComponent<Button>();
                spotText = spotBtn.GetComponentInChildren<TextMeshProUGUI>();
                break;
            case ItemData.equipSpot.RING:
                spotBtn = equippedPanel.transform.Find("Ring").transform.Find("Button").GetComponent<Button>();
                spotText = spotBtn.GetComponentInChildren<TextMeshProUGUI>();
                break;
            case ItemData.equipSpot.ARMOUR:
                spotBtn = equippedPanel.transform.Find("Armour").transform.Find("Button").GetComponent<Button>();
                spotText = spotBtn.GetComponentInChildren<TextMeshProUGUI>();
                break;
        }
        spotBtn.onClick.AddListener(delegate { UnEquipItem(spotBtn, spotText, item); });
        spotBtn.gameObject.GetComponent<Image>().sprite = item.itemData.Icon;
        spotText.text = item.itemData.displayName;
        gameManager.equippedItems.Add(item.itemData); // adding to equipped items
        gameManager.UpdateStats(item,true);
        item.RemoveFromStack();
       // UpdateInventory(inventoryUI,true);
        Debug.Log(item.itemData.displayName + " equipped");
    }

    void UnEquipItem(Button spot, TextMeshProUGUI text, Item item)
    {
        equipBtn.interactable = true;
        gameManager.UpdateStats(item, false);
        spot.onClick.RemoveAllListeners();
        spot.gameObject.GetComponent<Image>().sprite = null;
        text.text = "";
        item.AddToStack();
    }

    void UseItem(Item item)
    {
        if(item.itemData.displayName.Contains("Potion"))
        {
            PlayerHealthXP.Instance.IncreaseHealth(item.itemData.statToEnhance[0].statEnhance);
            Debug.Log("Potions used");
            ShowInventoryInfo(item, false);
            RemoveFromInventory(item.itemData);
        }
        Debug.Log(item.itemData.displayName + " used");
    }
}

[System.Serializable]
public class Item
{
    public ItemData itemData;
    public int stackSize;

    public Item(ItemData _itemData)
    {
        itemData = _itemData;
        AddToStack();
    }

    public void AddToStack() => stackSize++;

    public void RemoveFromStack() => stackSize--;
    
}
