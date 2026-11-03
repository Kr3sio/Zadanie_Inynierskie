using System.Collections.Generic;
using UnityEngine;

public enum SpellType { Projectile, Dash, Shield, StunArea, Heal}

[CreateAssetMenu(fileName = "NewSpell", menuName = "System/Spell")]
public class SpellData : ScriptableObject
{

    public string spellName;
    public float cooldown = 20f;
    public float DMG = 40f;

    [Header("Wymagania klas")]
    [Tooltip("Lista klas, które mogą używać tego czaru. Jeśli lista jest pusta, każdy może go użyć.")]
    public List<CharacterClass> allowedClasses = new List<CharacterClass>();

    [Header("Sprites")]
    public Sprite spellIcon;
    public Sprite projectileSprite;

    [Header("Type and values")]

    public SpellType spellType;
    public float potency; //wartoś mocy zaklęcia
    public float duration; // czas trwania

    //public virtual void Cast(Transform caster)
    //{
    //    Player player = caster.GetComponent<Player>();
    //    PlayerClassHandler classHandler = caster.GetComponent<PlayerClassHandler>();

        
    //}

   
}

