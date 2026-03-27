using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    private bool isCollected = false;
    private CheckPointManager manager;

    private void Start()
    {
        manager = FindObjectOfType<CheckPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;

            if (manager != null)
            {
                manager.CollectCheckpoint();
            }
            else
            {
                Debug.LogError("CheckPointManager not found in scene!");
            }

            // Disable or destroy checkpoint
            gameObject.SetActive(false);
            // Destroy(gameObject); // optional
        }
    }


}