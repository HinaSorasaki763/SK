using CustomUtility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InteractionButton : MonoBehaviour
{
    public Button Button;
    public TextMeshProUGUI text;
    private Interactable interactableInRange;
    private bool isButtonPressed = false;
    private float lastAttackTime;
    private float pointerDownTime;

    private void Awake()
    {
        Button.onClick.AddListener(OnButtonClick);
        AddPointerEvent(EventTriggerType.PointerDown, OnPointerDown);
        AddPointerEvent(EventTriggerType.PointerUp, OnPointerUp);
    }

    public void SetInteractable(Interactable interactable)
    {
        if (interactableInRange != interactable)
        {
            interactableInRange?.HideIndicator();
            interactableInRange = interactable;
            interactable.ShowIndicator(interactable.transform.position);
        }
        text.text = "Interact";
    }

    public void SetInteractableNull()
    {
        interactableInRange?.HideIndicator();
        interactableInRange = null;
        text.text = "Attack";
    }

    private void OnButtonClick()
    {
        if (interactableInRange != null)
        {
            interactableInRange.Interact();
        }
        else
        {
            PlayerAttack.Instance.Attack(false);
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator LongPressCheck()
    {
        while (isButtonPressed)
        {
            Debug.Log(isButtonPressed);
            yield return new WaitForSeconds(.25f);

            if (interactableInRange == null && Time.time - lastAttackTime >= PlayerAttack.Instance.weaponInHand.weaponData.cooldownTime)
            {
                PlayerAttack.Instance.Attack(true);
                lastAttackTime = Time.time;
                Debug.Log($"lastAttackTime = {lastAttackTime}");
            }
        }
    }

    private void OnPointerUp()
    {
        isButtonPressed = false;
        if (PlayerAttack.Instance.weaponInHand != null)
        {
            PlayerAttack.Instance.weaponInHand.StopAllAttack();
        }
    }


    private void OnPointerDown()
    {
        isButtonPressed = true;
        pointerDownTime = Time.time;
    }

    private void Update()
    {
        if (isButtonPressed && Time.time - pointerDownTime >= 1f)
        {
            StartCoroutine(LongPressCheck());

        }
    }
    public void ClearIndicator(Interactable interactable = null)
    {
        if (interactableInRange != null && interactable != interactableInRange)
        {
            interactableInRange.HideIndicator();
        }
    }
    private void AddPointerEvent(EventTriggerType eventType, UnityEngine.Events.UnityAction callback)
    {
        var trigger = Button.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener(_ => callback());
        trigger.triggers.Add(entry);
    }
}
