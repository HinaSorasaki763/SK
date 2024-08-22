using CustomUtility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Interactable
{
    public int index;
    private List<IWeaponBehaviour> behaviours = new List<IWeaponBehaviour>();
    public bool InteractionDisabled;
    private float lastAttackTime;
    public WeaponScriptableObject weaponData;

    public virtual void Start()
    {

    }
    public override void Awake()
    {
        Initialize(this.gameObject);
    }
    public virtual void Initialize( GameObject parentWeapon)
    {
        weaponData = ResourcePool.Instance.weaponData[index];
        Debug.Log($"index = {index}");
        Debug.Log($"{weaponData.WeaponName} , {weaponData.damage} , {weaponData.index}");
    }

    public void AddBehavior(IWeaponBehaviour behaviour)
    {
        behaviours.Add(behaviour);
    }

    public void PerformAttack()
    {
        if (Time.time - lastAttackTime < weaponData.cooldownTime)
        {
            Debug.Log($"Weapon {weaponData.WeaponName} is cooling down ,cooldowntime = {weaponData.cooldownTime} , time left {Time.time-lastAttackTime}");
            return;
        }
        Debug.Log($"weapon {weaponData.WeaponName} Performing Attack , behaviours count = {behaviours.Count}");
        foreach (var behavior in behaviours)
        {
            Debug.Log($"behavior =  {behavior}");
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
    public override void Initialize(GameObject parentWeapon)
    {
        base.Initialize(parentWeapon);
        AddBehavior(new BladeAttack(weaponData.attackRange, weaponData.damage, parentWeapon));
    }
}
public class ClassicMachineGun : Weapon
{
    public override void Awake()
    {
        index = 1;
        weaponData = ResourcePool.Instance.weaponData[index];
        StandardBullet standardBullet = new StandardBullet();
        standardBullet.SetParent(this.gameObject);
        AddBehavior(new MachineGunAttack(standardBullet, this.gameObject));
        Debug.Log("Adding ClassicMachineGun");
    }
}

public class ClassicShotGun : Weapon
{
    public override void Awake()
    {
        base.Awake();
        index = 2;
        StandardBullet standardBullet = new StandardBullet();
        standardBullet.SetParent(this.gameObject);
        AddBehavior(new ShotgunAttack(standardBullet, this.gameObject,5,30));
        Debug.Log("Adding ClassicShotGun");
    }
}
public class ClassicBlade : Weapon
{
    public override void Awake()
    {
        index = 3;
        weaponData = ResourcePool.Instance.weaponData[index];
        AddBehavior(new BladeAttack(5, 10, this.gameObject));
        Debug.Log("Adding ClassicBlade");
    }
}
public class ClassicSpear : Weapon
{
    public override void Awake()
    {

        index = 4;
        weaponData = ResourcePool.Instance.weaponData[index];
        AddBehavior(new SpearAttack(5,5,this.gameObject));
        Debug.Log("Adding ClassicSpear");
    }
}

public class ClassicLaser : Weapon
{
    public bool isContinuous = true;
    public override void Awake()
    {
        base.Awake();
        index = 5;
        LaserBullet laserBullet = new LaserBullet
        {
            continuous = isContinuous
        };
        laserBullet.SetParent(this.gameObject);
        AddBehavior(new LaserAttack(laserBullet, this.gameObject));
        Debug.Log("Adding ClassicLaser");
    }
}

public class ShotgunLaser : Weapon
{
    public override void Awake()
    {
        index = 6;
        weaponData = ResourcePool.Instance.weaponData[index];
        LaserBullet laserBullet = new LaserBullet
        {
            continuous = false
        };
        laserBullet.SetParent(this.gameObject);
        AddBehavior(new ShotgunAttack(laserBullet, this.gameObject, 5, 30));
        Debug.Log("Adding ShotgunLaser");
    }
}

public class LaserSpear : Weapon
{
    public override void Awake()
    {

        index = 7;
        weaponData = ResourcePool.Instance.weaponData[index];
        LaserBullet laserBullet = new LaserBullet
        {
            continuous = false
        };
        laserBullet.SetParent(this.gameObject);
        AddBehavior(new LaserAttack(laserBullet,this.gameObject));
        AddBehavior(new SpearAttack(5,5,this.gameObject));
        Debug.Log("Adding LaserSpear");
    }
}