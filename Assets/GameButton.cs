using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnPointerDownAction;
    public event Action OnPointerUpAction;
    public event Action OnPointerClickedAction;

    private bool isClicked;

    public bool IsClicked => isClicked;
    public bool IsPointerDown;
    public bool IsPointerUp;

    private void Update()
    {
        if (isClicked)
        {
            OnPointerClickedAction?.Invoke();
        }
    }

    private void LateUpdate()
    {
        IsPointerDown = false;
        IsPointerUp = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownAction?.Invoke();
        IsPointerDown = true;
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpAction?.Invoke();
        IsPointerUp = true;
        isClicked = false;
    }
}