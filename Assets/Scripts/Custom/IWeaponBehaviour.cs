using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtility;
using Unity.VisualScripting;

public interface IWeaponBehaviour
{
    void Attack();
}
public class Example :IWeaponBehaviour
{
    public List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };
    public void Attack()
    {
        Debug.Log("Example Attack");
        Utility.DebugTags(Tags);
    }

}
public class LaserAttack : IWeaponBehaviour
{
    private IBullet bulletType;
    private GameObject parentWeapon;
    public List<AttackTypeTag> Tags
    {
        get
        {
            var tags = new List<AttackTypeTag> { AttackTypeTag.Ranged };
            tags.AddRange(bulletType.Tags);
            return tags;
        }
    }
    public LaserAttack(IBullet bulletType, GameObject parentWeapon)
    {
        this.bulletType = bulletType;
        this.parentWeapon = parentWeapon;
        this.bulletType.SetParent(parentWeapon);
    }

    public void Attack()
    {
        Debug.Log("Laser Attack");
        bulletType.Fire();
        Utility.DebugTags(Tags);
    }
}
public class SpearAttack : IWeaponBehaviour
{
    private int dmg;
    private int length;
    private GameObject parentWeapon;
    public List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };
    public SpearAttack(int dmg, int length, GameObject parentWeapon)
    {
        this.dmg = dmg;
        this.length = length;
        this.parentWeapon = parentWeapon;
    }

    public void Attack()
    {
        Debug.Log("Spear Attack");
        Utility.DebugTags(Tags);
        PerformspearAttack();
    }
    private void PerformspearAttack()
    {
        GameObject spear =  ResourcePool.Instance.GetSpearAttack(parentWeapon);
        spear.GetComponent<Spear>().Init(parentWeapon,dmg);
        spear.transform.localScale = new Vector3(length, length, length) * Utility.AttackRangeRatio;
        spear.transform.rotation = parentWeapon.transform.rotation * Quaternion.Euler(0, 0, 270);
    }

}
public class ShotgunAttack : IWeaponBehaviour
{
    private IBullet bulletType;
    private GameObject parentWeapon;
    private int numberOfPellets = 5;
    private float spreadAngle = 30f;

    public ShotgunAttack(IBullet bulletType, GameObject parentWeapon,int numberOfPellets,float spreadAngle)
    {
        this.bulletType = bulletType;
        this.parentWeapon = parentWeapon;
        this.numberOfPellets = numberOfPellets;
        this.spreadAngle = spreadAngle;
        this.bulletType.SetParent(parentWeapon);
    }

    public List<AttackTypeTag> Tags
    {
        get
        {
            var tags = new List<AttackTypeTag> { AttackTypeTag.Ranged };
            tags.AddRange(bulletType.Tags);
            return tags;
        }
    }

    public void Attack()
    {
        Debug.Log($"Shotgun Attack, numberOfPellets = {numberOfPellets}");
        for (int i = 0; i < numberOfPellets; i++)
        {
            float angleOffset = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 direction = Quaternion.Euler(0, 0, angleOffset) * parentWeapon.transform.right;
            Debug.Log($"Bullet {i} direction: {direction}");
            bulletType.Fire(direction);
            Debug.Log($"Firing {i} bullet, bullet type = {bulletType}");
        }
        Utility.DebugTags(Tags);
    }

}

public class MachineGunAttack : IWeaponBehaviour
{
    private IBullet bulletType;
    private GameObject parentGameobject;
    public MachineGunAttack(IBullet bulletType,GameObject parent)
    {
        this.bulletType = bulletType;
        this.parentGameobject = parent;
        this.bulletType.SetParent(parent);
    }

    public List<AttackTypeTag> Tags
    {
        get
        {
            var tags = new List<AttackTypeTag> { AttackTypeTag.Ranged };
            tags.AddRange(bulletType.Tags);
            return tags;
        }
    }
    public void Attack()
    {
        Debug.Log("MachineGun Attack");
        bulletType.Fire();
        Utility.DebugTags(Tags);
    }
}
public class BladeAttack : IWeaponBehaviour
{
    private int dmg;
    private int length;
    private GameObject parentWeapon;
    public List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };
    public BladeAttack(int length,int dmg, GameObject parentWeapon)
    {
        this.length = length;
        this.parentWeapon = parentWeapon;
        this.dmg = dmg;
    }
    public void Attack()
    {
        Debug.Log($"Blade Attack length = {length}");
        Utility.DebugTags(Tags);
        PerformBladeAttack();
    }
    private void PerformBladeAttack()
    {
        GameObject attackRange = ResourcePool.Instance.GetMeleeAttackRange(parentWeapon);
        attackRange.GetComponent<MeleeRange>().Dmg = dmg;
        attackRange.transform.localScale = new Vector3(length,length,length)*Utility.AttackRangeRatio;
        attackRange.transform.rotation = parentWeapon.transform.rotation * Quaternion.Euler(0, 0, -45);
    }
}