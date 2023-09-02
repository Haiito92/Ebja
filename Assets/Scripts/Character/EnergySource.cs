using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Energy Source",  menuName = "Characters/Energy Source")]
public class EnergySource : ScriptableObject
{
    new public string name;
    public string typeName;
    public Color typeColor;
    public Sprite esIcon;

    [TextArea(0,20)]public string esEffect;

    public int lvl;
    public int superpositionLvl;

    public Stat atk;
    public Stat def;
    public Stat hp;


}


