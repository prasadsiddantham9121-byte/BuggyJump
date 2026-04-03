using UnityEngine;

public class Ring : MonoBehaviour
{
    private bool collected = false;

    private Collider ringCollider;

    private void Start()
    {
        ringCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            CheckPointManager.instance.CollectRing(gameObject);

            ringCollider.enabled = false;
        }
    }
}