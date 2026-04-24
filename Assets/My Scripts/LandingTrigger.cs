using UnityEngine;
using TMPro;
using System.Collections;
using Script;

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

        UI_Canvas.instance.isGameOver = true;
        if (!AndroidTV.IsAndroidOrFireTv())
        {
            UI_Canvas.instance.pauseButton.SetActive(false);
        }

        hasTriggered = true;
        GetComponent<Collider>().enabled = false;

        Debug.Log("Player Landed Here");
        UI_Canvas.instance.InGameAudioStop();
        SoundManager.instance.StopSound("Flying");
        SoundManager.instance.PlaySound("Jet_Closing");
        SoundManager.instance.PlaySound("Level_Pass");
        PlayerController controller = other.GetComponent<PlayerController>();
        PlayerVisualController visual = other.GetComponent<PlayerVisualController>();

        if (visual != null)
        {
            visual.OnLevelComplete();
        }

       
        GameObject landingCamera = visual.landingCamera;
        GameObject mainCamera = visual.mainCamera;

        
        visual.JetCloseAnimation();
        //visual.StopJetAnimation();

        controller.DisableControls();
        controller.ParticlesDisable();

       
        if (mainCamera != null) mainCamera.SetActive(false);
        if (landingCamera != null) landingCamera.SetActive(true);

       
        TimeManager.instance.StopTimer();

       

        StartCoroutine(HandleLandingSequence(controller, visual));

        if (CheckPointManager.instance != null)
        {
            CheckPointManager.instance.DiActivateLandingEffect();
        }

        if (RingPointManager.instance != null)
        {
            RingPointManager.instance.RingOffLandingEffect();
        }
        Debug.Log("Checkpoint instance: " + CheckPointManager.instance);
        Debug.Log("Ring instance: " + RingPointManager.instance);
    }

    IEnumerator HandleLandingSequence(PlayerController controller, PlayerVisualController visual)
    {
       
        yield return StartCoroutine(screenFader.FadeOut());


        visual.StopJetAnimation();
        visual.TriggerLandingAnimation();
       
      
        controller.DoLandingToResultsPos();

        yield return StartCoroutine(screenFader.FadeIn(0.8f));
    }

}