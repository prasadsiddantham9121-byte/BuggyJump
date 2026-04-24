using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound("Collectable");
            other.GetComponent<PlayerController>().ActivateBoost();
            Destroy(gameObject);
        }
    }
}
