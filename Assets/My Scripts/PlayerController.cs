using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float speedMovement = 5f;
    public float YawAmount = 120;

    public GameObject[] particleObjects;

    private float yaw;

    private Rigidbody rb;

    private float smoothHorizontal;
    private float smoothVertical;

    public float inputSmoothSpeed = 3f;

    [Header("Boost Settings")]
    public float boostMultiplier = 2f;
    public float boostDuration = 1.5f;

    public GameObject speedEffect;

    
    private bool isBoosting = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    //public void HandleMovement()
    //{
    //    //Forward Movement
    //    transform.position += transform.forward * speedMovement * Time.deltaTime;

    //    //Inputs
    //    float horizontalInput = Input.GetAxis("Horizontal");
    //    float verticalInput = Input.GetAxis("Vertical");

    //    //Yaw, Pitch And Roll Movements

    //    Yaw += horizontalInput * YewAmount * Time.deltaTime;
    //    float pitch = Mathf.Lerp(0,25, Mathf.Abs(verticalInput)) * -Mathf.Sign(verticalInput);
    //    float roll = Mathf.Lerp(0, 45, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

    //    //apply rotation
    //    transform.localRotation = Quaternion.Euler(Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * roll);

    //}

    //void HandleMovement()
    //{
    //    //Added newly
    //    float currentSpeed = isBoosting ? boostSpeed : speedMovement;

    //    transform.position += transform.forward * speedMovement * Time.deltaTime;


    //    float horizontal = Input.GetAxis("Horizontal");
    //    float vertical = Input.GetAxis("Vertical");


    //    smoothHorizontal = Mathf.Lerp(smoothHorizontal, horizontal, inputSmoothSpeed * Time.deltaTime);
    //    smoothVertical = Mathf.Lerp(smoothVertical, vertical, inputSmoothSpeed * Time.deltaTime);


    //    yaw += smoothHorizontal * YawAmount * Time.deltaTime;


    //    float pitch = Mathf.Lerp(0, 25f, Mathf.Abs(smoothVertical)) * -Mathf.Sign(smoothVertical);


    //    float roll = Mathf.Lerp(0, 45f, Mathf.Abs(smoothHorizontal)) * -Mathf.Sign(smoothHorizontal);


    //    Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);


    //    transform.rotation = Quaternion.Slerp(
    //        transform.rotation,
    //        targetRotation,
    //        4f * Time.deltaTime   // reduce this from 4 → 2 for smoother feel
    //    );
    //}

    void HandleMovement()
    {
        // Multiply speed instead of replacing
        float currentSpeed = isBoosting ? speedMovement * boostMultiplier : speedMovement;

        // APPLY SPEED ✅
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        smoothHorizontal = Mathf.Lerp(smoothHorizontal, horizontal, inputSmoothSpeed * Time.deltaTime);
        smoothVertical = Mathf.Lerp(smoothVertical, vertical, inputSmoothSpeed * Time.deltaTime);

        yaw += smoothHorizontal * YawAmount * Time.deltaTime;

        float pitch = Mathf.Lerp(0, 25f, Mathf.Abs(smoothVertical)) * -Mathf.Sign(smoothVertical);
        float roll = Mathf.Lerp(0, 45f, Mathf.Abs(smoothHorizontal)) * -Mathf.Sign(smoothHorizontal);

        //if (isBoosting)
        //{
        //    roll += boostRotationSpeed * Time.deltaTime;
        //}

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            2f * Time.deltaTime
        );

        
    }

    public void EnableControls()
    {
        StartCoroutine(EnableAfterDelay(1f));
       

        //StartCoroutine(SwitchCameraDelay());
        //rb.isKinematic = true;
        //rb.useGravity = false;

        //enabled = true;

    }
    
    public void DisableControls()
    {
        rb = GetComponent<Rigidbody>();

       rb.isKinematic = false;
       rb.useGravity = true;

       enabled = false;

    }

    private IEnumerator EnableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        enabled = true;

       
    }

    public void ParticlesEnable()
    {
        StartCoroutine(ShowPariclesWithDelay());
    }

    IEnumerator ShowPariclesWithDelay()
    {
        yield return new WaitForSeconds(2f);

        foreach (GameObject obj in particleObjects)
        {
            obj.SetActive(true);
        }
    }

    public void ParticlesDisable()
    {
        foreach (GameObject obj in particleObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ActivateBoost()
    {
        if (!isBoosting)
            StartCoroutine(BoostRoutine());
    }

    IEnumerator BoostRoutine()
    {
        isBoosting = true;
        speedEffect.SetActive(true);

        // 💥 instant forward push
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);

        yield return new WaitForSeconds(boostDuration);

        isBoosting = false;
        speedEffect.SetActive(false);

    }


}
