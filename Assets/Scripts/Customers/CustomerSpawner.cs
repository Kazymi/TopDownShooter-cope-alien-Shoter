using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private CustomerConfiguration[] customerConfigurations;
    [SerializeField] private CustomerNPCStateMachine prefab;

    private IPool<CustomerNPCStateMachine> pool;
    
    private void Awake()
    {
        var factory = new FactoryMonoObject<CustomerNPCStateMachine>(prefab.gameObject, transform);
        pool = new Pool<CustomerNPCStateMachine>(factory, 3);
        
        foreach (var customerConfiguration in customerConfigurations)
        {
            customerConfiguration.Initialize(pool);
        }
    }
}

[Serializable]
public class CustomerConfiguration
{
    [field:SerializeField] public Transform SpawnPosition { get; private set; }
    [field:SerializeField] public Transform TablePosition { get; private set; }
    [field:SerializeField] public CustomersTable CustomersTable { get; private set; }
    
    private IPool<CustomerNPCStateMachine> pool;

    public void Initialize(IPool<CustomerNPCStateMachine> pool)
    {
        this.pool = pool;
        SpawnCustomer();
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
        
        newCustomer.transform.position = SpawnPosition.position;
        newCustomer.transform.rotation = SpawnPosition.rotation;
        newCustomer.InitializeCustomer(itemList, SpawnPosition, TablePosition, CustomersTable,this);
    }
}