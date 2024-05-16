using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CustomerUI
{
    [SerializeField] private Image faceImage;
    [SerializeField] private Image sliderImage;

    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite angrySprite;

    public void UpdateImage(bool isHappy)
    {
        faceImage.sprite = isHappy ? happySprite : angrySprite;
    }

    public void StartSlider(float timer)
    {
        faceImage.gameObject.SetActive(true);
        sliderImage.gameObject.SetActive(true);
        UpdateImage(true);
        sliderImage.fillAmount = 0;
        sliderImage.DOKill();
        sliderImage.DOFillAmount(1, timer).OnComplete(() =>
        {
            sliderImage.gameObject.SetActive(false);
            UpdateImage(false);
        });
    }

    public void DisableImage()
    {
        faceImage.gameObject.SetActive(false);
        sliderImage.gameObject.SetActive(false);
    }
}