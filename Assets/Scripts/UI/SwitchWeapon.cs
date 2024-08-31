using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchWeapon : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI weaponName;
    private GameObject Character;

    private void Awake()
    {
        Button.onClick.AddListener(OnSwitchWeapon);
    }

    private void Update()
    {
        Character ??= PlayerMove.Instance.SelectedCharacter;
    }

    private void OnSwitchWeapon()
    {
        StartCoroutine(ShowWeaponName(PlayerAttack.Instance.SwitchWeapon()));
    }

    private IEnumerator ShowWeaponName(string name)
    {
        weaponName.gameObject.SetActive(true);
        weaponName.text = name;
        yield return new WaitForSeconds(2f);
        weaponName.text = string.Empty;
    }
}
 