using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    public static WeaponGenerator Instance;
    public WeaponScriptableObject[] weapons = new WeaponScriptableObject[7];

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

    public void Start()
    {
        weapons = Resources.LoadAll<WeaponScriptableObject>("Weapons");
    }

    private Dictionary<int, Func<GameObject, Weapon>> weaponDict = new Dictionary<int, Func<GameObject, Weapon>>()
    {
        {0, (gameObject) => gameObject.AddComponent<ClassicBlade>()},
        {1, (gameObject) => gameObject.AddComponent<ClassicMachineGun>()},
        {2, (gameObject) => gameObject.AddComponent<ClassicShotGun>()},
        {3, (gameObject) => gameObject.AddComponent<ClassicBlade>()},
        {4, (gameObject) => gameObject.AddComponent<ClassicSpear>()},
        {5, (gameObject) => gameObject.AddComponent<ClassicLaser>()},
        {6, (gameObject) => gameObject.AddComponent<ShotgunLaser>()},
        {7, (gameObject) => gameObject.AddComponent<LaserSpear>()},
    };

    public void CreateWeapon(int indx)
    {
        GameObject weaponObject = Instantiate(weapons[indx].WeaponObject);
        Weapon weaponComponent = weaponDict[indx](weaponObject);
        weaponComponent.objectName = weapons[indx].WeaponName;
        Debug.Log("Added Component Type: " + weaponComponent.GetType());
    }

    public void CreateRandWeapon()
    {
        int rand = 6;
      //  int rand = UnityEngine.Random.Range(0, weapons.Length);
        GameObject weaponObject = Instantiate(weapons[rand].WeaponObject);
        Weapon weaponComponent = weaponDict[rand](weaponObject);
        weaponComponent.objectName = weapons[rand].WeaponName;
        Debug.Log("Added Component Type: " + weaponComponent.GetType());
    }
}
