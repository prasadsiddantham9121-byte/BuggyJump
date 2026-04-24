using System.Collections;
using UnityEngine;

public class CharacterSelectAnim : MonoBehaviour
{
    public Animator animator;
    public GameObject effectObject;

    [Header("Laugh Timing")]
    public float minDelay = 5f;
    public float maxDelay = 12f;

    void OnEnable()
    {
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        // reset effect
        if (effectObject != null)
        {
            effectObject.SetActive(false);
        }

        StartCoroutine(RandomLaugh());
    }

    void OnDisable()
    {
        StopAllCoroutines();

        // force stop effect when switching
        if (effectObject != null)
        {
            effectObject.SetActive(false);
        }
    }

    IEnumerator RandomLaugh()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));


            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("onFire"))
            {
                animator.SetTrigger("onFire");
            }

        }
    }

    public void EnableFire()
    {
        effectObject.SetActive(true);
    }

    public void DisableFire()
    {
        effectObject.SetActive(false);
    }
}