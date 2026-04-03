using UnityEngine;
using TMPro;
using System.Collections;

public class LandingTrigger : MonoBehaviour
{
    public TextMeshProUGUI missedCP;

    private CheckPointManager cpManager;

    private void Start()
    {
        cpManager = FindObjectOfType<CheckPointManager>();

        missedCP.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player Landed Here");

        PlayerController controller = other.GetComponent<PlayerController>();
        PlayerVisualController visual = other.GetComponent<PlayerVisualController>();

        // ✅ Get cameras from ACTIVE PLAYER
        GameObject landingCamera = visual.landingCamera;
        GameObject mainCamera = visual.mainCamera;

        // Close jet
        visual.JetCloseAnimation();
        visual.StopJetAnimation();

        int totalCP = cpManager.levelsCheckPoints[cpManager.currentLevel].checkpoints.Length;
        int currentCP = PlayerPrefs.GetInt("CheckPoint");

        // Disable player controls
        controller.DisableControls();
        controller.ParticlesDisable();

        // ✅ Switch camera (correct one)
        if (mainCamera != null) mainCamera.SetActive(false);
        if (landingCamera != null) landingCamera.SetActive(true);

        // Trigger landing animation
        visual.TriggerLandingAnimation();

        // ✅ Set result
        if (currentCP < totalCP)
        {
            Debug.Log("Level Failed! Missed checkpoints.");
            visual.SetWin(false);
            missedCP.gameObject.SetActive(true);
            UI_Canvas.instance.ShowLevelFail();
        }
        else
        {
            Debug.Log("Player Landed Successfully!");
            visual.SetWin(true);
            UI_Canvas.instance.ShowLevelPass();
        }

        // Stop timer
        TimeManager.instance.StopTimer();
    }
}