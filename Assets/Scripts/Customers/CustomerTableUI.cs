using System;
using UnityEngine;

[Serializable]
public class CustomerTableUI
{
    [SerializeField] private CustomerTableUIConfiguration[] customerTableUiConfigurations;

    public void UpdateSpriteUI(int id, Sprite sprite)
    {
        customerTableUiConfigurations[id].Image.sprite = sprite;
    }

    public void UpdateCheckMark(int id, bool checkMark)
    {
        customerTableUiConfigurations[id].CheckMark.gameObject.SetActive(checkMark);
    }

    public void EnableUI(int amountObject)
    {
        var currentItem = amountObject;
        foreach (var customerTableUiConfiguration in customerTableUiConfigurations)
        {
            if (currentItem <= 0)
            {
                customerTableUiConfiguration.Image.gameObject.SetActive(false);
                continue;
            }

            currentItem--;
            customerTableUiConfiguration.Image.gameObject.SetActive(true);
        }
    }
}