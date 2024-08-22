using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{

    public int Dmg;
    private GameObject parentWeapon;
    private Transform spear_head;
    const float life = 5f;
    public void Init(GameObject obj,int dmg)
    {
        Dmg = dmg;
        parentWeapon = obj;
        spear_head = parentWeapon.transform.Find("SpearHead");
        transform.position = spear_head.position;
        transform.SetParent( null);
        Invoke(nameof(Return),life);
    }
    public void Return()
    {
        spear_head = null;
        parentWeapon = null;
        ResourcePool.Instance.ReturnSpearAttack(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Dealing {5} dmg");
        }
    }
}
