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
        PlayerMove.Instance.DontDestroy();
        Debug.Log("Portal interact");
        SceneManager.LoadScene("BattleScene");
    }
    

}
