using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "NewNode", menuName = "System/ClassTree/Progress Node")]

public class ClassUpgradeNode : ScriptableObject
{
    public string nodeName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("requirements")]
    public List<ClassUpgradeNode> prerequisites;
    public int costInPoints = 1;

    [Header("Stats bonuses")]
    public float MaxHPbonus;
    public float DMGbonus;
    public float DEFbonus;
    public float SPEEDbonus;

    [Header("Unlocks")]
    public SpellData unlockedSpell;
    public WeaponData unlockedWeapon;


}

