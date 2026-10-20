using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "System/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float baseDMG = 5f;
    public float attackSpeed = 1.2f;
    public GameObject weaponPrefab;


}