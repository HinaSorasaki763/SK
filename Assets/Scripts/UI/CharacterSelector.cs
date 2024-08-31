using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    public List<Slider> sliders = new();
    public TextMeshProUGUI characterName;
    public Button ConfirmButton;
    public GameObject Joysticks;
    public GameObject Selector;
    public PlayerMove playerMove;
    public List<GameObject> marks = new();
    public GameObject OperatingCanva;

    private void Awake()
    {
        ConfirmButton.onClick.AddListener(() =>
        {
            ConfirmCharacter();
            ResetCamera();
        });
    }

    public void ShowStats(PlayerScriptableObject playerStats, int level)
    {
        characterName.text = playerStats.Name;
        sliders[0].value = playerStats.Hitpoint;
        sliders[1].value = playerStats.Shield;
        sliders[2].value = playerStats.Mana;
        sliders[3].value = playerStats.MeleeDmg;
        sliders[4].value = playerStats.MovementSpeed;
    }

    private void ConfirmCharacter()
    {
        Joysticks.SetActive(true);
        OperatingCanva.SetActive(true);
        Selector.SetActive(false);
        marks.ForEach(mark => mark.SetActive(false));
        PlayerMove.Instance.DontDestroy();
    }

    private void ResetCamera()
    {
        Camera.main.orthographicSize = 5;
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    public void Deselect()
    {
        playerMove.SelectedCharacter = null;
        ResetCamera();
    }
}
