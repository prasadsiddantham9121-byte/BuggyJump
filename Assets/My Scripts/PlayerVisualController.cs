using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
   

    private Animator playerAnimator;

    public Animator jetAnimator;

    private Rigidbody playerRB;

   
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        
        playerRB = GetComponent<Rigidbody>();
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

    public void TriggerFlyingAnimation()
    {
        playerAnimator.SetBool("Flying", true);
    }

    public void TriggerLandingAnimation()
    {
        playerAnimator.SetTrigger("Landing");

        playerAnimator.SetBool("Flying", false);
        playerAnimator.SetBool("JumpAway", false);
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

}
