using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    private bool isCollected = false;



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isCollected)
            return;

        isCollected = true;

        CheckPointManager.instance.CollectCheckpoint(gameObject);
    }


}
