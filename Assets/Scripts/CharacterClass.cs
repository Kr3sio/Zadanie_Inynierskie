using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "System/ClassTree/CharacterClass")]

public class CharacterClass : ScriptableObject
{
    public string className;
    [TextArea] public string description;

    [Header("Base Statistics")]
    public float baseMaxHP;
    public float baseDMG;
    public float baseDEF;
    public float baseSPEED;

    [Header("Development tree")]
    public List<ClassUpgradeNode> allClassNodes = new List<ClassUpgradeNode>();

    [Header("Base equipment")]
    public WeaponData defaultWeapon;
    public List<SpellData> defaultSpells = new List<SpellData>();
    public int maxSpellSlots = 2;





}
