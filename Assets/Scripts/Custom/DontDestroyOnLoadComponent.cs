using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadComponent : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
