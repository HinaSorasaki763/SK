using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerStats : MonoBehaviour, IPointerClickHandler
{
    public PlayerScriptableObject playerStats;
    public CharacterSelector characterSelector;
    private Camera mainCamera;
    public GameObject mark;
    private Vector3 mark_pos;
    public PlayerMove playerMove;
    public void OnEnable()
    {
        mark_pos = mark.transform.position;
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
    public void Update()
    {
        float amount = 0.15f;
        float speed = 2f;
        float newY = mark_pos.y + Mathf.Sin(Time.time * speed) * amount;
        mark.transform.position = new Vector3(mark_pos.x, newY, mark_pos.z);
    }
    public float GetSpeed()
    {
        return playerStats.MovementSpeed;
    }
}
