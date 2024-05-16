using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomersTable : InterplayObject
{
    [SerializeField] private CustomerTableUI customerTableUi;
    [SerializeField] private Transform[] itemPosition;

    private CustomerNPCStateMachine customer;
    private List<TableConfiguration> currentTableConfigurations = new List<TableConfiguration>();

    public void TableInitialize(List<TableConfiguration> itemConfigurations, CustomerNPCStateMachine customer)
    {
        this.customer = customer;
        for (int i = 0; i < itemConfigurations.Count; i++)
        {
            itemConfigurations[i].ItemPosition = itemPosition[i];
            customerTableUi.UpdateSpriteUI(i,
                ItemSpriteStorage.Instance.GetSpriteByType(itemConfigurations[i].ItemType));
            customerTableUi.UpdateCheckMark(i, false);
        }

        customerTableUi.EnableUI(itemConfigurations.Count);
        currentTableConfigurations = itemConfigurations;
    }

    public override void IterplayObject(CharacterInvetory characterInvetory)
    {
        if (currentTableConfigurations.Count == 0) return;
        var isItemsFull = true;
        for (var index = 0; index < currentTableConfigurations.Count; index++)
        {
            var currentTableConfiguration = currentTableConfigurations[index];
            if (currentTableConfiguration.Item == null)
            {
                isItemsFull = false;
                var item = characterInvetory.GetItemByType(currentTableConfiguration.ItemType);
                if (item != null)
                {
                    customerTableUi.UpdateCheckMark(index, true);
                    item.IsSelectable = true;
                    item.ItemSelected();
                    item.transform.DOKill();
                    item.transform.DOJump(currentTableConfiguration.ItemPosition.position, 1.8f, 1, 0.6f);
                    item.transform.DORotateQuaternion(currentTableConfiguration.ItemPosition.rotation, 0.6f);
                    currentTableConfiguration.Item = item;
                    break;
                }
            }
        }

        if (isItemsFull)
        {
            GetItemCustomer();
        }
    }

    private void GetItemCustomer()
    {
        customerTableUi.EnableUI(0);
        foreach (var currentTableConfiguration in currentTableConfigurations)
        {
            currentTableConfiguration.Item.transform.DOKill();
            currentTableConfiguration.Item.transform.DOJump(customer.transform.position, 1.6f, 1, 0.6f).OnComplete(() =>
            {
                currentTableConfiguration.Item.ReturnToPool();
            });
        }

        customer.IsItemsGrab = true;
    }
}