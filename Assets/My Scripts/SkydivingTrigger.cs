using UnityEngine;

public class SkydivingTrigger : MonoBehaviour
{

    private ScreenFader screenFader;

    private void Start()
    {
        screenFader = FindObjectOfType<ScreenFader>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            print("player triggered");

            other.GetComponent<PlayerVisualController>().TriggerFlyingAnimation();

            other.GetComponent<PlayerController>().EnableControls();

            other.GetComponent<PlayerVisualController>().JetOpenAnimation();
            other.GetComponent<PlayerController>().ParticlesEnable();

            StartCoroutine(screenFader.FadeIn(2.3f));

        
            UI_Canvas.instance.inGameUI.SetActive(true);
            UI_Canvas.instance.SetCutsceneState(false);
        }
    }
}