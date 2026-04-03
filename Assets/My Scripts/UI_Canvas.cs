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
    public GameObject startPanel;
    public GameObject inGameUI;
    private ScreenFader screenFader;
    private bool isCutscenePlaying = false;

    
    //=== dummy ==================
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("In-Game Sound")]
    public AudioSource ingameAudio;

    //===================================== Warning Text =============================================

    [Header("Warning UI")]
    public GameObject warningPanel;
    public Image warningImage;
    public TextMeshProUGUI warningText;

    public float blinkSpeed = 2f;

    private Coroutine blinkCoroutine;

    private void Awake()
    {
        instance = this;

        screenFader = FindObjectOfType<ScreenFader>();
    }

    void Start()
    {
        Time.timeScale = 0f;
        startPanel.SetActive(true);
        inGameUI.SetActive(false);
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

    public void StartButton()
    {
        startPanel.SetActive(false);
        StartCoroutine(screenFader.FadeIn());
        Time.timeScale = 1f;
        SetCutsceneState(true);
    }

    public void HomeButton()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void PauseButtonHandler()
    {
        if (isCutscenePlaying) return;

        if (levelPassPanel.activeInHierarchy) return;
        if (levelFailPanel.activeInHierarchy) return;
        if (startPanel.activeInHierarchy) return;

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

  

}