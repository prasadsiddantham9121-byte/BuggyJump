using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;

    [System.Serializable]
    public class LevelCheckpoints
    {
        public GameObject[] checkpoints;
    }

    [System.Serializable]
    public class LevelRings
    {
        public GameObject[] rings;
    }

    public List<LevelCheckpoints> levelsCheckPoints = new List<LevelCheckpoints>();
    public List<LevelRings> levelRings = new List<LevelRings>();

    [Header("CheckPoint UI")]
    public TextMeshProUGUI Current_CP;
    public TextMeshProUGUI Total_CP;

    [Header("Ring UI")]
    public TextMeshProUGUI Current_Rings;
    public TextMeshProUGUI Total_Rings;

    public int currentLevel = 0;

    private int currentCheckPoint = 0;
    private int ringsPassed = 0;

    public Transform arrow;
    public Transform player;

    public Transform[] landingSpots;

    [Header("UI Backgrounds")]
    public GameObject checkpointBG;
    public GameObject ringBG;

    public enum LevelType
    {
        Checkpoints,
        Rings
    }

    public LevelType levelType;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found!");

        LoadLevel(currentLevel);
    }

    void LoadLevel(int levelIndex)
    {

        // 🔥 STEP 1: DISABLE EVERYTHING FROM ALL LEVELS
        foreach (var lvl in levelsCheckPoints)
        {
            foreach (var cp in lvl.checkpoints)
            {
                if (cp != null)
                    cp.SetActive(false);
            }
        }

        foreach (var lvl in levelRings)
        {
            foreach (var ring in lvl.rings)
            {
                if (ring != null)
                    ring.SetActive(false);
            }
        }

        currentLevel = levelIndex;

        PlayerPrefs.SetInt("CheckPoint", 0);
        currentCheckPoint = 0;
        ringsPassed = 0;

        var checkpoints = levelsCheckPoints[currentLevel].checkpoints;
        var rings = levelRings[currentLevel].rings;

        // 🔥 Detect level type
        if (checkpoints.Length > 0)
            levelType = LevelType.Checkpoints;
        else if (rings.Length > 0)
            levelType = LevelType.Rings;

        // UI switch
        checkpointBG.SetActive(levelType == LevelType.Checkpoints);
        ringBG.SetActive(levelType == LevelType.Rings);

        // ✅ ADD THIS RIGHT HERE
        Current_CP.gameObject.SetActive(levelType == LevelType.Checkpoints);
        Total_CP.gameObject.SetActive(levelType == LevelType.Checkpoints);

        Current_Rings.gameObject.SetActive(levelType == LevelType.Rings);
        Total_Rings.gameObject.SetActive(levelType == LevelType.Rings);

        int total = (levelType == LevelType.Checkpoints) ? checkpoints.Length : rings.Length;

        if (levelType == LevelType.Checkpoints)
        {
            Total_CP.text = "/ " + total.ToString();
            Current_CP.text = "0";
        }
        else
        {
            Total_Rings.text = "/ " + total.ToString();
            Current_Rings.text = "0";
        }

        // 🔥 Disable ALL landing indicators
        foreach (var spot in landingSpots)
        {
            if (spot != null && spot.childCount > 0)
            {
                spot.GetChild(0).gameObject.SetActive(false);
            }
        }

        // ---------------- CHECKPOINT SETUP ----------------
        if (levelType == LevelType.Checkpoints)
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].SetActive(i == 0);
            }
        }

        // ---------------- RING SETUP ----------------
        else if (levelType == LevelType.Rings)
        {
            foreach (var ring in rings)
            {
                ring.SetActive(true);

                Collider col = ring.GetComponent<Collider>();
                if (col != null) col.enabled = true;
            }
        }
    }

    void Update()
    {
        if (levelType == LevelType.Checkpoints)
        {
            currentCheckPoint = PlayerPrefs.GetInt("CheckPoint");
            Current_CP.text = currentCheckPoint.ToString();
        }
        else
        {
            Current_Rings.text = ringsPassed.ToString();
        }
    }

    void LateUpdate()
    {
        var checkpoints = levelsCheckPoints[currentLevel].checkpoints;
        var rings = levelRings[currentLevel].rings;

        // ---------------- CHECKPOINT MODE ----------------
        if (levelType == LevelType.Checkpoints)
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].SetActive(i == currentCheckPoint);
            }

            if (currentCheckPoint < checkpoints.Length)
                UpdateArrowDirection(checkpoints[currentCheckPoint].transform);
            else
                UpdateArrowDirection(landingSpots[currentLevel]);
        }

        // ---------------- RING MODE ----------------
        else
        {
            if (ringsPassed < rings.Length)
                UpdateArrowDirection(rings[ringsPassed].transform);
            else
                UpdateArrowDirection(landingSpots[currentLevel]);
        }
    }

    void UpdateArrowDirection(Transform target)
    {
        if (player == null || target == null) return;

        Vector3 dir = target.position - player.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        arrow.rotation = Quaternion.Euler(0, 0, -angle);
    }

    // 🔥 LANDING INDICATOR ACTIVATE
    void ActivateLandingIndicator()
    {
        if (landingSpots.Length > currentLevel)
        {
            Transform spot = landingSpots[currentLevel];

            if (spot != null && spot.childCount > 0)
            {
                spot.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Landing indicator missing!");
            }
        }

        Debug.Log("Go to Landing Area!");
    }

    // ---------------- CHECKPOINT ----------------
    public void CollectCheckpoint()
    {
        if (levelType != LevelType.Checkpoints) return;

        currentCheckPoint++;
        PlayerPrefs.SetInt("CheckPoint", currentCheckPoint);

        var checkpoints = levelsCheckPoints[currentLevel].checkpoints;

        // 🔥 ALL CHECKPOINTS DONE
        if (currentCheckPoint >= checkpoints.Length)
        {
            ActivateLandingIndicator();
        }
    }

    // ---------------- RING ----------------
    public void CollectRing(GameObject ring)
    {
        if (levelType != LevelType.Rings) return;

        ringsPassed++;

        Collider col = ring.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        var rings = levelRings[currentLevel].rings;

        // 🔥 ALL RINGS DONE
        if (ringsPassed >= rings.Length)
        {
            ActivateLandingIndicator();
        }
    }
}