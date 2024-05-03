using System.Collections;
using UnityEngine;

public class PlayerHandInventory : MonoBehaviour
{
    public void Initialize(PlayerListener playerListener)
    {
        StartCoroutine(UpdateClosestObject(playerListener));
    }

    private IEnumerator UpdateClosestObject(PlayerListener playerListener)
    {
        while (true)
        {
            if (playerListener.LastObject != null)
            {
                yield return new WaitForSeconds(0.3f);
                playerListener.UpdateClosestHandItem();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                playerListener.UpdateClosestHandItem();
            }
        }
    }
}