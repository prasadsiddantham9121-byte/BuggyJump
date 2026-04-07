using UnityEngine;
using TMPro;
using System.Collections;

public class LandingTrigger : MonoBehaviour
{
    public static LandingTrigger instance;

    private ScreenFader screenFader;

    public TextMeshProUGUI missedCP;

    public Transform startPoint;
    public Transform endPoint;

    private bool hasTriggered = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        screenFader = FindObjectOfType<ScreenFader>();
        missedCP.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        GetComponent<Collider>().enabled = false;

        Debug.Log("Player Landed Here");

        PlayerController controller = other.GetComponent<PlayerController>();
        PlayerVisualController visual = other.GetComponent<PlayerVisualController>();

        if (visual != null)
        {
            visual.OnLevelComplete(); // 🔥 THIS LINE IS REQUIRED
        }

        // ✅ Cameras
        GameObject landingCamera = visual.landingCamera;
        GameObject mainCamera = visual.mainCamera;

        // ✅ Stop jet & controls
        visual.JetCloseAnimation();
        visual.StopJetAnimation();

        controller.DisableControls();
        controller.ParticlesDisable();

        // ✅ Switch cameras
        if (mainCamera != null) mainCamera.SetActive(false);
        if (landingCamera != null) landingCamera.SetActive(true);

        // ✅ Landing animation
        

        // ✅ Stop timer
        TimeManager.instance.StopTimer();

        //StartCoroutine(screenFader.FadeOut());

        //visual.TriggerLandingAnimation();

        //controller.DoLandingToResultsPos();

        StartCoroutine(HandleLandingSequence(controller, visual));

        if (CheckPointManager.instance != null)
        {
            CheckPointManager.instance.DiActivateLandingEffect();
        }
        Debug.Log("Checkpoint instance: " + CheckPointManager.instance);
        Debug.Log("Ring instance: " + RingPointManager.instance);
    }

    IEnumerator HandleLandingSequence(PlayerController controller, PlayerVisualController visual)
    {
        //StopAllCoroutines();

        // Fade out first
        yield return StartCoroutine(screenFader.FadeOut());
       

        // Then play landing animation
        visual.TriggerLandingAnimation();
       
        // Small delay if needed
        //yield return new WaitForSeconds(0.2f);

        controller.DoLandingToResultsPos();

        yield return StartCoroutine(screenFader.FadeIn(0.8f));
    }

}