using UnityEngine;
using System.Collections;

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

            //int index = 1;

            StartCoroutine(UI_Canvas.instance.DelayedTutorial(2.5f));

            other.GetComponent<PlayerVisualController>().SetJet();
            

            other.GetComponent<PlayerVisualController>().TriggerFlyingAnimation();

            other.GetComponent<PlayerController>().EnableControls();

            if(other.GetComponent<PlayerVisualController>().jet.hasOpenClose)
            {
                SoundManager.instance.StopSound("Helicopter");
                SoundManager.instance.PlaySound("Jet_Starting");
                other.GetComponent<PlayerVisualController>().JetOpenAnimation();
                SoundManager.instance.PlaySound("Flying");
            }

            other.GetComponent<PlayerVisualController>().JetOpenAnimation();
            SoundManager.instance.PlaySound("Flying");
            other.GetComponent<PlayerController>().ParticlesEnable();

            StartCoroutine(screenFader.FadeIn(1f));

        
            UI_Canvas.instance.inGameUI.SetActive(true);
            

            StartCoroutine(DelayToDisableObject());
        }
    }

    IEnumerator DelayToDisableObject()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        SoundManager.instance.StopSound("Helicopter");
        UI_Canvas.instance.SetCutsceneState(false);
        SoundManager.instance.PlaySound("Flying");

    }
}