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
            GameObject obj = joysticks[selection].gameObject;
            if (i!= selection)
            {
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
                playerMove.SetCurrentJoystick(joysticks[selection]);
            }
        }
    }
}
