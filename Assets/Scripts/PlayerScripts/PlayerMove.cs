using CustomUtility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour, IPointerClickHandler
{
    public static PlayerMove Instance { get; private set; }
    public Joystick currentJoystick;
    public GameObject SelectedCharacter;
    public float detectionRadius;
    public LayerMask interactableLayer;
    public InteractionButton btn;
    private Vector3 joystickDir;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentJoystick(Joystick joystick)
    {
        currentJoystick = joystick;
        Debug.Log($"currentJoystick = {currentJoystick.name}");
    }

    public Vector3 GetJoystickDir() => joystickDir;

    public void UpdateJoystickDir(Vector3 dir) => joystickDir = dir;

    public void OnPointerClick(PointerEventData eventData) { }

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            WeaponGenerator.Instance.CreateRandWeapon();
        }

        if (SelectedCharacter == null) return;

        MoveCharacter();
        UpdateJoystickDir(new Vector3(currentJoystick.Horizontal, currentJoystick.Vertical, 0));
        FindClosestInteractable();
    }

    private void MoveCharacter()
    {
        float speed = SelectedCharacter.GetComponent<PlayerStats>().GetSpeed() * Time.deltaTime;
        Vector3 direction = new(
            currentJoystick.Horizontal > 0.2f ? speed : currentJoystick.Horizontal < -0.2f ? -speed : 0,
            currentJoystick.Vertical > 0.2f ? speed : currentJoystick.Vertical < -0.2f ? -speed : 0,
            0
        );

        SelectedCharacter.transform.position += direction;
        Camera.main.transform.position = SelectedCharacter.transform.position + new Vector3(0, 0, -10);
    }

    public Vector3 GetPlayerPos() => SelectedCharacter.transform.position;

    public void DontDestroy()
    {
        SelectedCharacter.transform.parent = null;
        SelectedCharacter.AddComponent<DontDestroyOnLoadComponent>();
    }

    private void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(SelectedCharacter.transform.position, detectionRadius, interactableLayer);
        Interactable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<Interactable>();
            if (interactable == null || !interactable.detectable) continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
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
