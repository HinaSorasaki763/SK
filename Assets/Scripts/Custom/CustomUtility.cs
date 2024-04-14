using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CustomUtility//所有公用的項目
{
    enum PlayerStats
    {
        Hitpoint,
        Shield,
        Mana,
        MeleeDamage,
        MovementSpeed,
    }
    enum CharacterType
    {
        Assassin,
        Caster,
        Knight,
        Priest,
        Ranger
    }
    public abstract class Interactable : MonoBehaviour
    {
        public string objectName = "Interactable Object";
        public bool InteractionDisabled;
        public abstract void Interact();
        public virtual void ShowIndicator(Vector3 pos)
        {
            if (!transform.Find("IndicatorPrefab(Clone)"))
            {
                GameObject indicator = ResourcePool.Instance.GetIndicator(gameObject);
                Indicator indicatorScript = indicator.GetComponent<Indicator>();
                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    Vector3 topOfObject = collider.bounds.center + new Vector3(0, collider.size.y / 2, 0);
                    indicatorScript.ResetPos(topOfObject);
                    Debug.Log("Success");
                }
                else
                {
                    indicatorScript.ResetPos(pos + new Vector3(0, 0.5f, 0));
                    Debug.Log("Error");
                }
                indicatorScript.indicatorText = ResourcePool.Instance.GetIndicatorText();
                indicatorScript.indicatorText.GetComponent<TextMeshProUGUI>().text = objectName;
            }
            else
            {
                Transform obj = transform.Find("IndicatorPrefab(Clone)");
                Indicator indicatorScript = obj.GetComponent<Indicator>();
                indicatorScript.indicatorText.GetComponent<TextMeshProUGUI>().text = "";
                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                Vector3 topOfObject = collider.bounds.center + new Vector3(0, collider.size.y / 2, 0);
                indicatorScript.ResetPos(topOfObject);
                indicatorScript.indicatorText.SetActive(true);
                obj.gameObject.SetActive(true);
                StartCoroutine(WaitOneFrame(indicatorScript));

            }
        }
        IEnumerator WaitOneFrame(Indicator indicatorScript)
        {
            yield return null;

            indicatorScript.indicatorText.GetComponent<TextMeshProUGUI>().text = objectName;
        }
        public virtual void HideIndicator()
        {
            
            Transform obj = transform.Find("IndicatorPrefab(Clone)");
            obj.gameObject.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.GetComponent<TextMeshProUGUI>().text = "";
        }

        protected virtual void Awake()
        {

        }
    }
}
