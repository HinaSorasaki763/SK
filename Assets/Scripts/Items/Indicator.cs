using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Indicator : MonoBehaviour
{
    private Vector3 initPos;
    public GameObject indicatorText;
    public void ResetPos(Vector3 pos)
    {
        initPos = pos;
    }
    public void Update()
    {
        float amount = 0.15f;
        float speed = 2f;
        float newY = initPos.y + Mathf.Sin(Time.time * speed) * amount;
        Vector3 pos = new Vector3(initPos.x, newY, initPos.z);
        transform.position = pos;
        if (indicatorText != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos + new Vector3(0, 0.25f, 0));
            indicatorText.transform.position = screenPos;
        }
    }
}
