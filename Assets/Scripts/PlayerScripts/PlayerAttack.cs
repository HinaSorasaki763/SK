using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }
    public int maxWeapon = 2;
    public List<Weapon> weapons;
    public Weapon weaponInHand;
    private GameObject Character;
    private void Awake()
    {
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
        if (Character == null)
        {
            Character = PlayerMove.Instance.SelectedCharacter;
        }
        RotateWeaponTowardsTarget();

    }
    void RotateWeaponTowardsTarget()
    {
        if (weaponInHand == null || Character == null) return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;
        GameObject closestEnemy = FindClosestEnemy(enemies);
        Transform weaponHandle = weaponInHand.transform.Find("Handle");
        FlipCharacterAndWeapon(closestEnemy.transform.position);
        RotateWeapon(weaponHandle, closestEnemy.transform.position);
        AdjustWeaponPosition(weaponHandle);
    }
    GameObject FindClosestEnemy(GameObject[] enemies)
    {
        float minDistance = float.MaxValue;
        GameObject closest = null;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, Character.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }
    void FlipCharacterAndWeapon(Vector3 targetPosition)
    {
        Vector3 scale = Character.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (targetPosition.x >= Character.transform.position.x ? 1 : -1);
        Character.transform.localScale = scale;
        scale = weaponInHand.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (targetPosition.x >= Character.transform.position.x ? 1 : -1);
        weaponInHand.transform.localScale = scale;
    }
    void RotateWeapon(Transform weaponHandle, Vector3 targetPosition)
    {
        targetPosition.z = 0;
        Vector3 direction = targetPosition - Character.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponInHand.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void AdjustWeaponPosition(Transform weaponHandle)
    {
        Vector3 offset = weaponHandle.position - weaponInHand.transform.position;
        weaponInHand.transform.position = Character.transform.position - offset;
    }
    public void Attack()
    {
        if (weaponInHand == null)
        {
            Debug.Log("no weapon");
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
    }

    private void AttachWeaponToCharacter(Weapon weapon, Transform weaponHandle)
    {
        weapon.transform.SetParent(Character.transform);
        Vector3 weaponPositionOffset = weaponHandle.position - weapon.transform.position;
        weapon.transform.position = Character.transform.position - weaponPositionOffset;
        weapon.InteractionDisabled = true;
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
    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        weapon.transform.SetParent(null);
        weapon.InteractionDisabled = false;
        weapon.ShowIndicator(weapon.transform.position);
    }
}
