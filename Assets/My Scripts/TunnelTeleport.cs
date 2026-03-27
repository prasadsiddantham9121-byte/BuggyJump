using System.Collections;
using UnityEngine;

public class TunnelTeleport : MonoBehaviour
{
    public Transform exitPoint;          // Where player will teleport
    public GameObject exitTrigger;       // Exit trigger GameObject

    [Header("Teleport Settings")]
    public float teleportCooldown = 1f;
    public float exitActiveTime = 2f;    // How long exit stays active

    private bool canTeleport = true;

    private void Start()
    {
        if (exitTrigger != null)
            exitTrigger.SetActive(false); // Start with exit OFF
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canTeleport)
        {
            StartCoroutine(Teleport(other));
        }
    }

    IEnumerator Teleport(Collider other)
    {
        canTeleport = false;

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        // 👉 TELEPORT
        other.transform.position = exitPoint.position;

        if (cc != null)
            cc.enabled = true;

        // 👉 ACTIVATE EXIT TEMPORARILY
        if (exitTrigger != null)
        {
            exitTrigger.SetActive(true);
            yield return new WaitForSeconds(exitActiveTime);
            exitTrigger.SetActive(false);
        }

        // 👉 COOLDOWN BEFORE NEXT TELEPORT
        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;
    }
}