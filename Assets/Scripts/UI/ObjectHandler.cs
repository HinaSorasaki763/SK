using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public GameObject CharacterSelector;
    public GameObject Joysticks;
    public GameObject StartgameButton;
    public GameObject StartgamePanel;
    public void Awake()
    {
        CharacterSelector.SetActive(false);
        Joysticks.SetActive(false);
        StartgameButton.SetActive(false);
        StartgamePanel.SetActive(true);
    }
}
