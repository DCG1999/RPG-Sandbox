using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestScript : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;
    GameObject loot;

    string prefabPath;
    public enum LootType { SWORD, POTION, RINGS, ARMOUR};
    public LootType lootType;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();

        switch(lootType)
        {
            case LootType.SWORD:
                prefabPath = "Swords/Sword";
                break;
            case LootType.POTION:
                prefabPath = "Potions/Potion"; ;
                break;
            case LootType.RINGS:
                prefabPath = "Rings/Ring"; 
                break;
            case LootType.ARMOUR:
                prefabPath = "Armour/Armour"; 
                break;
            
        }
    }

    public void OnOpen()
    {
        loot = GenerateLoot();
        loot.transform.position = this.transform.position;
        loot.name = loot.GetComponent<ItemScript>().inventoryData.displayName;

        animator.SetTrigger("openChest");
        Destroy(this.gameObject, 1.5f);

    }

    GameObject GenerateLoot()
    {
        
        float luck = gameManager.playerStats.luck;
        print(luck);

        if (luck < 3)
        {
            if (Random.Range(0, 101) > 95) return GameObject.Instantiate(Resources.Load(prefabPath + " " + Random.Range(1, 4))) as GameObject;

            else return GameObject.Instantiate(Resources.Load(prefabPath + " 1")) as GameObject;
         
        }

        else if (luck <= 6)
        {
            if (Random.Range(0, 101) > 95) return GameObject.Instantiate(Resources.Load(prefabPath + " " + Random.Range(1, 4))) as GameObject;

            else return GameObject.Instantiate(Resources.Load(prefabPath + " 2")) as GameObject;
        }

        else if (luck > 6)
        {
            if (Random.Range(0, 101) > 95) return GameObject.Instantiate(Resources.Load(prefabPath + " " + Random.Range(1, 4))) as GameObject;

            else return GameObject.Instantiate(Resources.Load(prefabPath + " 3")) as GameObject;

        }
        else
        return null;
    }
}
