using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerClassHandler :MonoBehaviour
{

    private Player player;

    [Header("Class and tree system")]
    public CharacterClass currentClass;
    public List<ClassUpgradeNode> unlockedNodes = new List<ClassUpgradeNode>();
    public int availableSkillPoints = 0;


    [Header("Equipment")]
    public WeaponData currentWeapon;
    public List<SpellData> equippedSpells = new List<SpellData>();


    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        if (currentClass != null)
        {
            InitializeClass(currentClass);
        }
    }

    public void InitializeClass(CharacterClass newClass)
    {
        currentClass = newClass;
        unlockedNodes.Clear();

        currentWeapon = newClass.defaultWeapon;
        equippedSpells = new List<SpellData>(newClass.defaultSpells);

        RecalculatePlayerStats();

        player.SetCurrentHp(player.MaxHp);
    }

    private void RecalculatePlayerStats()
    {
        if (currentClass == null || player == null) return;

        float totalHealthBonus = 0f;
        float totalSpeedBonus = 0f;
        float totalAttackBonus = 0f;
        float totalDefensBonus = 0f;

        foreach(var node in unlockedNodes)
        {
            totalHealthBonus += node.MaxHPbonus;
            totalDefensBonus += node.DEFbonus;
            totalAttackBonus += node.DMGbonus;
            totalSpeedBonus += node.SPEEDbonus;
        }

        float weaponBonus = (currentWeapon != null) ? currentWeapon.baseDMG : 0f;

        float finalMaxHp = currentClass.baseMaxHP + totalHealthBonus;
        float finalSpeed = currentClass.baseSPEED + totalSpeedBonus;
        float finalDmg = currentClass.baseDMG + totalAttackBonus + weaponBonus;
        float finalDef = currentClass.baseDEF + totalDefensBonus;

        player.SetStats(finalMaxHp, finalDmg, finalSpeed, finalDef);
    }


    public bool CanUnlockNode(ClassUpgradeNode node)
    {
        if (unlockedNodes.Contains(node)) return false;
        if (availableSkillPoints < node.costInPoints) return false;

        foreach(var req in node.prerequisites)
        {
            if (!unlockedNodes.Contains(req)) return false;
        }

        return true;
    }

    public bool UnlockNode(ClassUpgradeNode node)
    {
        if(!CanUnlockNode(node)){
            Debug.LogWarning("Nie można odblokować tego węzła");
            return false;
        }

        availableSkillPoints -= node.costInPoints;
        unlockedNodes.Add(node);

        if(node.unlockedSpell != null && !equippedSpells.Contains(node.unlockedSpell))
        {
            if(equippedSpells.Count < currentClass.maxSpellSlots)
            {
                equippedSpells.Add(node.unlockedSpell);
            }
            else
            {
                Debug.Log("Brak wolnych slotów na czary! Czar odblokowany, ale nie przypisany");
            }
        }
        if(node.unlockedWeapon != null)
        {
            currentWeapon = node.unlockedWeapon;
        }

        RecalculatePlayerStats();

        Debug.Log($"Odblokowano talent w drzewku: {node.nodeName}");
        return true;


    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        RecalculatePlayerStats();
    }

    public bool EquipSpell(SpellData newSpell, int slotIndex)
    {
        if(slotIndex >= 0 && slotIndex < currentClass.maxSpellSlots)
        {
            if (slotIndex < equippedSpells.Count) equippedSpells[slotIndex] = newSpell;
            else equippedSpells.Add(newSpell);

            return true;
        }

        return false;
    }
}