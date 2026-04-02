using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
   

    public Animator playerAnimator;

    public Animator jetAnimator;

    public GameObject playerModel;   // your current animated player
    public GameObject ragdollModel;  // ragdoll version

    // 🔥 NEW (IMPORTANT)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggorJumpAnimation()
    {
        playerAnimator.SetBool("JumpAway", true);

        playerRB.isKinematic = false;
        playerRB.useGravity = true;
    }

    //Coroutine flyingRoutine;

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

    public void JetOpenAnimation()
    {
        StartCoroutine(JetOpenWithDelay());
    }

    IEnumerator JetOpenWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        jetAnimator.SetBool("Jet_Open", true);
    }
    
    public void JetCloseAnimation()
    {
        jetAnimator.SetBool("Jet_Close", true);

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

    //public void ActivateRagdoll()
    //{
    //    playerAnimator.enabled = false;

    //    // Disable player visuals + control
    //    playerModel.SetActive(false);

    //    // Enable ragdoll
    //    ragdollModel.SetActive(true);

    //    // Match position & rotation
    //    ragdollModel.transform.position = playerModel.transform.position;
    //    ragdollModel.transform.rotation = playerModel.transform.rotation;

    //    // Transfer velocity (important for realism)
    //    Rigidbody[] ragdollBodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

    //    foreach (Rigidbody rb in ragdollBodies)
    //    {
    //        rb.velocity = playerRB.velocity;
    //    }
    //}

    //public void ActivateRagdoll()
    //{
    //    isRagdollActive = true;

    //    playerAnimator.enabled = false;

    //    // ✅ BEST POSITION FIX (HIPS ALIGNMENT)
    //    Vector3 offset = ragdollModel.transform.position - ragdollHips.position;

    //    ragdollModel.transform.position =
    //        playerHips.position + offset + Vector3.up * 0.2f;

    //    ragdollModel.transform.rotation = playerHips.rotation;

    //    // Disable player
    //    playerModel.SetActive(false);

    //    // Enable ragdoll
    //    ragdollModel.SetActive(true);

    //    // ✅ Enable physics properly
    //    Rigidbody[] ragdollBodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

    //    foreach (Rigidbody rb in ragdollBodies)
    //    {
    //        rb.isKinematic = false;
    //        rb.useGravity = true;
    //        rb.velocity = playerRB.velocity;
    //    }

    //    // ✅ SWITCH CAMERA
    //    CameraFollow cam = FindObjectOfType<CameraFollow>();
    //    if (cam != null)
    //    {
    //        cam.SetTarget(ragdollModel.transform);
    //    }
    //}

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
