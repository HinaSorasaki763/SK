using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }
    public int maxWeapon = 2;
    private int weaponIndex;
    public List<Weapon> weapons = new();
    public Weapon weaponInHand;
    private GameObject Character;
    private GameObject closestEnemy;
    private Hand handWeapon;
    private bool handInit = false;
    public GameObject handObj;
    public float closetEnemyDistance;
    public float currentCoolDown;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        if (!InitializeCharacter()) return;
        if (!handInit) InitializeHandWeapon();
        RotateWeaponTowardsTarget();
    }

    private bool InitializeCharacter()
    {
        if (Character == null)
        {
            Character = PlayerMove.Instance.SelectedCharacter;
            return Character != null;
        }
        return true;
    }

    private void InitializeHandWeapon()
    {
        handObj = Character.transform.Find("hand").gameObject;
        handWeapon = handObj.AddComponent<Hand>();
        currentCoolDown = handWeapon.weaponData.cooldownTime;
        handInit = true;
    }

    private void RotateWeaponTowardsTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        closestEnemy = FindClosestEnemy(enemies);

        if (enemies.Length == 0)
        {
            RotateWeaponWithDir(PlayerMove.Instance.GetJoystickDir());
        }
        else
        {
            RotateWeapon(closestEnemy.transform.position);
            FlipCharacterAndWeapon(closestEnemy.transform.position);
        }

        AdjustWeaponInHandPosition();
    }

    private GameObject FindClosestEnemy(GameObject[] enemies)
    {
        float minDistance = float.MaxValue;
        closestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, Character.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        closetEnemyDistance = closestEnemy != null ? minDistance : float.MaxValue;
        return closestEnemy;
    }

    public string SwitchWeapon()
    {
        if (weapons.Count <= 1)
        {
            return weaponInHand ? weaponInHand.objectName : "hand";
        }

        weaponIndex = (weaponIndex + 1) % maxWeapon;
        weaponInHand = weapons[weaponIndex];
        weaponInHand.gameObject.SetActive(true);
        currentCoolDown = weaponInHand.weaponData.cooldownTime;
        DisableOtherWeapons(weaponInHand);
        return weaponInHand.objectName;
    }

    private void FlipCharacterAndWeapon(Vector3 targetPosition)
    {
        bool shouldFlip = targetPosition.x >= Character.transform.position.x;
        Vector3 cv = Character.transform.rotation.eulerAngles;
        Vector3 wv = weaponInHand?.transform.rotation.eulerAngles ?? Vector3.zero;
        int z = shouldFlip ? (wv.z <= 90 || wv.z >= 270 ? 180 - (int)wv.z : -180 - (int)wv.z) : (int)wv.z;

        Character.transform.eulerAngles = new Vector3(cv.x, shouldFlip ? 180 : 0, cv.z);
        if (weaponInHand != null)
        {
            weaponInHand.transform.eulerAngles = new Vector3(wv.x, shouldFlip ? 180 : 0, z);
        }
        handObj.transform.eulerAngles = new Vector3(wv.x, shouldFlip ? 180 : 0, z);
    }


    private void RotateWeapon(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - Character.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (weaponInHand != null)
        {
            weaponInHand.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        handObj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void RotateWeaponWithDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (weaponInHand != null)
        {
            weaponInHand.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        handObj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void AdjustWeaponInHandPosition()
    {
        Transform weaponHandle = weaponInHand?.transform.Find("Handle");
        if (weaponHandle != null)
        {
            Vector3 offset = weaponHandle.position - weaponInHand.transform.position;
            weaponInHand.transform.position = Character.transform.position - offset;
        }
    }

    public void Attack()
    {
        if (!weaponInHand.weaponData.isMelee && closetEnemyDistance <= 2)
        {
            handWeapon.PerformAttack();
        }
        else
        {
            weaponInHand?.PerformAttack();
        }
    }

    public void PickupWeapon(Weapon newWeapon)
    {
        Transform weaponHandle = newWeapon.transform.Find("Handle");
        if (weaponHandle == null || Character == null) return;

        if (weapons.Count >= maxWeapon)
        {
            RemoveWeapon(weaponInHand);
            Weapon oldWeapon = weaponInHand;
            oldWeapon.transform.position = newWeapon.transform.position;
            oldWeapon.transform.rotation = Quaternion.identity;
            oldWeapon.ShowIndicator(oldWeapon.transform.position);
            weaponInHand = newWeapon;
            AttachWeaponToCharacter(newWeapon);
            ManageWeaponInventory(newWeapon);
            DisableOtherWeapons(newWeapon);
        }
        else
        {
            AttachWeaponToCharacter(newWeapon);
            ManageWeaponInventory(newWeapon);
            if (weaponInHand == null)
            {
                EquipWeapon(newWeapon, weaponHandle);
            }
            SwitchWeapon();
        }
    }



    private void AttachWeaponToCharacter(Weapon weapon)
    {
        weapon.transform.SetParent(Character.transform);
        AdjustWeaponInHandPosition();
        weapon.detectable = false;
        weapon.HideIndicator();
    }


    private void ManageWeaponInventory(Weapon weapon)
    {
        if (weapons.Count >= maxWeapon)
        {
            RemoveWeapon(weapons[^1]);
        }
        weapons.Add(weapon);
    }

    private void EquipWeapon(Weapon weapon, Transform weaponHandle)
    {
        weaponInHand = weapon;
        AdjustWeaponInHandPosition();
    }

    private void DisableOtherWeapons(Weapon currentWeapon)
    {
        foreach (var weapon in weapons)
        {
            if (weapon != currentWeapon)
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        
        weapon.transform.SetParent(null);
        weapon.detectable = true;
        weapon.ShowIndicator(weapon.transform.position);
    }
}
