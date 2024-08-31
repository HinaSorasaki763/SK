using System.Collections.Generic;
using UnityEngine;

public class JoysticksSelection : MonoBehaviour
{
    public List<Joystick> joysticks = new List<Joystick>();
    public PlayerMove playerMove;

    public void OnSelected(int selection)
    {
        for (int i = 0; i < joysticks.Count; i++)
        {
            joysticks[i].gameObject.SetActive(i == selection);
        }
        playerMove.SetCurrentJoystick(joysticks[selection]);
    }
}
