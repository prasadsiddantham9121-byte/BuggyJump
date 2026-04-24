using UnityEngine;

public class Ring : MonoBehaviour
{
    private bool collected = false;

    private Collider ringCollider;

    private void OnEnable()
    {
        collected = false;

        if (ringCollider == null)
            ringCollider = GetComponent<Collider>();

        ringCollider.enabled = true;
    }

    private void Start()
    {
        ringCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {

            SoundManager.instance.PlaySound("Collectable");
            collected = true;

            ringCollider.enabled = false;

            RingPointManager.instance.CollectRing(gameObject);

            gameObject.SetActive(false);
        }
    }
}
