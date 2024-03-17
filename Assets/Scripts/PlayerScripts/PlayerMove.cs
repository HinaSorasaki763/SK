using CustomUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour,IPointerClickHandler
{
    public Joystick currentJoystick;
    public GameObject SelectedCharacter;
    public float detectionRadius;
    public LayerMask interactableLayer;
    public InteractionButton btn;
    public void SetCurrentJoystick(Joystick joystick)
    {
        currentJoystick = joystick;
        Debug.Log($"currentJoystick = {currentJoystick.name}");
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void Update()
    {
        if (SelectedCharacter == null) return;

        float speed = SelectedCharacter.GetComponent<PlayerStats>().GetSpeed() * Time.deltaTime;
        float speedX = currentJoystick.Horizontal switch
        {
            > 0.2f => speed,
            < -0.2f => -speed,
            _ => 0,
        };

        float speedY = currentJoystick.Vertical switch
        {
            > 0.2f => speed,
            < -0.2f => -speed,
            _ => 0,
        };
        SelectedCharacter.transform.position += new Vector3(speedX, speedY, 0);
        FindClosestInteractable();
    }
    void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(SelectedCharacter.transform.position, detectionRadius, interactableLayer);
        float closestDistance = float.MaxValue;
        Interactable closestInteractable = null;
        Debug.Log($"hit.count = {hits.Length}");
        foreach (var hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }
        if (closestInteractable!= null)
        {
            btn.SetInteractable(closestInteractable);
        }
        else
        {
            btn.SetInteractableNull();
        }
    }
}
