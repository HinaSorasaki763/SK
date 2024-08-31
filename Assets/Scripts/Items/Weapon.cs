using CustomUtility;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Interactable
{
    public int index;
    protected List<IWeaponBehaviour> behaviours = new List<IWeaponBehaviour>();
    public WeaponScriptableObject weaponData;
    private float lastAttackTime;

    public override void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        weaponData = ResourcePool.Instance.weaponData[index];
        Debug.Log($"{weaponData.WeaponName} , {weaponData.damage} , {weaponData.index}");
    }

    public void AddBehavior(IWeaponBehaviour behaviour) => behaviours.Add(behaviour);

    public void PerformAttack()
    {
        if (Time.time - lastAttackTime < weaponData.cooldownTime)
        {
            Debug.Log($"Weapon {weaponData.WeaponName} is cooling down, time left {Time.time - lastAttackTime}");
            return;
        }

        Debug.Log($"Weapon {weaponData.WeaponName} Performing Attack");
        foreach (var behavior in behaviours)
        {
            behavior.Attack();
        }

        lastAttackTime = Time.time;
    }

    public override void Interact()
    {
        PlayerAttack.Instance.PickupWeapon(this);
    }
}

public class Hand : Weapon
{
    public Hand()
    {
        index = 0;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AddBehavior(new BladeAttack(weaponData.attackRange, weaponData.damage, gameObject));
    }
}

public class ClassicMachineGun : Weapon
{
    public override void Awake()
    {
        index = 1;
        base.Initialize();
        AddBehavior(new MachineGunAttack(new StandardBullet { ParentWeapon = gameObject }, gameObject));
    }
}

public class ClassicShotGun : Weapon
{
    public override void Awake()
    {
        index = 2;
        base.Initialize();
        AddBehavior(new ShotgunAttack(new StandardBullet { ParentWeapon = gameObject }, gameObject, 5, 30));
    }
}

public class ClassicBlade : Weapon
{
    public override void Awake()
    {
        index = 3;
        base.Initialize();
        AddBehavior(new BladeAttack(15, 10, gameObject));
    }
}

public class ClassicSpear : Weapon
{
    public override void Awake()
    {
        index = 4;
        base.Initialize();
        AddBehavior(new SpearAttack(5, 5, gameObject));
    }
}

public class ClassicLaser : Weapon
{
    public bool isContinuous = true;

    public override void Awake()
    {
        index = 5;
        base.Initialize();
        AddBehavior(new LaserAttack(new LaserBullet { continuous = isContinuous, ParentWeapon = gameObject }, gameObject));
    }
}

public class ShotgunLaser : Weapon
{
    public override void Awake()
    {
        index = 6;
        base.Initialize();
        AddBehavior(new ShotgunAttack(new LaserBullet { continuous = false, ParentWeapon = gameObject }, gameObject, 5, 30));
    }
}

public class LaserSpear : Weapon
{
    public override void Awake()
    {
        index = 7;
        base.Initialize();
        AddBehavior(new LaserAttack(new LaserBullet { continuous = false, ParentWeapon = gameObject }, gameObject));
        AddBehavior(new SpearAttack(5, 5, gameObject));
    }
}
