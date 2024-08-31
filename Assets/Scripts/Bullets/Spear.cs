using UnityEngine;

public class Spear : MonoBehaviour
{
    public int Dmg;
    private Transform spearHead;
    private const float life = 0.25f;

    public void Init(GameObject parentWeapon, int dmg)
    {
        Dmg = dmg;
        spearHead = parentWeapon.transform.Find("SpearHead");
        transform.position = spearHead.position;
        transform.SetParent(null);
        Invoke(nameof(ReturnToPool), life);
    }

    private void ReturnToPool()
    {
        spearHead = null;
        ResourcePool.Instance.ReturnSpearAttack(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Dealing {Dmg} dmg");
        }
    }
}
