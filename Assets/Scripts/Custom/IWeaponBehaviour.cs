using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public interface IWeaponBehaviour
{
    void Attack();
}
public class Example :IWeaponBehaviour
{
    public void Attack()
    {
        Debug.Log("Example Attack");
    }
}
public class LaserAttack : IWeaponBehaviour
{
    public void Attack()
    {
        Debug.Log("Laser Attack");
    }
}
public class SpearAttack : IWeaponBehaviour
{
    public void Attack()
    {
        Debug.Log("Spear Attack");
    }
}
public class ShotgunAttack : IWeaponBehaviour
{
    private IBullet bulletType;

    public ShotgunAttack(IBullet bulletType)
    {
        this.bulletType = bulletType;
    }

    public void Attack()
    {
        Debug.Log("Shotgun Attack");
        bulletType.Fire();
    }
}
public class MachineGunAttack : IWeaponBehaviour
{
    public void Attack()
    {
        Debug.Log("MachineGun Attack");
    }
}
public class BladeAttack : IWeaponBehaviour
{
    public void Attack()
    {
        Debug.Log("Blade Attack");
    }
}