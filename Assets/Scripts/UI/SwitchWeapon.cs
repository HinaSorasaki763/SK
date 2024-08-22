using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SwitchWeapon : MonoBehaviour
{
    public Button Button;
    private GameObject Character;
    public TextMeshProUGUI weaponName;
    public void Awake()
    {
        Button.onClick.AddListener(OnSwitchWeapon);
    }
    public void Update()
    {
        if (Character == null)
        {
            Character = PlayerMove.Instance.SelectedCharacter;
        }
    }
    public void OnSwitchWeapon()
    {
        string name = PlayerAttack.Instance.SwitchWeapon();
        StartCoroutine(ShowWeaponName(name));
    }
    public IEnumerator ShowWeaponName(string name)
    {
        weaponName.gameObject.SetActive(true);
        weaponName.text = name;
        float duration = 2f; // 持续时间
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponName.text = string.Empty; // 清除文本
    }
}
