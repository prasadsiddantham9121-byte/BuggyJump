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

        //LoadLevel(currentLevel);

        currentLevel = PlayerPrefs.GetInt(StringsData.levelToLoad, 0);
        LoadLevel(currentLevel);
    }

    void LoadLevel(int levelIndex)
    {

        if (levelIndex >= levelsCheckPoints.Count || levelIndex >= levelRings.Count)
        {
            Debug.LogError("Invalid Level Index: " + levelIndex);
            return;
        }

        currentLevel = levelIndex;

        //Debug.Log("Loading Level: " + currentLevel);

        Debug.Log("Loading Level: " + currentLevel);

        // 🔥 Disable everything first
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
        currentCheckPoint = 0;
        ringsPassed = 0;

        var checkpoints = levelsCheckPoints[currentLevel].checkpoints;
        var rings = levelRings[currentLevel].rings;

        // 🔥 Detect level type
        if (checkpoints.Length > 0)
            levelType = LevelType.Checkpoints;
        else
            levelType = LevelType.Rings;

        // UI switch
        checkpointBG.SetActive(levelType == LevelType.Checkpoints);
        ringBG.SetActive(levelType == LevelType.Rings);

        Current_CP.gameObject.SetActive(levelType == LevelType.Checkpoints);
        Total_CP.gameObject.SetActive(levelType == LevelType.Checkpoints);

        Current_Rings.gameObject.SetActive(levelType == LevelType.Rings);
        Total_Rings.gameObject.SetActive(levelType == LevelType.Rings);

        int total = (levelType == LevelType.Checkpoints) ? checkpoints.Length : rings.Length;

        if (levelType == LevelType.Checkpoints)
        {
            Total_CP.text = "/ " + total;
            Current_CP.text = "0";

            ActivateCheckpoint(0);
        }
        else
        {
            Total_Rings.text = "/ " + total;
            Current_Rings.text = "0";

            foreach (var ring in rings)
            {
                ring.SetActive(true);

                Collider col = ring.GetComponent<Collider>();
                if (col != null) col.enabled = true;
            }
        }

        // 🔥 Disable landing indicators
        foreach (var spot in landingSpots)
        {
            if (spot != null && spot.childCount > 0)
                spot.GetChild(0).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (levelType == LevelType.Checkpoints)
        {
            Current_CP.text = currentCheckPoint.ToString();

            var checkpoints = levelsCheckPoints[currentLevel].checkpoints;

            if (currentCheckPoint < checkpoints.Length)
                UpdateArrowDirection(checkpoints[currentCheckPoint].transform);
            else
                UpdateArrowDirection(landingSpots[currentLevel]);
        }
        else
        {
            Current_Rings.text = ringsPassed.ToString();

            var rings = levelRings[currentLevel].rings;

            if (ringsPassed < rings.Length)
                UpdateArrowDirection(rings[ringsPassed].transform);
            else
                UpdateArrowDirection(landingSpots[currentLevel]);
        }
    }

    // 🔥 ONLY activate needed checkpoint
    void ActivateCheckpoint(int index)
    {
        var checkpoints = levelsCheckPoints[currentLevel].checkpoints;

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetActive(i == index);
        }
    }

    void UpdateArrowDirection(Transform target)
    {
        if (player == null || target == null) return;

        Vector3 dir = target.position - player.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        arrow.rotation = Quaternion.Euler(0, 0, -angle);
    }

    void ActivateLandingIndicator()
    {
        if (landingSpots.Length > currentLevel)
        {
            Transform spot = landingSpots[currentLevel];

            if (spot != null && spot.childCount > 0)
                spot.GetChild(0).gameObject.SetActive(true);
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

        if (currentCheckPoint < checkpoints.Length)
        {
            ActivateCheckpoint(currentCheckPoint);
        }
        else
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

        if (ringsPassed >= rings.Length)
        {
            ActivateLandingIndicator();
        }
    }

    public void SetLevel(int levelIndex)
    {
        LoadLevel(levelIndex);
    }

    public int GetCurrentCheckpoint()
    {
        return currentCheckPoint;
    }
}