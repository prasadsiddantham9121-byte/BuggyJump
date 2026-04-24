using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script;

public class PlayerVisualController : MonoBehaviour
{
    //======================================== Jet ======================================================================

    public Animator playerAnimator;
    public Animator jetAnimator;

    [Header("Camera SetUp")]
    public GameObject landingCamera;
    public GameObject mainCamera;

    [System.Serializable]
    public class JetData
    {
        public AnimatorOverrideController overrideController;
        public bool hasOpenClose;
    }

    public JetData jet;

    //======================================== Player ======================================================================

    public GameObject playerModel;  
    public GameObject ragdollModel;  

    public Transform playerHips;
    public Transform ragdollHips;

    private Rigidbody playerRB;

    private CapsuleCollider capsule;
    private BoxCollider boxCollider;

    private bool isRagdollActive = false;

    private bool isLevelFinished = false;

    void Start()
    {

        CameraFollow cam = FindObjectOfType<CameraFollow>();

        if (cam != null)
        {
            cam.SetTarget(this.transform);
        }

        playerAnimator = GetComponentInChildren<Animator>();
        playerRB = GetComponent<Rigidbody>();

       
        capsule = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();

       
        if (capsule == null)
        {
            Debug.LogError("CapsuleCollider missing on Player!");
            return;
        }

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider missing on Player!");
            return;
        }

       
        capsule.enabled = true;
        boxCollider.enabled = false;
    }

    //======================================== Player Animations ======================================================================


    public void TriggorJumpAnimation()
    {
        playerAnimator.SetBool("JumpAway", true);

        playerRB.isKinematic = false;
        playerRB.useGravity = true;
    }

    public void TriggerFlyingAnimation()
    {
        playerAnimator.SetBool("Flying", true);

       
        
        capsule.enabled = false;
        boxCollider.enabled = true;
    }

    public void TriggerLandingAnimation()
    {
        playerAnimator.SetTrigger("Landing");

        playerAnimator.SetBool("Flying", false);
        playerAnimator.SetBool("JumpAway", false);

       
        capsule.enabled = true;
        boxCollider.enabled = false;
    }

   
    public void PlayResult(bool win)
    {
        playerAnimator.SetBool("IsWin", win);
    }

    // ===================== JET FUNCTIONS =====================

    public void SetJet()
    {
        jetAnimator.runtimeAnimatorController = jet.overrideController;
        jetAnimator.speed = 1f;

       
        if (!jet.hasOpenClose)
        {
            jetAnimator.Play("Jet_Open_Anim", 0, 0f);
        }
    }


    public void JetOpenAnimation()
    {
        StartCoroutine(JetOpenWithDelay());
    }

    IEnumerator JetOpenWithDelay()
    {
        yield return new WaitForSeconds(1.5f);

        if (jet.hasOpenClose)
        {
            jetAnimator.SetTrigger("Jet_Open");
        }
    }

    public void JetCloseAnimation()
    {
        if (jet.hasOpenClose)
        {
            jetAnimator.SetTrigger("Jet_Close");
        }
    }

    public void StopJetAnimation()
    {
        if (!jet.hasOpenClose)
        {
            jetAnimator.speed = 0f;
        }
    }

    public void OnLevelComplete()
    {
        isLevelFinished = true;

        // Optional: stop movement physics cleanly
        playerRB.velocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;
        playerRB.isKinematic = true;
    }

    //======================================================= Activating Ragdoll ===========================================
    private void OnCollisionEnter(Collision collision)
    {
        if (isLevelFinished) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            ActivateRagdoll();
            PlayerController player = GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableControls(); 
            }

        }
    }

    public void ActivateRagdoll()
    {
        if (isRagdollActive || isLevelFinished) return;

        UI_Canvas.instance.isGameOver = true;

        if (!AndroidTV.IsAndroidOrFireTv())
        {
            UI_Canvas.instance.pauseButton.SetActive(false);
        }

        isRagdollActive = true;


        playerAnimator.enabled = false;

        
        capsule.enabled = false;
        boxCollider.enabled = false;

       
        playerModel.SetActive(false);

        TimeManager.instance.StopTimer();

       
        RaycastHit hit;
        Vector3 startPos = playerHips.position + Vector3.up * 1.5f;

        if (Physics.Raycast(startPos, Vector3.down, out hit, 5f))
        {
            ragdollModel.transform.position = hit.point + Vector3.up * 0.3f;
        }
        else
        {
            ragdollModel.transform.position = playerHips.position + Vector3.up * 0.3f;
        }

      
        ragdollModel.transform.rotation = playerHips.rotation;

       
        ragdollModel.SetActive(true);

       
        Rigidbody[] ragdollBodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

       
        StartCoroutine(ApplyVelocityDelayed());

       
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        if (cam != null)
        {
            cam.SetTarget(ragdollHips);
        }

       
        PlayerController player = GetComponent<PlayerController>();
        if (player != null)
        {
            player.DisableControls();
        }

        UI_Canvas.instance.ShowLevelFail();
    }

    IEnumerator ApplyVelocityDelayed()
    {
        yield return new WaitForFixedUpdate();

        Rigidbody[] bodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in bodies)
        {
            rb.velocity = playerRB.velocity;
        }
    }

}
