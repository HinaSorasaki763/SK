using System.Collections.Generic;
using UnityEngine;
using CustomUtility;

public interface IBullet
{
    void Fire();
    void Fire(Vector3 direction);
    void EndFire();
    List<AttackTypeTag> Tags { get; }
    void SetParent(GameObject parent);
}

public abstract class BulletBase : IBullet
{
    protected GameObject parentWeapon;
    public GameObject ParentWeapon
    {
        get => parentWeapon;
        set => parentWeapon = value;
    }
    public abstract List<AttackTypeTag> Tags { get; }
    protected abstract GameObject GetBulletPrefab();
    protected abstract void InitializeBullet(GameObject bullet, Vector3 direction);

    public void SetParent(GameObject parent) => parentWeapon = parent;
    public void Fire() => Fire(parentWeapon.transform.right);

    public void Fire(Vector3 direction)
    {
        Transform firepoint = parentWeapon.transform.Find("FirePoint");
        if (firepoint == null)
        {
            Debug.LogError("Firepoint not found on parent weapon");
            return;
        }

        GameObject bullet = GetBulletPrefab();
        bullet.transform.position = firepoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        InitializeBullet(bullet, direction);
    }
    public virtual void EndFire()
    {
        Debug.Log("EndFire called, but not implemented.");
    }
}

public class LaserBullet : BulletBase
{
    public bool continuous;
    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Laser };

    protected override GameObject GetBulletPrefab() => ResourcePool.Instance.GetLaser(parentWeapon);

    protected override void InitializeBullet(GameObject bullet, Vector3 direction)
    {
        bullet.GetComponent<Laser>()?.Initialize(parentWeapon, continuous);
        Debug.Log($"Firing {(continuous ? "continuous" : "short-term")} laser from {parentWeapon} in direction {direction}");
    }
    public override void EndFire()
    {        var laser = parentWeapon.transform.GetComponentInChildren<Laser>();
        if (laser != null)
        {
            laser.EndLaser();
            Debug.Log("Laser fire ended.");
        }
    }
}

public class StandardBullet : BulletBase
{
    public override List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Bullet };

    protected override GameObject GetBulletPrefab() => ResourcePool.Instance.GetClassicBullet(parentWeapon);

    protected override void InitializeBullet(GameObject bullet, Vector3 direction)
    {
        bullet.GetComponent<ClassicBullet>()?.Initialize(direction);
        Debug.Log($"Firing standard bullet from {parentWeapon} in direction {direction}");
    }
}
