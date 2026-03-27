using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingTrigger : MonoBehaviour
{
    public GameObject landingCamera;
    public GameObject mainCamera;

    private CheckPointManager cpManager;

    private void Start()
    {
        cpManager = FindObjectOfType<CheckPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
            print("player Landed Here");
        other.GetComponent<PlayerVisualController>().JetCloseAnimation();

        int totalCP = cpManager.levels[cpManager.currentLevel].checkpoints.Length;
        int currentCP = PlayerPrefs.GetInt("CheckPoint");

        if (currentCP < totalCP)
        {
            Debug.Log("Level Failed! Missed checkpoints.");

            
            other.GetComponent<PlayerController>().DisableControls();
            other.GetComponent<PlayerController>().ParticlesDisable();

            
            mainCamera.SetActive(false);
            landingCamera.SetActive(true);

            
            other.GetComponent<PlayerVisualController>().TriggerLandingAnimation();

            
            TimeManager.instance.StopTimer();

            
            UI_Canvas.instance.ShowLevelFail();

            return;
        }

        

        Debug.Log("Player Landed Successfully!");


        other.GetComponent<PlayerController>().DisableControls();
        
        other.GetComponent<PlayerController>().ParticlesDisable();

        mainCamera.SetActive(false);
        landingCamera.SetActive(true);

        other.GetComponent<PlayerVisualController>().TriggerLandingAnimation();

        TimeManager.instance.StopTimer();

        UI_Canvas.instance.ShowLevelPass();
        
    }
}
