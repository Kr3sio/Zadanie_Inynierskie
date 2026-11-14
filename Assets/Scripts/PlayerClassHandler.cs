using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerClassHandler :MonoBehaviour
{
    private float[] spellCooldownTimers = new float[2];

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

    private void Update()
    {
        for(int i = 0; i < spellCooldownTimers.Length; i++)
        {
            if (spellCooldownTimers[i]>0) spellCooldownTimers[i] -= Time.deltaTime;
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
        float totalAttackSpeedBonus = 0f;
        float totalDefensBonus = 0f;

        foreach(var node in unlockedNodes)
        {
            totalHealthBonus += node.MaxHPbonus;
            totalDefensBonus += node.DEFbonus;
            totalAttackBonus += node.DMGbonus;
            totalAttackSpeedBonus += node.AttackSpeedbonus;
            totalSpeedBonus += node.SPEEDbonus;
        }

        float weaponDMGBonus = (currentWeapon != null) ? currentWeapon.baseDMG : 0f;
        float weaponAttackSpeedBonus = (currentWeapon != null) ? currentWeapon.attackSpeed : 0f;

        float finalMaxHp = currentClass.baseMaxHP + totalHealthBonus;
        float finalSpeed = currentClass.baseSPEED + totalSpeedBonus;
        float finalDmg = currentClass.baseDMG + totalAttackBonus + weaponDMGBonus;
        float finalAttackSpeed = currentClass.baseAttackSpeed + totalAttackSpeedBonus * weaponAttackSpeedBonus;
        float finalDef = currentClass.baseDEF + totalDefensBonus;

        player.SetStats(finalMaxHp, finalDmg,finalAttackSpeed, finalSpeed, finalDef);
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


    public void CastSpellFromSlot(int slotIndex)
    {
        if(slotIndex >= 0 && slotIndex < equippedSpells.Count)
        {
            SpellData spellToCast = equippedSpells[slotIndex];

            if (spellToCast != null) 
            {
                if (spellCooldownTimers[slotIndex] <= 0)
                {

                    switch (spellToCast.spellType)
                    {
                        case SpellType.Projectile:
                            CastProjectile(spellToCast);
                            break;
                        case SpellType.Dash:
                            CastDash(spellToCast.potency, spellToCast.duration);
                            break;
                        case SpellType.Heal:
                            ExecuteHeal(spellToCast.potency);
                            break;
                        case SpellType.Shield:
                            ExecuteShield(spellToCast.duration);
                            break;
                        case SpellType.StunArea:
                            ExecuteStun();
                            break;
                    }
                    spellCooldownTimers[slotIndex] = spellToCast.cooldown;
                }
                else 
                {
                    Debug.Log($"Czar {spellToCast.spellName} jest jeszcze na cooldownie! Zostało: {spellCooldownTimers[slotIndex]:F1}s");
                    return;
                }
            }
            else
            {
                Debug.Log($"Slot {slotIndex +1} jest pusty!");
            }

        }
        else
        {
            Debug.Log($"Brak przypisanego czaru w slocie {slotIndex + 1}!");
        }
    }

    private void CastProjectile(SpellData spell)
    {
        Debug.Log($"Wystrzelono pocisk: {spell.spellName} z DMG: {spell.DMG}");
    }

    
    public void CastDash(float potency, float duration)
    {
        StartCoroutine(player.DashRoutine(potency, duration));
    }

    private void ExecuteHeal(float potency)
    {
        Debug.Log($"Uleczono gracza o: {potency} HP");
    }

    private void ExecuteShield(float duration)
    {
        Debug.Log($"Aktywowano tarczę na czas: {duration}s");
    }

    private void ExecuteStun()
    {
        Debug.Log($"Wyzowlono obszarowy STUN wokół gracza!");
    }
}