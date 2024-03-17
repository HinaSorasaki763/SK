using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public List<Slider> sliders = new List<Slider>();
    public TMPro.TextMeshProUGUI characterName;
    public int level;
    public Button ConfirmButton;
    public GameObject Joysticks;
    public GameObject Selector;
    public PlayerMove playerMove;
    public List<GameObject> marks = new List<GameObject>();
    public void Awake()
    {
        ConfirmButton.onClick.AddListener(ConfirmCharacter);
    }
    public void ShowStats(PlayerScriptableObject playerStats,int level)
    {
        characterName.text = playerStats.Name;
        sliders[0].value = playerStats.Hitpoint;
        sliders[1].value = playerStats.Shield;
        sliders[2].value = playerStats.Mana;
        sliders[3].value = playerStats.MeleeDmg;
        sliders[4].value = playerStats.MovementSpeed;
    }
    public void ConfirmCharacter()
    {
        Joysticks.SetActive(true);
        Selector.SetActive(false);
        foreach (var item in marks)
        {
            item.SetActive(false);
        }
    }
    public void ResetCamera()
    {
        Camera camera = Camera.main;
        camera.orthographicSize = 5;
        camera.transform.position = new Vector3(0, 0, -10);
    }
}
