using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public Transform target;   
    //public Vector3 offset;    
    //public float smoothSpeed = 5f;

    //void FixedUpdate()
    //{
    //    if (target == null) return;


    //    Vector3 desiredPosition = target.position + offset;


    //    Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);


    //    transform.position = smoothedPosition;


    //    transform.LookAt(target);
    //}
    public Transform target;
    public float distance = 5f;   // how far behind
    public float height = 2f;     // height from player
    public float smoothSpeed = 5f;

    void FixedUpdate()
    {
        if (target == null) return;

        // 📍 Position behind player
        Vector3 desiredPosition = target.position
                                - target.forward * distance
                                + Vector3.up * height;

        // 🧠 Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // 👀 Look at player
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
