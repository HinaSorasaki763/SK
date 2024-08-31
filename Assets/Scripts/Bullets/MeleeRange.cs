using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRange : MonoBehaviour
{
    public int Dmg;
    const float life = 0.25f;
    private void OnEnable()
    {
        Invoke(nameof(ResetMeleeRange), life);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Dealing {Dmg} dmg to {collision.gameObject.name}");
        }
    }
    private void ResetMeleeRange()
    {
        Dmg = 0;
        ResourcePool.Instance.ReturnMeleeRange(gameObject);
    }
}
