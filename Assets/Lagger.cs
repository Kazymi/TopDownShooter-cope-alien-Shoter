using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lagger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var list = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                list.Add(Random.Range(0, 100));
                Sort(list);
            }
        }
    }

    private void Sort(List<int> list)
    {
        list.Sort();
    }
}