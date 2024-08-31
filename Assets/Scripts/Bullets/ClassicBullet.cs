using UnityEngine;

public class ClassicBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int Dmg;

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifetime);
    }

    public void Initialize(Vector2 direction)
    {
        transform.SetParent(ResourcePool.Instance.tempBulletParent);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Dealing {Dmg} dmg to {collision.gameObject.name}");
            CancelInvoke(nameof(ReturnToPool));
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        ResourcePool.Instance.ReturnClassicBullet(gameObject);
    }
}
