using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform target;
    public float distance = 5f;   // how far behind
    public float height = 2f;     // height from player
    public float smoothSpeed = 5f;

    void Start()
    {
        // ✅ Auto find player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("No object with tag 'Player' found!");
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

    // ✅ This is the key function
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
