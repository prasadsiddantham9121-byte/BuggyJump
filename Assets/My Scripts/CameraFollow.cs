using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 5f;

    void Start()
    {
        // 🔍 Auto find player by tag
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            print("Player has been Assighned");
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position
                                - target.forward * distance
                                + Vector3.up * height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}