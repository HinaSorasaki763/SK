using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUtility;

public interface IBullet
{
    void Fire(); // 保留不带方向的方法
    void Fire(Vector3 direction); // 新增带方向的方法
    List<AttackTypeTag> Tags { get; }
    void SetParent(GameObject parent);
}

public class LaserBullet : IBullet
{
    private GameObject parentWeapon;
    public List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Laser };
    public bool continuous;

    public void SetParent(GameObject parent)
    {
        parentWeapon = parent;
    }

    // 默认 Fire 方法，使用当前武器方向
    public void Fire()
    {
        Fire(parentWeapon.transform.right); // 默认方向为武器的右侧（可以根据实际需求修改）
    }

    // 带方向的 Fire 方法
    public void Fire(Vector3 direction)
    {
        Transform firepoint = parentWeapon.transform.Find("FirePoint");
        if (firepoint == null)
        {
            Debug.LogError("Firepoint not found on parent weapon");
            return;
        }

        GameObject laser = ResourcePool.Instance.GetLaser(parentWeapon);
        laser.transform.position = firepoint.position;

        // 根据传入的方向设置激光的旋转
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log($"Laser rotation set to: {laser.transform.rotation.eulerAngles}");

        Laser bulletComponent = laser.GetComponent<Laser>();
        if (bulletComponent != null)
        {
            bulletComponent.Initialize(parentWeapon, continuous);
        }

        Debug.Log($"Firing {(continuous ? "continuous" : "short-term")} laser from {parentWeapon} in direction {direction} with rotation angle {angle}");
    }
}



public class StandardBullet : IBullet
{
    private GameObject parentWeapon;
    public List<AttackTypeTag> Tags => new List<AttackTypeTag> { AttackTypeTag.Bullet };

    public void SetParent(GameObject parent)
    {
        parentWeapon = parent;
    }

    public void Fire()
    {
        Fire(parentWeapon.transform.right);
    }

    public void Fire(Vector3 direction)
    {
        Transform firepoint = parentWeapon.transform.Find("FirePoint");
        if (firepoint == null)
        {
            Debug.LogError("Firepoint not found on parent weapon");
            return;
        }
        GameObject bullet = ResourcePool.Instance.GetClassicBullet(parentWeapon);
        bullet.transform.position = firepoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        ClassicBullet bulletComponent = bullet.GetComponent<ClassicBullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Initialize(direction);
        }

        Debug.Log($"Firing standard bullet from {parentWeapon} in direction {direction}");
    }
}
