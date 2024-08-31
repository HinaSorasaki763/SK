using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask wallLayer;
    public int dmg;
    public bool continuous;
    private Transform firePoint;
    private bool check;
    private float damageInterval = 0.25f;
    private float damageTimer;
    const float tempLaserLife = 0.1f;
    private Collider2D laserCollider;
    private Collider2D[] results = new Collider2D[10];
    private void Awake()
    {
        laserCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (continuous || !check)
        {
            UpdateLaser();
            CheckCollisions();
            Debug.Log($"damageTimer = {damageTimer}");
        }
    }

    public void Initialize(GameObject parentWeapon, bool isContinuous)
    {
        continuous = isContinuous;
        firePoint = parentWeapon.transform.Find("FirePoint");
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 270f);
        UpdateLaser();

        if (!continuous)
        {
            transform.SetParent(ResourcePool.Instance.tempLaserParent);
            Invoke(nameof(ReturnLaser), tempLaserLife);
            check = true;
        }
    }

    private void UpdateLaser()
    {
        Vector3 direction = firePoint.right;
        Vector3[] offsets = { Vector3.zero, Vector3.Cross(direction, Vector3.forward) * 0.1f, Vector3.Cross(direction, Vector3.back) * 0.1f };

        float minDistance = Mathf.Infinity;

        foreach (var offset in offsets)
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position + offset, direction, Mathf.Infinity, wallLayer);
            if (hit.collider != null)
            {
                minDistance = Mathf.Min(minDistance, hit.distance);
            }
        }

        if (minDistance < Mathf.Infinity)
        {
            AdjustLaserLength(minDistance);
        }
        else
        {
            Debug.LogError("Laser should collide with wall");
        }
    }

    private void AdjustLaserLength(float length)
    {
        transform.localScale = new Vector3(transform.localScale.x, length / 5.8f, transform.localScale.z);
    }

    private void CheckCollisions()
    {
        ContactFilter2D contactFilter = new ContactFilter2D
        {
            layerMask = enemyLayer,
            useTriggers = true
        };

        int hitCount = Physics2D.OverlapCollider(laserCollider, contactFilter, results);

        for (int i = 0; i < hitCount; i++)
        {
            if (results[i].CompareTag("Enemy"))
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= damageInterval)
                {
                    Debug.Log($"Dealing {dmg} dmg to {results[i].name}");
                    damageTimer = 0f;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log($"Initial hit: dealing {dmg} dmg to {collision.name}");
        }
    }

    private void ReturnLaser()
    {
        firePoint = null;
        ResourcePool.Instance.ReturnLaser(gameObject);
        check = false;
    }
}
