using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour,IPointerClickHandler
{
    public GameObject StartgameText;
    public GameObject EntergameButton;
    public void OnPointerClick(PointerEventData eventData)
    {
        StartgameText.SetActive(false);
        EntergameButton.SetActive(true);
    }
}
