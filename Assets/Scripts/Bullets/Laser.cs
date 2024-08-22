using UnityEngine;

public class Laser : MonoBehaviour
{
    public LayerMask wallLayer;
    public int dmg;
    public int width;
    public bool continuous;
    public GameObject currPlayer;
    private GameObject parentWeapon;
    private Transform firePoint;
    private bool check = false;

    private void OnEnable()
    {
        // 可在这里初始化
    }

    private void FixedUpdate()
    {
        if (continuous)
        {
            UpdateLaser(); // 持续激光需要不断更新
        }
    }

    public void Initialize(GameObject obj, bool isContinuous)
    {
        currPlayer = PlayerMove.Instance.SelectedCharacter;
        parentWeapon = obj;
        firePoint = parentWeapon.transform.Find("FirePoint");
        continuous = isContinuous;
        float angle = transform.rotation.eulerAngles.z;
        angle += 270f;  // 或者 angle -= 90f, 根据你的需求进行调整
        transform.rotation = Quaternion.Euler(0, 0, angle);
        UpdateLaser();

        if (!continuous)
        {
            transform.SetParent(ResourcePool.Instance.tempLaserParent);
            Invoke(nameof(ReturnLaser), 3f); // 3秒后回收激光
            check = true; // 标志位，用于阻止后续更新
        }
    }

    private void UpdateLaser()
    {
        if (!continuous && check)
        {
            return; // 对于短期激光，如果已经初始化过，则不再更新
        }

        // 如果是第一次调用，则不改变rotation，只计算长度
        if (continuous || !check)
        {
            Vector3 startPosition = firePoint.position;
            Vector3 direction = firePoint.right; // 使用传入的方向，不再自动更新
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, Mathf.Infinity, wallLayer);

            // 左右偏移以确保检测边缘
            Vector3 leftOffset = Vector3.Cross(direction, Vector3.forward) * 0.1f;
            Vector3 rightOffset = Vector3.Cross(direction, Vector3.back) * 0.1f;

            RaycastHit2D leftHit = Physics2D.Raycast(startPosition + leftOffset, direction, Mathf.Infinity, wallLayer);
            RaycastHit2D rightHit = Physics2D.Raycast(startPosition + rightOffset, direction, Mathf.Infinity, wallLayer);

            float distance = Mathf.Min(hit.collider != null ? hit.distance : Mathf.Infinity,
                                       leftHit.collider != null ? leftHit.distance : Mathf.Infinity,
                                       rightHit.collider != null ? rightHit.distance : Mathf.Infinity);
            if (distance < Mathf.Infinity)
            {
                AdjustLaserLength(distance);
            }
            else
            {
                Debug.LogError("should collide with wall");
            }
        }
    }

    void AdjustLaserLength(float length)
    {
        Vector3 scale = transform.localScale;
        scale.y = length / 5.8f;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 对敌人造成伤害
            Debug.Log($"Dealing {5} dmg to {collision.name}");
        }
    }

    void ReturnLaser()
    {
        parentWeapon = null;
        firePoint = null;
        ResourcePool.Instance.ReturnLaser(gameObject);
        check = false; // 重置标志位
    }
}
