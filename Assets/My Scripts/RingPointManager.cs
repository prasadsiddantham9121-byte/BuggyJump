using UnityEngine;
using TMPro;

public class RingPointManager : MonoBehaviour
{
    public static RingPointManager instance;

    [Header("Rings in this Level")]
    public GameObject[] rings;

    private int totalRings;
    private int collectedRings = 0;

    [Header("UI")]
    public GameObject ringBG;
    public TextMeshProUGUI ringText;

    [Header("Arrow Settings")]
    public Transform arrow;
    public Transform player;
    public Transform landingPoint;

    private Transform currentTarget;

    //public GameObject checkPointManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void Start()
    {
        //checkPointManager.SetActive(false);

        FindPlayer();
        SetupLevel();
    }

    private void Update()
    {
        UpdateArrow();
    }

    void SetupLevel()
    {
        collectedRings = 0;

        if (rings == null || rings.Length == 0)
        {
            Debug.LogWarning("No rings assigned!");
            ringBG.SetActive(false);
            return;
        }

        ringBG.SetActive(true);

        totalRings = rings.Length;
        UpdateUI();

        // Activate all rings
        foreach (var ring in rings)
        {
            if (ring != null)
                ring.SetActive(true);
        }

        // Find first nearest ring
        UpdateNearestRing();
    }

    public void CollectRing(GameObject ringObj)
    {
        if (!ringObj.activeSelf) return;

        collectedRings++;
        UpdateUI();

        if (collectedRings < totalRings)
        {
            UpdateNearestRing();
        }
        else
        {
            Debug.Log("All rings collected!");

            RingActivateLandingEffect();

            currentTarget = landingPoint;
        }
    }

    void UpdateNearestRing()
    {
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (var ring in rings)
        {
            if (ring != null && ring.activeSelf)
            {
                float dist = Vector3.Distance(player.position, ring.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = ring.transform;
                }
            }
        }

        currentTarget = nearest;
    }

    void UpdateUI()
    {
        if (ringText != null)
            ringText.text = collectedRings + " / " + totalRings;
    }

    void UpdateArrow()
    {
        if (arrow == null || player == null || currentTarget == null)
            return;

        // Convert target position into player's local space
        Vector3 direction = player.InverseTransformPoint(currentTarget.position);

        // Calculate angle
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Optional offset (depends on your arrow model orientation)
        angle += 180;

        // Apply rotation (LOCAL)
        arrow.localEulerAngles = new Vector3(0, 180, angle);
    }

    void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    public bool AreAllRingsCollected()
    {
        return collectedRings == totalRings;
    }

    public int GetCurrent()
    {
        return collectedRings;
    }

    public int GetTotal()
    {
        return totalRings;
    }

    void RingActivateLandingEffect()
    {
        if (landingPoint == null) return;

        
        if (landingPoint.childCount > 0)
        {
            landingPoint.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void RingOffLandingEffect()
    {
        if (landingPoint == null)
        {
            Debug.LogError("LandingPoint NOT assigned!");
            return;
        }

        if (landingPoint.childCount == 0)
        {
            Debug.LogError("LandingPoint has NO child!");
            return;
        }

        landingPoint.GetChild(0).gameObject.SetActive(false);

        Debug.Log("Landing Effect OFF"); 
    }
}