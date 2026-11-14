using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "System/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float baseDMG = 5f;
    public float attackSpeed = 1.2f;

    public Sprite weaponIcon;

    public bool isUnlocked;

    public int price;

    public string description;



    [Header("Wymagania klas")]
    [Tooltip("Lista klas, które mogą używać tego czaru. Jeśli lista jest pusta, każdy może go użyć.")]
    public List<CharacterClass> allowedClasses = new List<CharacterClass>();


}