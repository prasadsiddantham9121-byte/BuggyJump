using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public bool hasOpenClose; // ✅ true = open/close, false = single animation
    }

    public JetData jet; // ✅ ONLY ONE JET

    //======================================== Player ======================================================================

    public GameObject playerModel;   // your current animated player
    public GameObject ragdollModel;  // ragdoll version

    public Transform playerHips;
    public Transform ragdollHips;

    private Rigidbody playerRB;

    private CapsuleCollider capsule;
    private BoxCollider boxCollider;

    private bool isRagdollActive = false;


    // Start is called before the first frame update
    void Start()
    {

        CameraFollow cam = FindObjectOfType<CameraFollow>();

        if (cam != null)
        {
            cam.SetTarget(this.transform);
        }

        playerAnimator = GetComponentInChildren<Animator>();
        playerRB = GetComponent<Rigidbody>();

        // ✅ ASSIGN FIRST
        capsule = GetComponent<CapsuleCollider>();
        boxCollider = GetComponent<BoxCollider>();

        // ✅ Safety check
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

        // ✅ THEN use them
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

       
        // Switch colliders
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

    public void SadWalk()
    {
        playerAnimator.SetBool("IsWin", false);
    }
    
    public void HappyDance()
    {
        playerAnimator.SetBool("IsWin", true);
    }

    public void SetWin(bool win)
    {
        playerAnimator.SetBool("IsWin", win);
    }

    // ===================== JET FUNCTIONS =====================

    public void SetJet()
    {
        jetAnimator.runtimeAnimatorController = jet.overrideController;
        jetAnimator.speed = 1f;

        // ✅ For spin jet
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


    //======================================================= Activating Ragdoll ===========================================
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ActivateRagdoll();
            PlayerController player = GetComponent<PlayerController>();
            if (player != null)
            {
                player.DisableControls(); // 👈 disable controls here
            }

        }
    }

    public void ActivateRagdoll()
    {
        if (isRagdollActive) return;
        isRagdollActive = true;

        // Disable animator
        playerAnimator.enabled = false;

        // Disable player colliders (VERY IMPORTANT)
        capsule.enabled = false;
        boxCollider.enabled = false;

        // Disable player visuals
        playerModel.SetActive(false);

        TimeManager.instance.StopTimer();

        // ✅ FIND SAFE GROUND POSITION USING RAYCAST
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

        // Match rotation
        ragdollModel.transform.rotation = playerHips.rotation;

        // Enable ragdoll
        ragdollModel.SetActive(true);

        // Enable physics on ragdoll
        Rigidbody[] ragdollBodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // ✅ APPLY VELOCITY SAFELY (prevents glitch/explosion)
        StartCoroutine(ApplyVelocityDelayed());

        // ✅ SWITCH CAMERA
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        if (cam != null)
        {
            cam.SetTarget(ragdollHips);
        }

        // Disable player control
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
