using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameController : MonoBehaviour
{
    public MapBuilder mapBuilder;
    void Start()
    {
        mapBuilder.BuildHollowSquare();
        for (int i = 0; i < 7; i++)
        {
            Vector3 v = new Vector3(0,5+i,0);
            WeaponGenerator.Instance.CreateWeapon(i,v);
        }
    }
}
