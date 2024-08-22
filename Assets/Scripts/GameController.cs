using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapBuilder mapBuilder;

    void Start()
    {
        mapBuilder.BuildHollowSquare();
    }

    void Update()
    {
        // 例如，可以在按下某个按键时清理地图
        if (Input.GetKeyDown(KeyCode.C))
        {
            mapBuilder.ClearMap();
        }
    }
}
