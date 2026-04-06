using UnityEngine;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    [Header("Checkpoints (Single Level Only)")]
    public GameObject[] checkpoints;

    private int totalCheckpoints;
    private int currentCheckpoints = 0;

    [Header("UI")]
    public GameObject cpBG;
    public TextMeshProUGUI cpText;

    [Header("Arrow Settings")]
    public Transform arrow;
    public Transform player;
    public Transform landingPoint;

    [Header("Fade")]
    public ScreenFader screenFader;

    private Transform currentTarget;

    public Transform startPoint;
    public Transform endPoint;
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
       

        FindPlayer();
        SetupCheckpoints();
    }

    private void Update()
    {
        UpdateArrow();
    }

    void SetupCheckpoints()
    {
        currentCheckpoints = 0;

        if (checkpoints == null || checkpoints.Length == 0)
        {
            Debug.LogWarning("No checkpoints assigned!");
            cpBG.SetActive(false);
            return;
        }

        cpBG.SetActive(true);

        totalCheckpoints = checkpoints.Length;
        UpdateUI();

        // Disable all first
        foreach (var cp in checkpoints)
        {
            if (cp != null)
                cp.SetActive(false);
        }

        // Activate first checkpoint
        checkpoints[0].SetActive(true);

        // Set arrow target
        currentTarget = checkpoints[0].transform;
    }

    public void CollectCheckpoint(GameObject collectedCP)
    {
        collectedCP.SetActive(false);

        currentCheckpoints++;
        UpdateUI();

        if (currentCheckpoints < totalCheckpoints)
        {
            checkpoints[currentCheckpoints].SetActive(true);
            currentTarget = checkpoints[currentCheckpoints].transform;
        }
        else
        {
            Debug.Log("All checkpoints collected!");

            ActivateLandingEffect();

            // 👉 Arrow goes to landing point
            currentTarget = landingPoint;
            PlayerController.instance.DoLandingToResultsPos();
        }
    }

    void UpdateUI()
    {
        if (cpText != null)
            cpText.text = currentCheckpoints + " / " + totalCheckpoints;
    }

    void UpdateArrow()
    {
        if (arrow == null || player == null || currentTarget == null)
            return;

        Vector3 direction = currentTarget.position - player.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            arrow.rotation = Quaternion.Slerp(arrow.rotation, targetRotation, Time.deltaTime * 5f);
        }
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

    public bool AreAllCheckpointsCollected()
    {
        return currentCheckpoints >= totalCheckpoints;
    }

    public int GetCurrent()
    {
        return currentCheckpoints;
    }

    public int GetTotal()
    {
        return totalCheckpoints;
    }


    void ActivateLandingEffect()
    {
        if (landingPoint == null) return;

        // Get first (and only) child
        if (landingPoint.childCount > 0)
        {
            landingPoint.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void DiActivateLandingEffect()
    {
        if (landingPoint.childCount > 0)
        {
            landingPoint.GetChild(0).gameObject.SetActive(false);
        }
    }
}