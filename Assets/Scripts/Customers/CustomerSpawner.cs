using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerNPCStateMachine prefab;

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform tablePosition;
    [SerializeField] private CustomersTable customersTable;

    private IPool<CustomerNPCStateMachine> pool;

    public static CustomerSpawner Instance;

    private void Awake()
    {
        var factory = new FactoryMonoObject<CustomerNPCStateMachine>(prefab.gameObject, transform);
        pool = new Pool<CustomerNPCStateMachine>(factory, 3);
        SpawnCustomer();
        Instance = this;
    }

    public void SpawnCustomer()
    {
        var randomItemAmount = Random.Range(1, 4);
        var itemList = new List<TableConfiguration>();
        for (int i = 0; i < randomItemAmount; i++)
        {
            var items = Enum.GetValues(typeof(ItemType));
            var randomIndex = Random.Range(0, items.Length);
            var randomItem = (ItemType) items.GetValue(randomIndex);
            while (randomItem == ItemType.Clear)
            {
                items = Enum.GetValues(typeof(ItemType));
                randomIndex = Random.Range(0, items.Length);
                randomItem = (ItemType) items.GetValue(randomIndex);
            }

            var newTableConfiguration = new TableConfiguration {ItemType = randomItem};
            itemList.Add(newTableConfiguration);
        }

        var newCustomer = pool.Pull();
        newCustomer.transform.position = spawnPosition.position;
        newCustomer.transform.rotation = spawnPosition.rotation;
        newCustomer.InitializeCustomer(itemList, spawnPosition, tablePosition, customersTable);
    }
}