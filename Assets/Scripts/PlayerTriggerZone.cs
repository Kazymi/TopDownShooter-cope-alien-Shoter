using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTriggerZone : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private Image Slider;

    public event Action OnTriggerCompleted;
    public event Action OnPlayerLeftZone;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        var triggerListener = other.GetComponent<PlayerTriggerListener>();
        if (triggerListener && triggerListener.IsCanBeRead)
        {
            Slider.DOKill();
            Slider.DOFillAmount(1, 1f - timer * Slider.fillAmount).OnComplete(() =>
            {
                OnTriggerCompleted?.Invoke();
            });
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        OnPlayerLeftZone?.Invoke();
        var triggerListener = other.GetComponent<PlayerTriggerListener>();
        if (triggerListener && triggerListener.IsCanBeRead)
        {
            Slider.DOKill();
            Slider.DOFillAmount(0, timer * Slider.fillAmount);
        }
    }
}