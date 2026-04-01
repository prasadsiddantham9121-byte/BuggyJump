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

    public List<LevelCheckpoints> levels = new List<LevelCheckpoints>();
    public List<LevelRings> levelRings = new List<LevelRings>();

    public TextMeshProUGUI Current_CP;
    public TextMeshProUGUI Total_CP;

    public int currentLevel = 0;

    private int currentCheckPoint = 0;
    private int ringsPassed = 0;

    public Transform arrow;
    public Transform player;

    public Transform[] landingSpots;

    [Header("UI Backgrounds")]
    public GameObject checkpointBG;
    public GameObject ringBG;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadLevel(currentLevel);
    }

    void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        PlayerPrefs.SetInt("CheckPoint", 0);
        currentCheckPoint = 0;
        ringsPassed = 0;

        var checkpoints = levels[currentLevel].checkpoints;
        var rings = levelRings[currentLevel].rings;

        bool hasCheckpoints = checkpoints.Length > 0;
        bool hasRings = rings.Length > 0;

        // ---------------- UI SWITCH ----------------
        checkpointBG.SetActive(hasCheckpoints);
        ringBG.SetActive(hasRings);

        // ---------------- TOTAL COUNT ----------------
        int total = hasCheckpoints ? checkpoints.Length : rings.Length;

        Total_CP.text = "/ " + total.ToString();
        Current_CP.text = "0";

        // ---------------- ACTIVATE RINGS ----------------
        if (hasRings)
        {
            foreach (var ring in rings)
            {
                ring.SetActive(true);
            }
        }
    }

    void Update()
    {
        var checkpoints = levels[currentLevel].checkpoints;
        bool hasCheckpoints = checkpoints.Length > 0;

        if (hasCheckpoints)
        {
            currentCheckPoint = PlayerPrefs.GetInt("CheckPoint");
            Current_CP.text = currentCheckPoint.ToString();
        }
        else
        {
            Current_CP.text = ringsPassed.ToString();
        }
    }

    public void LateUpdate()
    {
        var checkpoints = levels[currentLevel].checkpoints;
        var rings = levelRings[currentLevel].rings;

        bool hasCheckpoints = checkpoints.Length > 0;
        bool hasRings = rings.Length > 0;

        // ---------------- CHECKPOINT LEVEL ----------------
        if (hasCheckpoints)
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].SetActive(i == currentCheckPoint);
            }

            if (currentCheckPoint < checkpoints.Length)
            {
                UpdateArrowDirection(checkpoints[currentCheckPoint].transform);
            }
            else
            {
                UpdateArrowDirection(landingSpots[currentLevel]);
            }
        }

        // ---------------- RING LEVEL ----------------
        else if (hasRings)
        {
            if (ringsPassed < rings.Length)
            {
                UpdateArrowDirection(rings[ringsPassed].transform);
            }
            else
            {
                UpdateArrowDirection(landingSpots[currentLevel]);
            }
        }
    }

    void UpdateArrowDirection(Transform target)
    {
        Vector3 dir = target.position - player.position;

        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        arrow.rotation = Quaternion.Euler(0, 0, -angle);
    }

    // ---------------- CHECKPOINT ----------------
    public void CollectCheckpoint()
    {
        currentCheckPoint++;

        PlayerPrefs.SetInt("CheckPoint", currentCheckPoint);

        Current_CP.text = currentCheckPoint.ToString();
    }

    // ---------------- RING ----------------
    public void CollectRing()
    {
        ringsPassed++;

        Current_CP.text = ringsPassed.ToString();

       

        var rings = levelRings[currentLevel].rings;

        if (ringsPassed >= rings.Length)
        {
            Debug.Log("All Rings Completed → Go to Landing!");
        }
    }
}