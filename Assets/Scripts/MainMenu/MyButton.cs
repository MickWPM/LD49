using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public AnimationClip hoverClip;
    public UnityEvent clickedEvent;
    Animator animator;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        ShowButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickedEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }

    private void OnEnable()
    {
        if(animator != null)
            ShowButton();
    }


    void ShowButton()
    {
        animator.SetTrigger("Appear");
    }
}
