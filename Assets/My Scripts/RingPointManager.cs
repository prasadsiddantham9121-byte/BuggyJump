using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RingPointManager : MonoBehaviour
{
    public static RingPointManager instance;

    [System.Serializable]
    public class LevelRings
    {
        public GameObject[] rings;
    }

    [Header("All Levels")]
    public List<LevelRings> levels = new List<LevelRings>();

    private int currentLevelIndex;

    private GameObject[] rings;
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

    public GameObject checkPointManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        checkPointManager.SetActive(false);

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
        collectedRings = 0;

        if (levels.Count == 0 || currentLevelIndex >= levels.Count)
        {
            Debug.LogWarning("No ring level data!");
            ringBG.SetActive(false);
            return;
        }

        rings = levels[currentLevelIndex].rings;

        if (rings == null || rings.Length == 0)
        {
            Debug.Log("No rings in this level");
            ringBG.SetActive(false);
            return;
        }

        ringBG.SetActive(true);

        totalRings = rings.Length;
        UpdateUI();

        // ✅ Activate ALL rings at once
        foreach (var ring in rings)
        {
            if (ring != null)
                ring.SetActive(true);
        }

        // 👉 Set first nearest target
        UpdateNearestRing();
    }

    public void CollectRing(GameObject ringObj)
    {
        collectedRings++;
        UpdateUI();

        if (collectedRings < totalRings)
        {
            // 👉 Find next nearest ring
            UpdateNearestRing();
        }
        else
        {
            Debug.Log("All rings collected!");

            // 👉 Switch to landing point
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

    public bool AreAllRingsCollected()
    {
        return collectedRings >= totalRings;
    }

    public int GetCurrent()
    {
        return collectedRings;
    }

    public int GetTotal()
    {
        return totalRings;
    }
}
