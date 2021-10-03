using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent clickedEvent;
    public UnityEvent hoverStartEvent;
    public UnityEvent hoverEndEvent;

    public bool DoHoverAnim = true;
    public bool DoAppearAnim = true;

    Animator animator;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        ShowButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickedEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverStartEvent?.Invoke();
        if (DoHoverAnim == false) return;
        if (animator != null)
            animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverEndEvent?.Invoke();
        if (DoHoverAnim == false) return;
        if (animator != null)
            animator.SetBool("Hover", false);
    }

    private void OnEnable()
    {
        if(animator != null)
            ShowButton();
    }

    public void DoClickedEffect()
    {
        if (animator != null)
            animator.SetTrigger("Clicked");
    }

    public void UnDoClickedEffect()
    {
        if (animator != null)
            animator.SetTrigger("Unclicked");
    }

    void ShowButton()
    {
        if (DoAppearAnim == false) return;
        if (animator != null)
            animator.SetTrigger("Appear");
    }
}
