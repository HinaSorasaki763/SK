using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStats : MonoBehaviour, IPointerClickHandler
{
    public PlayerScriptableObject playerStats;
    public CharacterSelector characterSelector;
    public GameObject mark;
    public PlayerMove playerMove;

    private Vector3 initialMarkPosition;
    private Camera mainCamera;

    private void OnEnable()
    {
        initialMarkPosition = mark.transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        characterSelector.gameObject.SetActive(true);
        characterSelector.ShowStats(playerStats, 0);

        mainCamera = Camera.main;
        mainCamera.orthographicSize = 1.75f;
        mainCamera.transform.position = transform.position + new Vector3(0, 0.19f, -10);

        playerMove.SelectedCharacter = gameObject;
    }

    private void Update()
    {
        mark.transform.position = new Vector3(initialMarkPosition.x, initialMarkPosition.y + Mathf.Sin(Time.time * 2f) * 0.15f, initialMarkPosition.z);
    }

    public float GetSpeed() => playerStats.MovementSpeed;
}
