using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CheckPointManagered : MonoBehaviour
{
    public static CheckPointManagered instance;

    [System.Serializable]
    public class LevelData
    {
        public GameObject[] checkpoints;
    }

    [Header("All Levels")]
    public List<LevelData> levels = new List<LevelData>();

    [Header("Current Level")]
    private int currentLevelIndex;

    private GameObject[] cps;
    private int totalCheckpoints;
    private int currentCheckpoints = 0;

    [Header("UI")]
    public GameObject cpBG;
    public TextMeshProUGUI cpText; // ✅ ONE TEXT ONLY

    [Header("Arrow Settings")]
    public Transform arrow;
    public Transform player;
    public Transform landingPoint;

    public GameObject ringPointmanager;

    private Transform currentTarget;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


    }

    private void Start()
    {
        ringPointmanager.SetActive(false);

        currentLevelIndex = PlayerPrefs.GetInt("levelToLoad", 0);

        FindPlayer();
        SetupLevel();
    }

    private void Update()
    {
        UpdateArrow();
    }

    void SetupLevel()
    {
        currentCheckpoints = 0;

        if (levels.Count == 0 || currentLevelIndex >= levels.Count)
        {
            Debug.LogWarning("No level data found!");
            cpBG.SetActive(false);
            return;
        }

        cps = levels[currentLevelIndex].checkpoints;

        if (cps == null || cps.Length == 0)
        {
            Debug.Log("No checkpoints in this level");
            cpBG.SetActive(false);
            return;
        }

        cpBG.SetActive(true);

        totalCheckpoints = cps.Length;
        UpdateUI();

        // Disable all checkpoints first
        foreach (var cp in cps)
        {
            if (cp != null)
                cp.SetActive(false);
        }

        // Activate first checkpoint
        cps[0].SetActive(true);

        // Set first arrow target
        currentTarget = cps[0].transform;
    }

    public void CollectCheckpoint(GameObject collectedCP)
    {
        collectedCP.SetActive(false);

        currentCheckpoints++;
        UpdateUI();

        if (currentCheckpoints < totalCheckpoints)
        {
            cps[currentCheckpoints].SetActive(true);

            // Next checkpoint target
            currentTarget = cps[currentCheckpoints].transform;
        }
        else
        {
            Debug.Log("All checkpoints collected!");

            // 👉 Switch arrow to landing point
            currentTarget = landingPoint;
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
}