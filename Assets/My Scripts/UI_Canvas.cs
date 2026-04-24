using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Script;
using TMPro;
using System.Collections;

public class UI_Canvas : MonoBehaviour
{
    public static UI_Canvas instance;

    public GameObject levelPassPanel;
    public GameObject levelFailPanel;
    public GameObject pausePanel;
    //public GameObject startPanel;
    public GameObject inGameUI;
    private ScreenFader screenFader;
    private bool isCutscenePlaying = false;
    public static bool isfromgame;

    public Joystick joystick;

    public bool isTutorial = false;
    public bool isGameOver = false;
    public bool isGameStarted = false;

    
    //=== Timer ==================
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("In-Game Sound")]
    public AudioSource audioSource;
    public AudioClip audioClip;
    //===================================== Warning Text =============================================

    [Header("Warning UI")]
    public GameObject warningPanel;
    public Image warningImage;
    public TextMeshProUGUI warningText;

    public float blinkSpeed = 2f;

    public GameObject[] levels;
    private int currentLevelIndex = 0;

    //================================================= Tutorial References ==================
    public GameObject tvTutorial;
    public GameObject tabTutorial;
    public GameObject pauseButton;


    private Coroutine blinkCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        screenFader = FindObjectOfType<ScreenFader>();

        isfromgame = true;

        if (!AndroidTV.IsAndroidOrFireTv())
        {
            pauseButton.SetActive(false);
        }
    }

    void Start()
    {
        //Time.timeScale = 0f;
        //startPanel.SetActive(true);
        inGameUI.SetActive(false);
        StartCoroutine(screenFader.FadeIn());
        SetCutsceneState(true);
        SoundManager.instance.PlaySound("Helicopter");


        InGameAudioStart();

        // ✅ Read selected level first
        currentLevelIndex = PlayerPrefs.GetInt("SelectedLevel", 0);

        // Safety clamp
        currentLevelIndex = Mathf.Clamp(currentLevelIndex, 0, levels.Length - 1);

        // ✅ Activate correct level
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] != null)
                levels[i].SetActive(i == currentLevelIndex);
        }
    }

    void Update()
    {
        PauseButtonHandler();
    }

    public void UpdateTimer(string timeTextValue)
    {
        timerText.text = timeTextValue;
    }

    //==================================================== UI Panels =================================================================

    public void ShowLevelPass()
    {
        StartCoroutine(ShowLevelPassWithDelay());
    }
    
    public void ShowLevelFail()
    {
        StartCoroutine(ShowlevelFailWithDelay());

    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public IEnumerator ShowLevelPassWithDelay()
    {
        yield return new WaitForSeconds(8f);
        levelPassPanel.SetActive(true);
    }

    public IEnumerator ShowlevelFailWithDelay()
    {
        yield return new WaitForSeconds(4f);
        levelFailPanel.SetActive(true);
    }

    //==================================================== UI Buttons =================================================================

  

    public void LevelPassHomeButton()
    {
        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 0);

        int unlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

        // Mark level completed
        PlayerPrefs.SetInt("LevelCompleted_" + currentLevel, 1);

        if (currentLevel + 1 >= unlocked)
        {
            PlayerPrefs.SetInt("levelsUnlocked", currentLevel + 2);
        }

        PlayerPrefs.SetInt("SelectedLevel", currentLevel + 1);
        // Move highlighter to next level
        PlayerPrefs.SetInt("currentHighlighter", currentLevel + 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void LevelFailHomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseButtonHandler()
    {
        if (isCutscenePlaying) return;
        if (isGameOver) return;
        if (levelPassPanel.activeInHierarchy) return;
        if (levelFailPanel.activeInHierarchy) return;
        //if (startPanel.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    //===================================== Warning Text =============================================
    public void ShowWarning(string message)
    {
        warningPanel.SetActive(true);
        warningText.text = message;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkWarning());
    }

    public void HideWarning()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        warningPanel.SetActive(false);
    }

    IEnumerator BlinkWarning()
    {
        while (true)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);

            // Text
            Color tColor = warningText.color;
            tColor.a = alpha;
            warningText.color = tColor;

            // Image
            Color iColor = warningImage.color;
            iColor.a = alpha;
            warningImage.color = iColor;

            yield return null;
        }
    }

    //============================================= Cut Scene Hadler ======================================================
    public void SetCutsceneState(bool state)
    {
        isCutscenePlaying = state;
    }

    //============================================= In Game Audio Handling ======================================================

    public void InGameAudioStop()
    {
        audioSource.clip = audioClip;
        audioSource.Stop();
    }

    public void InGameAudioStart()
    {

        audioSource.clip = audioClip;
        audioSource.Play();

    }

    //===================================================== Tutorial Part ===================================================
    public IEnumerator ShowFirstTimeTutorial()
    {
       
        if (!AndroidTV.IsAndroidOrFireTv())
        {
            pauseButton.SetActive(true);
        }

        int level = PlayerPrefs.GetInt("SelectedLevel", 0);

        if (level == 0 && PlayerPrefs.GetInt("Level1TutorialShown", 0) == 0)
        {
            isTutorial = true;

            if (AndroidTV.IsAndroidOrFireTv())
                tvTutorial.SetActive(true);
            else
                tabTutorial.SetActive(true);

            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(3f);

            tvTutorial.SetActive(false);
            tabTutorial.SetActive(false);

            isTutorial = false;

            PlayerPrefs.SetInt("Level1TutorialShown", 1);
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f;
        isGameStarted = true;

    }

    public IEnumerator DelayedTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(ShowFirstTimeTutorial());
    }
}