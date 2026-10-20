using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "System/Spell")]
public class SpellData : ScriptableObject
{

    public float spellName;
    public float cooldown = 20f;
    public float DMG = 40f;

    public Sprite spellIcon;

    public virtual void Cast(Transform caster)
    {
        Debug.Log($"Rzucono czar: {spellName}");
    }

}