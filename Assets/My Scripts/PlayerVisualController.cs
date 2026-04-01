using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
   

    private Animator playerAnimator;

    public Animator jetAnimator;

    public GameObject playerModel;   // your current animated player
    public GameObject ragdollModel;  // ragdoll version

    private Rigidbody playerRB;

    private CapsuleCollider capsule;
    private BoxCollider boxCollider;




    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
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
        }
    }

    public void ActivateRagdoll()
    {
        playerAnimator.enabled = false;

        // Disable player visuals + control
        playerModel.SetActive(false);

        // Enable ragdoll
        ragdollModel.SetActive(true);

        // Match position & rotation
        ragdollModel.transform.position = playerModel.transform.position;
        ragdollModel.transform.rotation = playerModel.transform.rotation;

        // Transfer velocity (important for realism)
        Rigidbody[] ragdollBodies = ragdollModel.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.velocity = playerRB.velocity;
        }
    }

}
