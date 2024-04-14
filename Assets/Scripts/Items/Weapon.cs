using CustomUtility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : Interactable
{
    public void Start()
    {
        
    }
    public int index;
    private List<IWeaponBehaviour> behaviours = new List<IWeaponBehaviour>();
    public void AddBehavior(IWeaponBehaviour behaviour)
    {
        behaviours.Add(behaviour);
    }

    public void PerformAttack()
    {
        Debug.Log($"Performing Attack , behaviours count = {behaviours.Count}");
        foreach (var behavior in behaviours)
        {
            behavior.Attack();
        }
    }
    public override void Interact()
    {
        PlayerAttack.Instance.PickupWeapon(this);
    }
}
public class ClassicMachineGun : Weapon
{
    public ClassicMachineGun()
    {
        index = 1;
        AddBehavior(new MachineGunAttack());
        Debug.Log("Constructor Called");
    }
}
public class ClassicShotGun : Weapon
{
    public ClassicShotGun()
    {
        index = 2;
        AddBehavior(new ShotgunAttack(new StandardBullet()));
        Debug.Log("Constructor Called");
    }
}
public class ClassicBlade :Weapon
{
    public ClassicBlade()
    {
        index = 3;
        AddBehavior(new BladeAttack());
        Debug.Log("Constructor Called");
    }
}
public class ClassicSpear : Weapon
{
    public ClassicSpear()
    {
        index = 4;
        AddBehavior(new SpearAttack());
        Debug.Log("Constructor Called");
    }
}
public class ClassicLaser : Weapon
{
    public ClassicLaser()
    {
        index = 5;
        AddBehavior(new LaserAttack());
        Debug.Log("Constructor Called");
    }
}
public class ShotgunLaser : Weapon
{
    public ShotgunLaser()
    {
        index = 6;
        AddBehavior(new ShotgunAttack(new LaserBullet()));
        Debug.Log("Constructor Called");
    }
}
public class LaserSpear : Weapon
{
    public LaserSpear()
    {
        index = 7;
        AddBehavior(new LaserAttack());
        AddBehavior(new SpearAttack());
        Debug.Log("Constructor Called");
    }
}