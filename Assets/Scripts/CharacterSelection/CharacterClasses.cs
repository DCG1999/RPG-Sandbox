using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CharacterClass", menuName = "CharClass")]
public class CharacterClasses : ScriptableObject
{
    public enum charClass{ Brawler, Assassin, Wizard, Commoner};
    public charClass Class;

    public string description;

    public int strength;
    public int agilty;
    public int stamina;
    public int luck;
    public int magic;

    public GameObject playerPrefab;
}
