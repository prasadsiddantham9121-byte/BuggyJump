using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaneMovement : MonoBehaviour
{
    public GameObject cutSceneCamera;
    public GameObject mainCamera;
    public Transform lookTarget;

    public PlayerVisualController playerVisualController;

    [Header("Jump Delay Settings")]
    public float jumpDelay = 2f;

    public ScreenFader screenFader;

    private bool jumped = false;

    void Start()
    {
        playerVisualController = GetComponentInChildren<PlayerVisualController>();

        // 👇 Start coroutine delay
        StartCoroutine(JumpWithDelay());
    }

    void Update()
    {
        // Camera looks at cube during cutscene
        if (mainCamera.activeSelf && lookTarget != null)
        {
            mainCamera.transform.LookAt(lookTarget);
        }
    }

    IEnumerator JumpWithDelay()
    {
        yield return new WaitForSeconds(jumpDelay);

       

        JumpPlayer();

        // Fade to black
        yield return StartCoroutine(screenFader.FadeOut());

        //cutSceneCamera.SetActive(false);

    }

    void JumpPlayer()
    {
        if (jumped) return;

        Debug.Log("JUMP TRIGGERED");

        jumped = true;

        playerVisualController.transform.parent = null;
        playerVisualController.TriggorJumpAnimation();


        mainCamera.SetActive(true);

        Invoke(nameof(DisablePlane), 1f);
    }

    void DisablePlane()
    {
        gameObject.SetActive(false);
    }
}