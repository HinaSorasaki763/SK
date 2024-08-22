using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }
    public int maxWeapon = 2;
    private int weaponIndex;
    public List<Weapon> weapons;
    public Weapon weaponInHand;
    private GameObject Character;
    private GameObject closestEnemy;
    private Hand handWeapon;
    public SwitchWeapon switchWeapon;
    private bool handInit = false;
    public GameObject handObj;
    public float closetEnemyDistance;
    public float currentCoolDown;
    private void Awake()
    {
        weaponIndex = 0;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Update()
    {
        if (PlayerMove.Instance.SelectedCharacter == null)
        {
            Character = null;
            return;
        }
        if (Character == null)
        {
            Character = PlayerMove.Instance.SelectedCharacter;
        }

        if (Character!= null&&!handInit)
        {
            handObj = Character.transform.Find("hand").gameObject;
            handWeapon = handObj.AddComponent<Hand>();
           // handWeapon.Initialize(handObj);
            currentCoolDown = handWeapon.weaponData.cooldownTime;
            handInit = true;
        }
        RotateWeaponTowardsTarget();
    }

    void RotateWeaponTowardsTarget()
    {
        Transform weaponHandle = weaponInHand?.transform.Find("Handle");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = FindClosestEnemy(enemies);
        if (enemies.Length == 0)
        {
            closetEnemyDistance = float.MaxValue;
            Vector3 vector = PlayerMove.Instance.GetJoysickDir();
            RotateWeaponWithDir(vector);
            AdjustWeaponPosition(weaponHandle);
            return;
        }
        RotateWeapon(closestEnemy.transform.position);
        FlipCharacterAndWeapon(closestEnemy.transform.position);
        AdjustWeaponPosition(weaponHandle);
    }

    GameObject FindClosestEnemy(GameObject[] enemies)
    {
        float minDistance = float.MaxValue;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, Character.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
                closetEnemyDistance = distance;
            }
        }
        return closestEnemy;
    }
    public Vector3 GetRotatePos()
    {
        if (closestEnemy!= null)
        {
            Vector3 v = closestEnemy.transform.position;
            Debug.Log($"Get from enemy : {v}");
            return v;
        }
        else
        {
            Vector3 v = PlayerMove.Instance.GetJoysickDir();
            Debug.Log($"Get from player : {v}");
            return v;
        }
    }
    public string SwitchWeapon()
    {
        if (weapons.Count <= 1)
        {
            if (!weaponInHand)
            {
                return "hand";
            }
            return weaponInHand.objectName;
        }
        weaponIndex++;
        if (weaponIndex >= maxWeapon)
        {
            weaponIndex = 0;
        }
        weaponInHand = weapons[weaponIndex];
        weapons[weaponIndex].gameObject.SetActive(true);
        currentCoolDown = weapons[weaponIndex].weaponData.cooldownTime;
        DisableOtherWeapons(weaponInHand);
        return weaponInHand.objectName;
    }

    void FlipCharacterAndWeapon(Vector3 targetPosition)
    {
        bool shouldFlip = targetPosition.x >= Character.transform.position.x;
        Vector3 cv = Character.transform.rotation.eulerAngles;
        Vector3 wv = weaponInHand?.transform.rotation.eulerAngles ?? Vector3.zero;
        if (shouldFlip)
        {
            int z = 0;
            if (wv.z <= 90 && wv.z > 0)
            {
                z = 180 - (int)wv.z;
            }
            if (wv.z <= 360 && wv.z >= 270)
            {
                z = -180 - (int)wv.z;
            }

            Character.transform.eulerAngles = new Vector3(cv.x, 180, cv.z);
            if (weaponInHand != null)
            {
                weaponInHand.transform.eulerAngles = new Vector3(wv.x, 180, z);
            }
            handObj.transform.eulerAngles = new Vector3(wv.x, 180, z);
        }
        else
        {
            Character.transform.eulerAngles = new Vector3(cv.x, 0, cv.z);
            if (weaponInHand != null)
            {
                weaponInHand.transform.eulerAngles = new Vector3(wv.x, 0, wv.z);
            }
         //   handObj.transform.eulerAngles = new Vector3(wv.x, 180, wv.z);
        }
    }

    void RotateWeapon(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - Character.transform.position;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (weaponInHand != null)
        {
            Vector3 wv = weaponInHand?.transform.rotation.eulerAngles ?? Vector3.zero;
            weaponInHand.transform.eulerAngles = new Vector3(wv.x, wv.y, angle);
        }
        Vector3 hv = handObj?.transform.rotation.eulerAngles ?? Vector3.zero;
        handObj.transform.eulerAngles = new Vector3(hv.x, hv.y, angle);
    }

    void RotateWeaponWithDir(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (weaponInHand != null)
        {
            weaponInHand.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        Vector3 hv = handObj.transform.rotation.eulerAngles;
        handObj.transform.eulerAngles = new Vector3(hv.x, hv.y, angle);
    }

    void AdjustWeaponPosition(Transform weaponHandle)
    {
        if (weaponHandle == null) return;
        Vector3 offset = weaponHandle.position - weaponInHand.transform.position;
        weaponInHand.transform.position = Character.transform.position - offset;
    }

    public void Attack()
    {
        if (weaponInHand == null|| closetEnemyDistance <= 2)
        {
            handWeapon.PerformAttack();
            return;
        }
        weaponInHand.PerformAttack();
    }

    public void PickupWeapon(Weapon weapon)
    {
        Transform weaponHandle = weapon.transform.Find("Handle");
        if (weaponHandle == null || Character == null)
        {
            return;
        }
        AttachWeaponToCharacter(weapon, weaponHandle);
        ManageWeaponInventory(weapon);

        Debug.Log($"Picked up weapon {weapon.name}");
        if (weaponInHand == null)
        {
            EquipWeapon(weapon, weaponHandle);
        }
        SwitchWeapon();
    }

    private void AttachWeaponToCharacter(Weapon weapon, Transform weaponHandle)
    {
        weapon.transform.SetParent(Character.transform);
        Vector3 weaponPositionOffset = weaponHandle.position - weapon.transform.position;
        weapon.transform.position = Character.transform.position - weaponPositionOffset;
        weapon.InteractionDisabled = true;
        weapon.detectable = false;
        weapon.HideIndicator();
    }

    private void ManageWeaponInventory(Weapon weapon)
    {
        if (weapons.Count >= maxWeapon)
        {
            Weapon lastWeapon = weapons[^1];
            Debug.Log($"Dropping weapon {lastWeapon.name} to pick up {weapon.name}");
            RemoveWeapon(lastWeapon);
        }
        weapons.Add(weapon);
    }

    private void EquipWeapon(Weapon weapon, Transform weaponHandle)
    {
        weaponInHand = weapon;
        AdjustWeaponInHandPosition(weaponHandle);
    }

    private void AdjustWeaponInHandPosition(Transform weaponHandle)
    {
        Vector3 positionOffset = weaponHandle.position - weaponInHand.transform.position;
        weaponInHand.transform.position = Character.transform.position - positionOffset;
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
        weapon.InteractionDisabled = false;
        weapon.ShowIndicator(weapon.transform.position);
    }
}
