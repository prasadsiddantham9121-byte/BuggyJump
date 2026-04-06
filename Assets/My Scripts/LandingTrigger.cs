using UnityEngine;
using TMPro;
using System.Collections;

public class LandingTrigger : MonoBehaviour
{
    public TextMeshProUGUI missedCP;

    private void Start()
    {
        missedCP.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

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
        visual.TriggerLandingAnimation();

        // ✅ Stop timer
        TimeManager.instance.StopTimer();

        if (CheckPointManager.instance != null)
        {
            CheckPointManager.instance.DiActivateLandingEffect();
        }
        Debug.Log("Checkpoint instance: " + CheckPointManager.instance);
        Debug.Log("Ring instance: " + RingPointManager.instance);

        // ================= LEVEL RESULT =================


        if (CheckPointManager.instance != null &&
            CheckPointManager.instance.GetTotal() > 0)
        {
            if (CheckPointManager.instance.AreAllCheckpointsCollected())
            {
                print("Level Pass");

                visual.PlayResult(true);

                UI_Canvas.instance.ShowLevelPass();
            }
            else
            {
                print("Level Fail");

                visual.PlayResult(false);

                missedCP.gameObject.SetActive(true);

                int missed = GetMissedCount();
                missedCP.text = " You Missed " + missed + " CheckPoints! ";

                UI_Canvas.instance.ShowLevelFail();
            }
        }

        else if (RingPointManager.instance != null &&
                  RingPointManager.instance.GetTotal() > 0)
        {

            if(RingPointManager.instance !=null &&
                RingPointManager.instance.GetTotal() > 0)


            if (RingPointManager.instance.AreAllRingsCollected())
            {
                print("Level Pass");
                visual.PlayResult(true);
                UI_Canvas.instance.ShowLevelPass();
            }
            else
            {
                print("Level Fail");

                visual.PlayResult(false);


                missedCP.gameObject.SetActive(true);

                int missed = GetMissedRings();
                missedCP.text = " You Missed " + missed + " Rings! ";

                UI_Canvas.instance.ShowLevelFail();
            }
        }
    }

    int GetMissedCount()
    {
        if (CheckPointManager.instance == null) return 0;

        return CheckPointManager.instance.GetTotal() -
               CheckPointManager.instance.GetCurrent();
    }

    int GetMissedRings()
    {
        if (RingPointManager.instance == null) return 0;

        return RingPointManager.instance.GetTotal() -
               RingPointManager.instance.GetCurrent();
    }
}