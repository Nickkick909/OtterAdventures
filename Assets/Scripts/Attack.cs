using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum HealType
{
    Health,
    Stamina,
    Both
}
[System.Serializable]
public enum AttackType
{
    Light,
    Heavy,
    Utility
};

[System.Serializable]
public class Attack
{
    public string name;
    public string description;


    public int staminaCost;
    public int attackPower;
    public int healPower;
    public AttackType type;
    public int levelUnlocked;


    public string animation;

    public HealType typeHeal;

    public int attackUses;
    public int maxAttackUses;
}
