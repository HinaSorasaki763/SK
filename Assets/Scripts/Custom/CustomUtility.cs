using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CustomUtility
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
    public enum AttackTypeTag
    {
        Melee,
        Ranged,
        Bullet,
        Laser
    }
    public static class Utility
    {
        public static float AttackRangeRatio = 0.33f;
        public static bool HasTag(List<AttackTypeTag> tags, AttackTypeTag tag)
        {
            return tags.Contains(tag);
        }

        public static void DebugTags(List<AttackTypeTag> tags)
        {
            Debug.Log("Tags: " + string.Join(", ", tags));
        }
    }
    public abstract class Interactable : MonoBehaviour
    {
        public string objectName = "Interactable Object";
        public bool detectable = true;
        public abstract void Interact();
        public virtual void ShowIndicator(Vector3 pos)
        {
            if (!transform.Find("IndicatorPrefab(Clone)"))
            {
                GameObject indicator = ResourcePool.Instance.GetIndicator(gameObject);
                Indicator indicator1 = indicator.GetComponent<Indicator>();
                indicator1.ResetPos(pos + new Vector3(0, 0.5f, 0));
                indicator1.indicatorText = ResourcePool.Instance.GetIndicatorText();
                indicator1.indicatorText.GetComponent<TextMeshProUGUI>().text = objectName;
            }
            else
            {
                Transform obj = transform.Find("IndicatorPrefab(Clone)");
                obj.gameObject.SetActive(true);
                obj.GetComponent<Indicator>().indicatorText.SetActive(true);
                obj.GetComponent<Indicator>().indicatorText.GetComponent<TextMeshProUGUI>().text= objectName;
            }
        }
        public virtual void HideIndicator()
        {
            Transform obj = transform.Find("IndicatorPrefab(Clone)");
            obj.gameObject.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.GetComponent<TextMeshProUGUI>().text = "";
        }

        public virtual void Awake()
        {

        }
    }
}
