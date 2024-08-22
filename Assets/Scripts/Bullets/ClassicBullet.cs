using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    private float life = 0;
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        life = 0;
    }
    public void Update()
    {
        life += Time.deltaTime;
        if (life >= lifetime)
        {
            life = 0;
            ResourcePool.Instance.ReturnClassicBullet(gameObject);
        }
    }
        public void Initialize(Vector2 direction)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log($"collide on {collision.collider.name}");
            ResourcePool.Instance.ReturnClassicBullet(gameObject);
        }

    }
}