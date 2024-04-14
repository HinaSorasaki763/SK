using CustomUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    public Button Button;
    public TMPro.TextMeshProUGUI text;
    private Interactable interactableInRange;
    public void Awake()
    {
        Button.onClick.AddListener(PerformInteract);
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
    public void ClearIndicator(Interactable interactable =null)
    {
        if (interactableInRange != null&&interactable != interactableInRange)
        {
            interactableInRange.HideIndicator();
        }
    }
    public void PerformInteract()
    {
        if (interactableInRange!=null)
        {
            interactableInRange.Interact();
        }
        else
        {
            PlayerAttack.Instance.Attack();
        }
    }
    public void Start()
    {
        
    }
    public void Update()
    {
        
    }
}
