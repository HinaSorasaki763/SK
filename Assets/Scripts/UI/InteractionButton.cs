using CustomUtility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 添加命名空间

public class InteractionButton : MonoBehaviour
{
    public Button Button;
    public TMPro.TextMeshProUGUI text;
    private Interactable interactableInRange;
    private bool isButtonPressed = false;
    private float lastAttackTime;
    private float pointerDownTime;
    public void Awake()
    {
        Button.onClick.AddListener(OnButtonClick);
    }

    public void SetInteractable(Interactable interactable)
    {
        ClearIndicator(interactable);
        interactableInRange = interactable;
        interactable.ShowIndicator(interactable.transform.position);
        text.text = "Interact";
    }

    public void SetInteractableNull()
    {
        ClearIndicator();
        interactableInRange = null;
        text.text = "Attack";
    }

    public void ClearIndicator(Interactable interactable = null)
    {
        if (interactableInRange != null && interactable != interactableInRange)
        {
            interactableInRange.HideIndicator();
        }
    }

    public void OnButtonClick()
    {
        if (interactableInRange != null)
        {
            interactableInRange.Interact();
        }
        else
        {
            PlayerAttack.Instance.Attack();
            lastAttackTime = Time.time; // 记录第一次攻击的时间
        }
    }

    private IEnumerator LongPressCheck()
    {
        while (isButtonPressed)
        {
            yield return new WaitForSeconds(1f);
            if (interactableInRange == null && Time.time - lastAttackTime >= PlayerAttack.Instance.weaponInHand.weaponData.cooldownTime)
            {
                PlayerAttack.Instance.Attack();
                lastAttackTime = Time.time; 
            }
        }
    }

    public void OnPointerUp()
    {
        isButtonPressed = false;
    }

    public void OnPointerDown()
    {
        isButtonPressed = true;
        pointerDownTime = Time.time;
    }
    public void Start()
    {
        // 添加指针事件监听器
        EventTrigger trigger = Button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown(); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    public void Update()
    {
        if (isButtonPressed && Time.time - pointerDownTime >= 1f)
        {
            StartCoroutine(LongPressCheck());
            isButtonPressed = false; // 防止重复启动
        }
    }
}
