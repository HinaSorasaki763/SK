using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Character",menuName = "ScriptableObject/character")]

public class PlayerScriptableObject : ScriptableObject
{
    public string Name;
    [TextArea(3,10)]
    public string Description;
    public int Hitpoint;
    public int Mana;
    public int Shield;
    public int MovementSpeed;
    public int MeleeDmg;
}
