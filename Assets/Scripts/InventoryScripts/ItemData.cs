using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite Icon;
    public GameObject prefab;

    public StatToEnhance[] statToEnhance;

    public enum itemType { EQUIPABLE, USABLE, ARTEFACT};
    public itemType ItemType;

    public enum equipSpot {NONE, HAND, MELEE, RING, ARMOUR};
    public equipSpot EquipSpot;
    // add ewhere to equip logic
}

[System.Serializable]
public class StatToEnhance
{
    public enum stat { STRENGHT, AGILITY, STAMINA, LUCK, MAGIC, HEALTH };
    public stat statToEnhance;
    public int statEnhance;
}

