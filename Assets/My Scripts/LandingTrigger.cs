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

        // ✅ Get cameras from ACTIVE PLAYER
        GameObject landingCamera = visual.landingCamera;
        GameObject mainCamera = visual.mainCamera;

        
        visual.JetCloseAnimation();
        visual.StopJetAnimation();

        
        controller.DisableControls();
        controller.ParticlesDisable();

        
        if (mainCamera != null) mainCamera.SetActive(false);
        if (landingCamera != null) landingCamera.SetActive(true);

        
        visual.TriggerLandingAnimation();

        
        TimeManager.instance.StopTimer();


        //========================================= Chechpoint Level Pass and Fail Conditions ===============================

        // Try checkpoint first
        if (CheckPointManagered.instance != null)
        {
            if (CheckPointManagered.instance.AreAllCheckpointsCollected())
            {
                print("Level Pass");
                UI_Canvas.instance.ShowLevelPass();
            }
            else
            {
                print("Level Fail");

                missedCP.gameObject.SetActive(true);

                int missed = GetMissedCount();
                missedCP.text = " You Missed " + missed + " CheckPoints! ";

                UI_Canvas.instance.ShowLevelFail();
            }
        }

        // 👉 If no checkpoint manager, use rings
        else if (RingPointManager.instance != null)
        {
            if (RingPointManager.instance.AreAllRingsCollected())
            {
                print("Level Pass");
                UI_Canvas.instance.ShowLevelPass();
            }
            else
            {
                print("Level Fail");

                missedCP.gameObject.SetActive(true);

                int missed = GetMissedRings();
                missedCP.text = " You Missed " + missed + " Rings! ";

                UI_Canvas.instance.ShowLevelFail();
            }
        }

    }

    int GetMissedCount()
    {
        return CheckPointManagered.instance == null ? 0 :
               (CheckPointManagered.instance.GetTotal() - CheckPointManagered.instance.GetCurrent());
    }

    int GetMissedRings()
    {
        return RingPointManager.instance == null ? 0 :
            (RingPointManager.instance.GetTotal() - RingPointManager.instance.GetCurrent());
    }

    //========================================= Ring Collection Level Pass and Fail Conditions ===============================

}