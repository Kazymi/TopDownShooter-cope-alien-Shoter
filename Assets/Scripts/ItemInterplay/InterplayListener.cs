using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterplayListener : MonoBehaviour
{
    public void Initialize(PlayerListener playerListener)
    {
        StartCoroutine(UpdateClosestObject(playerListener));
    }

    private IEnumerator UpdateClosestObject(PlayerListener playerListener)
    {
        Debug.Log("Start");
        while (true)
        {
            if (playerListener.LastObject != null)
            {
                yield return new WaitForSeconds(0.3f);
                playerListener.UpdateClosestInterplayObject();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                playerListener.UpdateClosestInterplayObject();
            }
        }
    }
}