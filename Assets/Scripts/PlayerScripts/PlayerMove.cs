using CustomUtility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour, IPointerClickHandler
{
    public static PlayerMove Instance;
    public Joystick currentJoystick;
    public GameObject SelectedCharacter;
    public float detectionRadius;
    public LayerMask interactableLayer;
    public InteractionButton btn;
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
    public void SetCurrentJoystick(Joystick joystick)
    {
        currentJoystick = joystick;
        Debug.Log($"currentJoystick = {currentJoystick.name}");
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            WeaponGenerator.Instance.CreateRandWeapon();
        }
        if (SelectedCharacter == null) return;

        float speed = SelectedCharacter.GetComponent<PlayerStats>().GetSpeed() * Time.deltaTime;

        // 通過三元運算符簡化速度的計算
        float speedX = currentJoystick.Horizontal switch
        {
            > 0.2f => speed,
            < -0.2f => -speed,
            _ => 0,
        };

        float speedY = currentJoystick.Vertical switch
        {
            > 0.2f => speed,
            < -0.2f => -speed,
            _ => 0,
        };

        // 更新位置
        SelectedCharacter.transform.position += new Vector3(speedX, speedY, 0);
        FindClosestInteractable();
    }
    public Vector3 GetPlayerPos()
    {
        return SelectedCharacter.transform.position;
    }
    public void DontDestroy()
    {
        SelectedCharacter.transform.parent = null;
        SelectedCharacter.AddComponent<DontDestroyOnLoadComponent>();
    }
    void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(SelectedCharacter.transform.position, detectionRadius, interactableLayer);
        float closestDistance = float.MaxValue;
        Interactable closestInteractable = null;
        foreach (var hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance && !interactable.InteractionDisabled)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }
        if (closestInteractable != null)
        {
            btn.SetInteractable(closestInteractable);
        }
        else
        {
            btn.SetInteractableNull();
        }
    }
}
