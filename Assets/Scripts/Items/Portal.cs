using CustomUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactable
{
    public void Start()
    {

    }
    public override void Interact()
    {
        Debug.Log("Portal interact");
        Disable();
        SceneManager.LoadScene("BattleScene");

    }
}
