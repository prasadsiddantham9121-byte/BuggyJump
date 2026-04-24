using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Script;
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
    public bool isResults;

    public Joystick joystick;

    // ================= CONTROL MODE =================
    public enum ControlMode
    {
        Auto,
        TV,
        Mobile,
        Both
    }

    [Header("Control Mode")]
    public ControlMode controlMode = ControlMode.Auto;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (joystick == null  && UI_Canvas.instance != null)
        {
            joystick = UI_Canvas.instance.joystick;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    //void HandleMovement()
    //{
    //    if (isResults)
    //        return;

    //    float currentSpeed = isBoosting ? speedMovement * boostMultiplier : speedMovement;


    //    transform.position += transform.forward * currentSpeed * Time.deltaTime;
    //    if (AndroidTV.IsAndroidOrFireTv())
    //    {


    //        float horizontal = Input.GetAxis("Horizontal");
    //        float vertical = Input.GetAxis("Vertical");

    //        smoothHorizontal = Mathf.Lerp(smoothHorizontal, horizontal, inputSmoothSpeed * Time.deltaTime);
    //        smoothVertical = Mathf.Lerp(smoothVertical, vertical, inputSmoothSpeed * Time.deltaTime);

    //        yaw += smoothHorizontal * YawAmount * Time.deltaTime;

    //        float pitch = Mathf.Lerp(0, 25f, Mathf.Abs(smoothVertical)) * -Mathf.Sign(smoothVertical);
    //        float roll = Mathf.Lerp(0, 45f, Mathf.Abs(smoothHorizontal)) * -Mathf.Sign(smoothHorizontal);



    //        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);
    //    }
    //    else
    //    {


    //        float horizontal = joystick.Horizontal;
    //        float vertical = joystick.Vertical;

    //        smoothHorizontal = Mathf.Lerp(smoothHorizontal, horizontal, inputSmoothSpeed * Time.deltaTime);
    //        smoothVertical = Mathf.Lerp(smoothVertical, vertical, inputSmoothSpeed * Time.deltaTime);

    //        yaw += smoothHorizontal * YawAmount * Time.deltaTime;

    //        float pitch = Mathf.Lerp(0, 25f, Mathf.Abs(smoothVertical)) * -Mathf.Sign(smoothVertical);
    //        float roll = Mathf.Lerp(0, 45f, Mathf.Abs(smoothHorizontal)) * -Mathf.Sign(smoothHorizontal);

    //        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);
    //    }
    //}

    void HandleMovement()
    {
        if (isResults)
            return;

        float currentSpeed = isBoosting ? speedMovement * boostMultiplier : speedMovement;

        // Forward movement
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        float horizontal = 0f;
        float vertical = 0f;

        // 🔥 CONTROL MODE HANDLING
        switch (controlMode)
        {
            case ControlMode.TV:
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
                break;

            case ControlMode.Mobile:
                if (joystick != null)
                {
                    horizontal = joystick.Horizontal;
                    vertical = joystick.Vertical;
                }
                
                break;

            case ControlMode.Both:
                float jH = joystick != null ? joystick.Horizontal : 0f;
                float jV = joystick != null ? joystick.Vertical : 0f;

                horizontal = Input.GetAxis("Horizontal") + jH;
                vertical = Input.GetAxis("Vertical") + jV;
                break;

            case ControlMode.Auto:
                if (AndroidTV.IsAndroidOrFireTv())
                {
                    horizontal = Input.GetAxis("Horizontal");
                    vertical = Input.GetAxis("Vertical");
                }
                else
                {
                    if (joystick != null)
                    {
                        horizontal = joystick.Horizontal;
                        vertical = joystick.Vertical;
                    }
                    
                }
                break;
        }

        // Clamp (important for BOTH mode)
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        // Smooth input
        smoothHorizontal = Mathf.Lerp(smoothHorizontal, horizontal, inputSmoothSpeed * Time.deltaTime);
        smoothVertical = Mathf.Lerp(smoothVertical, vertical, inputSmoothSpeed * Time.deltaTime);

        // Rotation
        yaw += smoothHorizontal * YawAmount * Time.deltaTime;

        float pitch = Mathf.Lerp(0, 25f, Mathf.Abs(smoothVertical)) * -Mathf.Sign(smoothVertical);
        float roll = Mathf.Lerp(0, 45f, Mathf.Abs(smoothHorizontal)) * -Mathf.Sign(smoothHorizontal);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2.5f * Time.deltaTime);
    }

    // ================= CONTROL ENABLE / DISABLE =================

    public void EnableControls()
    {
        StartCoroutine(EnableAfterDelay(1f));
       
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

        rb.isKinematic = false;
        rb.useGravity = false;

        enabled = true;

       
    }

    // ================= PARTICLES =================

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


    // ================= BOOST =================
    public void ActivateBoost()
    {
        if (!isBoosting)
            StartCoroutine(BoostRoutine());
    }

    IEnumerator BoostRoutine()
    {
        isBoosting = true;
        SoundManager.instance.PlaySound("Booster");
        speedEffect.SetActive(true);

        // 💥 instant forward push
        rb.AddForce(transform.forward * 10f, ForceMode.Impulse);

        yield return new WaitForSeconds(boostDuration);

        isBoosting = false;
        SoundManager.instance.StopSound("Booster");
        speedEffect.SetActive(false);

    }


    // ================= LANDING =================
    public void DoLandingToResultsPos()
    {
        isResults = true;
        Transform startPt = LandingTrigger.instance.startPoint;
        Transform endPt = LandingTrigger.instance.endPoint;

        //transform.eulerAngles = Vector3.zero;
        transform.rotation = startPt.rotation;
        transform.position = startPt.position;

        transform.DOMove(endPt.position, 0.8f).onComplete+=OnReachedEndPoint;
    }


    void OnReachedEndPoint()
    {
        PlayerVisualController visual = GetComponent<PlayerVisualController>();

        if (visual == null) return;

        // ================= LEVEL RESULT =================

        if (CheckPointManager.instance != null &&
            CheckPointManager.instance.GetTotal() > 0)
        {
            if (CheckPointManager.instance.AreAllCheckpointsCollected())
            {

                visual.PlayResult(true);
                UI_Canvas.instance.ShowLevelPass();
                int levelCompletionReward = 50;
                CoinManager.instance.AddLevelCompletionCoins(levelCompletionReward);

            }
            else
            {
                visual.PlayResult(false);

                int missed = CheckPointManager.instance.GetTotal() -
                             CheckPointManager.instance.GetCurrent();

                SoundManager.instance.StopSound("Flying");
                SoundManager.instance.PlaySound("Level_Fail");
                LandingTrigger.instance.missedCP.gameObject.SetActive(true);
                LandingTrigger.instance.missedCP.text =
                    " You Missed " + missed + " CheckPoints! ";

                UI_Canvas.instance.ShowLevelFail();

            }
        }
        else if (RingPointManager.instance != null &&
                 RingPointManager.instance.GetTotal() > 0)
        {
            if (RingPointManager.instance.AreAllRingsCollected())
            {
                visual.PlayResult(true);
                UI_Canvas.instance.ShowLevelPass();
                int levelCompletionReward = 50;
                CoinManager.instance.AddLevelCompletionCoins(levelCompletionReward);
            }
            else
            {
                visual.PlayResult(false);

                int missed = RingPointManager.instance.GetTotal() -
                             RingPointManager.instance.GetCurrent();

                SoundManager.instance.StopSound("Flying");
                SoundManager.instance.PlaySound("Level_Fail");
                LandingTrigger.instance.missedCP.gameObject.SetActive(true);
                LandingTrigger.instance.missedCP.text =
                    " You Missed " + missed + " Rings! ";

                UI_Canvas.instance.ShowLevelFail();
                LandingTrigger.instance.missedCP.gameObject.SetActive(false);

            }
        }
    }
    

}
