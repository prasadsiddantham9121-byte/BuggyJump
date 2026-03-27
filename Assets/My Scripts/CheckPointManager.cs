using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelCheckpoints
    {
        public GameObject[] checkpoints; 
    }

    [Header("Levels Data")]
    public List<LevelCheckpoints> levels = new List<LevelCheckpoints>();
    public List<GameObject> levelRoots = new List<GameObject>(); // 👈 Assign Level1, Level2, etc.


    public TextMeshProUGUI Current_CP;
    public TextMeshProUGUI Total_CP;

    public int currentLevel = 0; 
    private int currentCheckPoint = 0;

    public Transform arrow;          // Arrow UI / 3D arrow
    public Transform player;         // Player transform

    public Transform[] landingSpots; // ONE per level

    void Start()
    {
        // Safety check
        if (levels.Count == 0 || levelRoots.Count == 0)
        {
            Debug.LogError("Levels or LevelRoots not assigned!");
            return;
        }

        if (levels.Count != levelRoots.Count)
        {
            Debug.LogError("Levels and LevelRoots count mismatch!");
            return;
        }

        currentLevel = GetActiveLevelIndex();

        LoadLevel(currentLevel);
    }

    int GetActiveLevelIndex()
    {
        for (int i = 0; i < levelRoots.Count; i++)
        {
            if (levelRoots[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0; // fallback
    }

    void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        PlayerPrefs.SetInt("CheckPoint", 0);
        currentCheckPoint = 0;

        int total = levels[currentLevel].checkpoints.Length;
        Total_CP.text = "/ " + total.ToString();
        Current_CP.text = "0";
    }

    void Update()
    {
        currentCheckPoint = PlayerPrefs.GetInt("CheckPoint");

        Current_CP.text = currentCheckPoint.ToString();

       

        //if (currentCheckPoint >= currentLevelCheckpoints.Length)
        //{
        //    Debug.Log("Level Complete!");


        //    currentLevel++;

        //    if (currentLevel < levels.Count)
        //    {
        //        LoadLevel(currentLevel);
        //    }

        //    enabled = false;


        //}
    }

    public void LateUpdate()
    {
        var currentLevelCheckpoints = levels[currentLevel].checkpoints;

        for (int i = 0; i < currentLevelCheckpoints.Length; i++)
        {
            if (currentLevelCheckpoints[i] != null)
                currentLevelCheckpoints[i].SetActive(i == currentCheckPoint);
        }

        // Arrow logic
        if (player == null || arrow == null) return;


        // Arrow Direction Logic
        if (currentCheckPoint < currentLevelCheckpoints.Length)
        {
            // Point to current checkpoint
            UpdateArrowDirection(currentLevelCheckpoints[currentCheckPoint].transform);
        }
        else if (currentLevel < landingSpots.Length)
        {
            // All checkpoints done → point to landing spot
            UpdateArrowDirection(landingSpots[currentLevel]);
        }
    }

    void UpdateArrowDirection(Transform target)
    {
        Vector3 dir = target.position - player.position;

        // Convert 3D world direction to 2D plane (XZ → XY)
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        arrow.rotation = Quaternion.Euler(0, 0, -angle);
    }

    //void UpdateArrowDirection(Transform target)
    //{
    //    Vector3 dir = target.position - player.position;

    //    // Keep arrow horizontal (ignore height difference)
    //    dir.y = 0;

    //    if (dir != Vector3.zero)
    //    {
    //        arrow.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 90, 0);
    //    }
    //}

    public void CollectCheckpoint()
    {
        currentCheckPoint++;

        PlayerPrefs.SetInt("CheckPoint", currentCheckPoint);

        Current_CP.text = currentCheckPoint.ToString();
    }
}