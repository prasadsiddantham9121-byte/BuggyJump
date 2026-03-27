using UnityEngine;
using TMPro;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("Level Timer Settings")]
    [Tooltip("Set time (in seconds) for each level")]
    public float[] levelTimes;   // Assign 10 values in Inspector

    public int currentLevel = 0;

    private float countDownTimer;
    private bool canCountdown;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartLevelTimer(currentLevel);
    }

    private void Update()
    {
        if (canCountdown)
        {
            countDownTimer -= Time.deltaTime;

            countDownTimer = Mathf.Max(0, countDownTimer);

            UI_Canvas.instance.UpdateTimer(FormatTime(countDownTimer));
            if (countDownTimer <= 0)
            {
                countDownTimer = 0;
                canCountdown = false;

                StopTimer();
                UI_Canvas.instance.ShowLevelFail();

                print("Time Up!");

            }
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelTimer(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelTimes.Length)
        {
            Debug.LogError("Invalid Level Index!");
            return;
        }

        currentLevel = levelIndex;
        countDownTimer = levelTimes[levelIndex];
        canCountdown = true;
    }

   
    public void StopTimer()
    {
        canCountdown = false;
    }

   
    public void ResumeTimer()
    {
        canCountdown = true;
    }

    public float GetCountDown()
    {
        return countDownTimer;
    }

    public void AddTime(float time)
    {
        countDownTimer += time;
    }



}
