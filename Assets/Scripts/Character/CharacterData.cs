using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/Character")]
public class CharacterData : ScriptableObject
{
    [Header("Main Infos")]
    [Space]

    new public string name;
    [TextArea(0,10)]public string description;
    public Sprite spriteCombat;
    public Sprite spriteMenu;
    public Sprite portrait;

    public Tribe tribe;

    public int lvl;
    public int elevationLvl;

    public Stat atk;
    public Stat def;
    public Stat hp;
    public Stat speed;
    public Stat critChance;
    public Stat critDmg;

    [Space]
    [Header("EnergySource Infos")]
    [Space]

    public EnergySource energySource;

}

public enum Tribe { Liberty, Purity, Knowledge, Order }
