using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    public int Dmg;
    private float counter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 对敌人造成伤害
            Debug.Log($"Dealing {Dmg} dmg");
        }
    }
    public void Update()
    {
        counter += Time.deltaTime;
        if (counter > 0.25f)
        {
            counter = 0;
            Dmg = 0;

            ResourcePool.Instance.ReturnMeleeRange(gameObject);
        }
    }
}
