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
        public static bool HasTag(List<AttackTypeTag> tags, AttackTypeTag tag) => tags.Contains(tag);
        public static void DebugTags(List<AttackTypeTag> tags) => Debug.Log("Tags: " + string.Join(", ", tags));
    }

    public abstract class Interactable : MonoBehaviour
    {
        public string objectName = "Interactable Object";
        public bool detectable = true;
        public abstract void Interact();
        public virtual void Reuse() => detectable = true;

        public virtual void ShowIndicator(Vector3 pos)
        {
            var indicatorTransform = transform.Find("IndicatorPrefab(Clone)");
            var indicatorComponent = indicatorTransform
                ? indicatorTransform.GetComponent<Indicator>()
                : ResourcePool.Instance.GetIndicator(gameObject).GetComponent<Indicator>();
            if (indicatorComponent.indicatorText == null)
            {
                indicatorComponent.indicatorText = ResourcePool.Instance.GetIndicatorText();
            }

            UpdateIndicator(indicatorComponent, pos);

            if (indicatorComponent.indicatorText.GetComponent<TextMeshProUGUI>().text != objectName)
            {
                indicatorComponent.indicatorText.GetComponent<TextMeshProUGUI>().text = "";
                Invoke(nameof(UpdateIndicatorText), 0.033f);
            }
        }

        private void UpdateIndicatorText()
        {
            var indicatorTransform = transform.Find("IndicatorPrefab(Clone)");
            if (indicatorTransform != null)
            {
                var indicatorTextComponent = indicatorTransform.GetComponent<Indicator>().indicatorText.GetComponent<TextMeshProUGUI>();
                if (indicatorTextComponent.text == "") indicatorTextComponent.text = objectName;
            }
        }

        private void UpdateIndicator(Indicator indicator, Vector3 pos)
        {
            indicator.ResetPos(pos + new Vector3(0, 0.5f, 0));
            indicator.gameObject.SetActive(true);
            indicator.indicatorText.SetActive(true);
        }

        public virtual void Disable()
        {
            detectable = false;
            HideIndicator();
        }

        public virtual void HideIndicator()
        {
            var obj = transform.Find("IndicatorPrefab(Clone)");
            obj.gameObject.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.SetActive(false);
            obj.GetComponent<Indicator>().indicatorText.GetComponent<TextMeshProUGUI>().text = "";
        }

        public virtual void Awake() { }
    }
}
