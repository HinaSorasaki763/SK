using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/weapon")]

public class WeaponScriptableObject : ScriptableObject
{
    public GameObject WeaponObject;
    public string WeaponName;
    public int index;
    public int attackRange;
    public float cooldownTime;
    public int damage;
    public int manaCost;
    public bool isMelee;
}
