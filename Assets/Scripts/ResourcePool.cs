using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ResourcePool : MonoBehaviour
{
    public static ResourcePool Instance { get; private set; }

    public WeaponScriptableObject[] weaponData;
    public GameObject indicatorPrefab;
    public GameObject IndicatorTextPrefab;
    public GameObject MeleeAttackRangePrefab;
    public GameObject SpearMeleeAttackPrefab;
    public GameObject ClassicBulletPrefab;
    public GameObject LaserPrefab;
    public GameObject wallPrefab;

    public Transform indicatorParent;
    public Transform indicatorTextParent;
    public Transform MeleeAttackRangeParent;
    public Transform SpearMeleeAttackParent;
    public Transform ClassicBulletParent;
    public Transform LaserParent;
    public Transform wallParent;

    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();
    private Dictionary<GameObject, Transform> parents = new Dictionary<GameObject, Transform>();

    public Transform tempLaserParent;

    private void Awake()
    {
        weaponData = Resources.LoadAll<WeaponScriptableObject>("Weapons");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializePool(indicatorPrefab, indicatorParent, 10);
            InitializePool(IndicatorTextPrefab, indicatorTextParent, 10);
            InitializePool(MeleeAttackRangePrefab, MeleeAttackRangeParent, 10);
            InitializePool(SpearMeleeAttackPrefab,SpearMeleeAttackParent,10);
            InitializePool(ClassicBulletPrefab, ClassicBulletParent, 10);
            InitializePool(LaserPrefab, LaserParent, 10);
            InitializePool(wallPrefab, wallParent, 50); 
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void InitializePool(GameObject prefab, Transform parent, int count)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new List<GameObject>();
            parents[prefab] = parent;
        }

        StartCoroutine(InitializePoolCoroutine(prefab, count));
    }

    IEnumerator InitializePoolCoroutine(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewPrefab(prefab);
            yield return null;
        }
    }

    GameObject CreateNewPrefab(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        newObj.transform.SetParent(parents[prefab]);
        pools[prefab].Add(newObj);
        return newObj;
    }

    public GameObject GetObjectFromPool(GameObject prefab, Transform customParent, Vector3 position = default)
    {
        if (!pools.ContainsKey(prefab))
        {
            InitializePool(prefab, customParent, 1);
        }

        foreach (var item in pools[prefab])
        {
            if (!item.activeInHierarchy && item.transform.parent == parents[prefab])
            {
                item.SetActive(true);
                item.transform.SetParent(customParent);
                item.transform.position = position == default ? customParent.position : position;
                return item;
            }
        }

        GameObject newObj = CreateNewPrefab(prefab);
        newObj.SetActive(true);
        newObj.transform.SetParent(customParent);
        newObj.transform.position = position == default ? customParent.position : position;
        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj, GameObject prefab)
    {
        if (pools.ContainsKey(prefab))
        {
            obj.transform.localScale = Vector3.one;
            obj.transform.SetParent(parents[prefab]);
            obj.SetActive(false);
        }
    }

    // Wrapper methods for specific objects
    public GameObject GetClassicBullet(GameObject weapon) =>
        GetObjectFromPool(ClassicBulletPrefab, weapon.transform);

    public void ReturnClassicBullet(GameObject classicBullet) =>
        ReturnObjectToPool(classicBullet, ClassicBulletPrefab);

    public GameObject GetMeleeAttackRange(GameObject weapon) =>
        GetObjectFromPool(MeleeAttackRangePrefab, weapon.transform);

    public void ReturnMeleeRange(GameObject meleeAttackRange) =>
        ReturnObjectToPool(meleeAttackRange, MeleeAttackRangePrefab);

    public GameObject GetSpearAttack(GameObject weapon) =>
        GetObjectFromPool(SpearMeleeAttackPrefab, weapon.transform);

    public void ReturnSpearAttack(GameObject spearAttack) =>
        ReturnObjectToPool(spearAttack, SpearMeleeAttackPrefab);
        
    public GameObject GetLaser(GameObject weapon) =>
        GetObjectFromPool(LaserPrefab, weapon.transform);

    public void ReturnLaser(GameObject laser) =>
        ReturnObjectToPool(laser, LaserPrefab);

    public GameObject GetWall(Vector3 position) =>
    GetObjectFromPool(wallPrefab,wallParent, position);

    public void ReturnWall(GameObject wall) =>
        ReturnObjectToPool(wall, wallPrefab);

    public GameObject GetIndicator(GameObject parent) =>
        GetObjectFromPool(indicatorPrefab, parent.transform);

    public void ReturnIndicator(GameObject indicator) =>
        ReturnObjectToPool(indicator, indicatorPrefab);

    public GameObject GetIndicatorText()
    {
        var textObj = GetObjectFromPool(IndicatorTextPrefab, indicatorTextParent);
        textObj.GetComponent<TextMeshProUGUI>().text = "";
        return textObj;
    }

    public void ReturnTMPText(GameObject textObj)
    {
        if (textObj != null)
        {
            textObj.GetComponent<TextMeshProUGUI>().text = "";
            ReturnObjectToPool(textObj, IndicatorTextPrefab);
        }
    }
}
