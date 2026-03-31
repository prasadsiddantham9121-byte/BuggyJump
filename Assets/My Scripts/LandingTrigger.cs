using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingTrigger : MonoBehaviour
{
    public GameObject landingCamera;
    public GameObject mainCamera;

    private CheckPointManager cpManager;

    //private Animator anim;

    private void Start()
    {
        cpManager = FindObjectOfType<CheckPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player Landed Here");

        PlayerController controller = other.GetComponent<PlayerController>();
        PlayerVisualController visual = other.GetComponent<PlayerVisualController>();

        // Close jet
        visual.JetCloseAnimation();

        int totalCP = cpManager.levels[cpManager.currentLevel].checkpoints.Length;
        int currentCP = PlayerPrefs.GetInt("CheckPoint");

        // Disable player controls
        controller.DisableControls();
        controller.ParticlesDisable();

        // Switch camera
        mainCamera.SetActive(false);
        landingCamera.SetActive(true);

        // Trigger landing animation
        visual.TriggerLandingAnimation();

        // ✅ Set result (IMPORTANT)
        if (currentCP < totalCP)
        {
            Debug.Log("Level Failed! Missed checkpoints.");
            visual.SetWin(false);   // will go to Sad Walk AFTER landing
            UI_Canvas.instance.ShowLevelFail();
        }
        else
        {
            Debug.Log("Player Landed Successfully!");
            visual.SetWin(true);    // will go to Dance AFTER landing
            UI_Canvas.instance.ShowLevelPass();
        }

        // Stop timer
        TimeManager.instance.StopTimer();
    }
}
