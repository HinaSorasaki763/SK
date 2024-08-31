using System.Collections.Generic;
using UnityEngine;
using CustomUtility;
using System.Linq;

public interface IWeaponBehaviour
{
    void Attack();
    List<AttackTypeTag> Tags { get; }
}

public abstract class WeaponBehaviourBase : IWeaponBehaviour
{
    protected GameObject parentWeapon;
    public abstract List<AttackTypeTag> Tags { get; }
    public abstract void Attack();

    protected WeaponBehaviourBase(GameObject parentWeapon)
    {
        this.parentWeapon = parentWeapon;
    }

    protected void DebugTags() => Utility.DebugTags(Tags);
}

public abstract class RangedWeaponBehaviour : WeaponBehaviourBase
{
    protected IBullet bulletType;

    protected RangedWeaponBehaviour(IBullet bulletType, GameObject parentWeapon) : base(parentWeapon)
    {
        this.bulletType = bulletType;
        bulletType.SetParent(parentWeapon);
    }

    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Ranged }.Concat(bulletType.Tags).ToList();
}

public class Example : WeaponBehaviourBase
{
    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };

    public Example(GameObject parentWeapon) : base(parentWeapon) { }

    public override void Attack()
    {
        Debug.Log("Example Attack");
        DebugTags();
    }
}

public class LaserAttack : RangedWeaponBehaviour
{
    public LaserAttack(IBullet bulletType, GameObject parentWeapon) : base(bulletType, parentWeapon) { }

    public override void Attack()
    {
        Debug.Log("Laser Attack");
        bulletType.Fire();
        DebugTags();
    }
}

public class SpearAttack : WeaponBehaviourBase
{
    private int dmg;
    private int length;

    public SpearAttack(int dmg, int length, GameObject parentWeapon) : base(parentWeapon)
    {
        this.dmg = dmg;
        this.length = length;
    }

    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };

    public override void Attack()
    {
        Debug.Log("Spear Attack");
        DebugTags();
        PerformSpearAttack();
    }

    private void PerformSpearAttack()
    {
        GameObject spear = ResourcePool.Instance.GetSpearAttack(parentWeapon);
        spear.GetComponent<Spear>().Init(parentWeapon, dmg);
        spear.transform.localScale = new Vector3(length, length, length) * Utility.AttackRangeRatio;
        spear.transform.rotation = parentWeapon.transform.rotation * Quaternion.Euler(0, 0, 270);
    }
}

public class ShotgunAttack : RangedWeaponBehaviour
{
    private int numberOfPellets;
    private float spreadAngle;

    public ShotgunAttack(IBullet bulletType, GameObject parentWeapon, int numberOfPellets, float spreadAngle)
        : base(bulletType, parentWeapon)
    {
        this.numberOfPellets = numberOfPellets;
        this.spreadAngle = spreadAngle;
    }

    public override void Attack()
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
        DebugTags();
    }
}

public class MachineGunAttack : RangedWeaponBehaviour
{
    public MachineGunAttack(IBullet bulletType, GameObject parentWeapon) : base(bulletType, parentWeapon) { }

    public override void Attack()
    {
        Debug.Log("MachineGun Attack");
        bulletType.Fire();
        DebugTags();
    }
}

public class BladeAttack : WeaponBehaviourBase
{
    private int dmg;
    private int length;

    public BladeAttack(int dmg, int length, GameObject parentWeapon) : base(parentWeapon)
    {
        this.dmg = dmg;
        this.length = length;
    }

    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Melee };

    public override void Attack()
    {
        Debug.Log($"Blade Attack length = {length}");
        DebugTags();
        PerformBladeAttack();
    }

    private void PerformBladeAttack()
    {
        GameObject attackRange = ResourcePool.Instance.GetMeleeAttackRange(parentWeapon);
        attackRange.GetComponent<MeleeRange>().Dmg = dmg;
        attackRange.transform.localScale = new Vector3(length, length, length) * Utility.AttackRangeRatio;
        attackRange.transform.rotation = parentWeapon.transform.rotation * Quaternion.Euler(0, 0, -45);
    }
}
