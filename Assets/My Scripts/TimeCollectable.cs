using TMPro;
using UnityEngine;

public class TimeCollectable : MonoBehaviour
{
   // public float timeToAdd = 2f;
    public TextMeshProUGUI floatingText;
    //public AudioClip pickupClip;   // Assign in Inspector

    private TimeManager timeManager;
    //private AudioSource pickupSource;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();

        
        //GameObject audioObj = GameObject.FindGameObjectWithTag("AddTime");
        //if (audioObj != null)
        //{
        //    pickupSource = audioObj.GetComponent<AudioSource>();
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        float randomTime = Random.Range(5, 16);

        // Add time
        if (timeManager != null)
        {
            print("Time Added");
            timeManager.AddTime(randomTime);
        }

        //// Play pickup sound
        //if (pickupSource != null && pickupClip != null)
        //{
        //    pickupSource.PlayOneShot(pickupClip);
        //}

        // Show floating text
        if (floatingText != null)
        {
            //floatingText.text = "+" + randomTime.ToString("0");
            floatingText.transform.position = transform.position + Vector3.up * 1f;
            floatingText.gameObject.SetActive(true);
            FloatingText ft = floatingText.GetComponent<FloatingText>();
            if (ft != null)
            {
                ft.Setup(randomTime);
            }

            //floatingText.GetComponent<FloatingText>().Setup(randomTime);
        }

        Destroy(gameObject);
    }
}