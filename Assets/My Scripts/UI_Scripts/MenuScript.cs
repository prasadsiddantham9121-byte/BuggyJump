//using Cinemachine;
using Script;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MenuScript : MonoBehaviour
{
    public static MenuScript instance;
    public GameObject Mainmenu, Sub, levelselection, tab_Loading, remote, pc, buy3Charaters, buyAllCharacters, unlockfullgame, CharacterSelection;
    //public GameObject Water;
    public int selectedlvl;
    public AudioSource menuSound;

    private void Awake()
    {
        if (instance == null)

            instance = this;

       
    }
    
    private void OnEnable()
    {
        if (levelselection.activeInHierarchy)
        {
            UpdateLevelButtonsUI();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        AudioListener.pause = false;
       
       // isRotating = new bool[JetSkis.Length];

        PlayerPrefs.SetInt("Jet0", 1);
        if (!PlayerPrefs.HasKey("PopUpCounter"))
        {
            PlayerPrefs.SetInt("PopUpCounter", 0);
        }

       
        UpdateLevelButtonsUI(); 

    }
   // private bool[] isRotating;
   
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPrevious();
        }


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNext();
        }

    }

    public void MenuSoundOn()
    {
        menuSound.Play();
    }
    public void MenuSoundOff()
    {
        menuSound.Stop();
    }

    public void Menubackbutton()
    {
        Mainmenu.SetActive(false);
        Sub.SetActive(true);
        MenuSoundOff();
    }
    public void LevelSelectionbackButton()
    {
        levelselection.SetActive(false);
        CharacterSelection.SetActive(true);
        UpdateTotalCoins();
        DeactivateAllAnims();
       // Water.SetActive(true);
        CheckJetBuy(currentjet);
        DeactiveAllJetskis();
        JetSkis[0].SetActive(true);
        Amims[0].SetActive(true);
        buyAllCharacters.SetActive(false);
        buy3Charaters.SetActive(false);
    }
    public void BoatSelectionBackbutton()
    {
        CharacterSelection.SetActive(false);
        UpdateTotalCoins();
       // Water.SetActive(false);
        CheckJetBuy(currentjet = 0);
        DeactiveAllJetskis();
        Mainmenu.SetActive(true);
    }

    void DeactivateAllAnims()
    {
        for (int i = 0; i < Amims.Length; i++)
        {
            Amims[i].SetActive(false);
        }
    }

    public void store1605coins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins");
        PlayerPrefs.Save();
        int coin950 = 1605;
        totalCoins += coin950;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        UpdateTotalCoins();

    }
    public void store2605coins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins");
        PlayerPrefs.Save();
        int coin1250 = 2605;
        totalCoins += coin1250;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        UpdateTotalCoins();
    }
    public void UnlockTruckFromStore()
    {
        PlayerPrefs.SetInt("Jet1", 1);
        PlayerPrefs.SetInt("Jet2", 1);
    }
    public void UnlockTruckCoinsFromStore()
    {
        PlayerPrefs.SetInt("Jet1", 1);
        int totalCoins = PlayerPrefs.GetInt("TotalCoins");
        PlayerPrefs.Save();
        int coin2000 = 2000;
        totalCoins += coin2000;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        UpdateTotalCoins();
    }
    
    
    public void Subscriptionclose()
    {
        if (UI_Canvas.isfromgame)
        {
            Sub.SetActive(false);
            MenuSoundOn();
            //Water.gameObject.SetActive(true);
            JetSkis[0].SetActive(true);
            CharacterSelection.SetActive(true);
            UpdateTotalCoins();
        }
        else
        {
            Mainmenu.SetActive(true);
            UpdateTotalCoins();
            Sub.SetActive(false);
            MenuSoundOn();
        }
    }
    public void OnStartBtnPressed()
    {
        Mainmenu.SetActive(false);
        Sub.SetActive(false);
        CharacterSelection.SetActive(true);
        UpdateTotalCoins();
        JetSkis[0].SetActive(true);
       // Water.gameObject.SetActive(true);
    }

    public void OnUnlockAllCharactersCloseBtnPressed()
    {
        levelselection.SetActive(true);
        buyAllCharacters.SetActive(false);
        buy3Charaters.SetActive(false);
        // trucks.gameObject.SetActive(false);
    }

    public void OnCharacterSelection()
    {
        if (PlayerPrefs.GetInt("PopUpCounter") == 0)
        {
            buy3Charaters.SetActive(true);
            CharacterSelection.SetActive(false);
           // Water.gameObject.SetActive(false);
            PlayerPrefs.SetInt("PopUpCounter", 1);
        }
        else
        {
            buyAllCharacters.SetActive(true);
            CharacterSelection.SetActive(false);
           // Water.gameObject.SetActive(false);
            PlayerPrefs.SetInt("PopUpCounter", 0);
        }

        for (int i = 0; i < JetSkis.Length; i++)
        {
            JetSkis[i].SetActive(false);
        }

    }
    public void Subscription_Any()
    {
        for (int i = 1; i < 10; i++)
        {
            PlayerPrefs.SetInt("Jet" + i, 1);
        }
        PlayerPrefs.SetInt("BuyFullGame", 1);
    }

    public IAPButton buyButton;
    public int currentjet;
    public void CharacterBuy()
    {
        // Debug.Log("Current Truck Index: " + currenttruck);
        PlayerPrefs.SetInt("Jet" + currentjet.ToString(), 1);
        CheckJetBuy(currentjet);

        Invoke("purchecs", 1f);
    }
    public void purchecs()
    {
        select.SetActive(true);
        buyButton_.SetActive(false);
        // free.SetActive(false);
        unlock2000.gameObject.SetActive(false);
    }
    public void UnlockAllCharBuy()
    {

        for (int i = 1; i < 10; i++)
        {
            PlayerPrefs.SetInt("Jet" + i, 1);
        }
    }
    public void Unlock3CharBuy()
    {

        for (int i = 1; i < 3; i++)
        {
            PlayerPrefs.SetInt("Jet" + i, 1);
        }
    }

    [SerializeField] GameObject select, buyButton_;
    [SerializeField] TextMeshProUGUI Jetname;
    public GameObject[] Amims;
    public GameObject[] JetSkis;
    [SerializeField] string[] JetskiNames;
    public void CheckJetBuy(int JetSki)
    {
        unlock2000.gameObject.SetActive(false);
        buyButton_.SetActive(false);
        UpdateAnims();
        Debug.Log("JetSki");
        for (int i = 0; i < JetSkis.Length; i++)
        {
            if (i == currentjet)
            {
                JetSkis[i].SetActive(true);
                unlock2000.gameObject.SetActive(false);
                Jetname.text = JetskiNames[i];
               

                if (PlayerPrefs.GetInt("Jet" + JetSki) == 1)
                {
                    select.SetActive(true);
                    buyButton_.SetActive(false);
                    unlock2000.gameObject.SetActive(false);

                }
                else
                {
                    select.SetActive(false);
                    buyButton_.SetActive(true);
                    if (JetSki == 1)
                    {

                        unlock2000.SetActive(true);
                    }
                    else
                    {

                        unlock2000.SetActive(false);
                    }

                }
            }
            else
            {
                JetSkis[i].SetActive(false);
                
            }
        }

    }

    void UpdateAnims()
    {
        for (int i = 0; i < Amims.Length; i++)
        {
            Amims[i].SetActive(i == currentjet);
        }
    }

    public GameObject store;
    public void OnstorePanel()
    {
        Mainmenu.SetActive(false);
        store.SetActive(true);
    }
    public void OffstorePanel()
    {
        Mainmenu.SetActive(true);
        UpdateTotalCoins();
        store.SetActive(false);
    }

    public GameObject unlock2000;
   

    private void DeactiveAllJetskis()
    {
        for (int i = 0; i < JetSkis.Length; i++)
        {
            JetSkis[i].SetActive(false);
           
        }
    }

    public void leftClick()
    {
            if (currentjet > 0)
            {
                currentjet--;
            }

            buyButton.productId = "skydiver" + (currentjet + 1);
            CheckJetBuy(currentjet);
           
     
    }

    public void Moreinfo()
    {
        Application.OpenURL("https://www.amazon.com/Games-PlayD-Game-Studio-Private-Limited/s?rh=n%3A9209902011%2Cp_4%3APlayD+Game+Studio+Private+Limited");
    }
    public void rightClick()
    {
        if (currentjet < JetSkis.Length - 1)
        {
            currentjet++;
        }

        buyButton.productId = "skydiver" + (currentjet + 1);

        CheckJetBuy(currentjet);

    }

    public void Loading()
    {
        levelselection.SetActive(false);
        if (AndroidTV.IsAndroidOrFireTv())
        {
            
            remote.SetActive(true);
        }
        else
        {
            string devicename = SystemInfo.deviceModel.ToString();
            devicename.Trim();
            devicename.ToLower();
            if (devicename.Contains("Subsystem"))
            {
               
                pc.SetActive(true);
            }
            else
            {
              
                tab_Loading.SetActive(true);
            }
        }
    }
    public Button[] lvlButtons;
    int levelsUnlocked;

    // ------------------- NEW: LevelButtonUI Class ----------------------
    [System.Serializable]
    public class LevelButtonUI
    {
        public Button button;       
        public GameObject lockImg;  
        public GameObject interactable; 
        public GameObject completed;  
    }

    public LevelButtonUI[] lvlButtonsUI;
    
    void UpdateLevelButtonsUI()
    {
        levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked", 1);

        for (int i = 0; i < lvlButtonsUI.Length; i++)
        {
            LevelButtonUI lvl = lvlButtonsUI[i];

            // Default state: Locked
            lvl.button.interactable = false;
            lvl.lockImg.SetActive(true);
            lvl.interactable.SetActive(false);
            lvl.completed.SetActive(false);

            // Unlocked levels
            if (i < levelsUnlocked)
            {
                lvl.button.interactable = true;
                lvl.lockImg.SetActive(false);
                lvl.interactable.SetActive(true);

                // Completed level
                if (PlayerPrefs.GetInt("LevelCompleted_" + i, 0) == 1)
                {
                    lvl.interactable.SetActive(false);
                    lvl.completed.SetActive(true);
                }
            }
        }
    }

    
    public void LoadLevel(int levelIndex)
    {
        Debug.LogError("LevelsUnlocked" + PlayerPrefs.GetInt("levelsUnlocked"));

        // Just start coroutine always
        StartCoroutine(LoadLevelCoroutine(levelIndex));
    }


    private IEnumerator LoadLevelCoroutine(int levelIndex)
    {
        if (levelIndex >= 4 && PlayerPrefs.GetInt("BuyFullGame", 0) == 0)
        {

            levelselection.SetActive(false);
            unlockfullgame.SetActive(true);
            yield break;
        }
        Loading();
        yield return new WaitForSeconds(3f);



        //PlayerPrefs.SetInt("level", levelIndex + 1);
        PlayerPrefs.SetInt("SelectedLevel", levelIndex);
        SceneManager.LoadScene("prasad");           
    }

    public RectTransform content;
    public int selectedIndex = 0;
    public float smoothSpeed = 2.5f;
    public GameObject SelectedObj;
    public float[] positions;
    public void Content_move()
    {
        if (positions.Length > 0 && selectedIndex >= 0 && selectedIndex < positions.Length)
        {
            if (SelectedObj != null && SelectedObj.GetComponent<levelid>() != null)
            {
               
                Vector2 targetPosition = new Vector2(positions[SelectedObj.GetComponent<levelid>().id], content.anchoredPosition.y);
                
                content.anchoredPosition = Vector2.Lerp(content.anchoredPosition, targetPosition, smoothSpeed * Time.deltaTime);
            }


        }
    }
    public void SelectNext()
    {
        if (selectedIndex < positions.Length - 1)
        {
            selectedIndex++;
        }
    }

    public void SelectPrevious()
    {
        if (selectedIndex > 0)
        {
            selectedIndex--;
        }
    }


    public void BUYFULLGameButton()
    {
        PlayerPrefs.SetInt("BuyFullGame", 1);
        Invoke("BUYFULLGameCloseButton", 1f);
    }
    public void BUYFULLGameCloseButton()
    {
        unlockfullgame.SetActive(false);
        levelselection.SetActive(true);

    }

    public List<TextMeshProUGUI> totalCoinsTexts;

    public void UpdateTotalCoins()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins");
        string displayText = totalCoins.ToString();
        foreach (TextMeshProUGUI text in totalCoinsTexts)
        {
            text.text = displayText;

        }
    }
    private const int unlockThreshold = 2000;
    public GameObject notEnoughCoinstext;
    public void Unlockjet2000()
    {
        int totalCoins = PlayerPrefs.GetInt("TotalCoins");
        if (totalCoins >= unlockThreshold)
        {
            totalCoins -= unlockThreshold;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            
            PlayerPrefs.SetInt("Jet1", 1);
            UpdateTotalCoins();
            CheckJetBuy(currentjet);
            Debug.Log("Char unlocked!");
            unlock2000.SetActive(false);
            buyButton_.SetActive(false);
            select.SetActive(true);

            StartCoroutine(SetSelectButtonFocus());
        }
        else
        {
            notEnoughCoinstext.SetActive(true);
            StartCoroutine(Popop());
        }
    }
    public IEnumerator Popop()
    {
        yield return new WaitForSeconds(3f);
        notEnoughCoinstext.SetActive(false);
    }

    IEnumerator SetSelectButtonFocus()
    {
        yield return null; // wait 1 frame (VERY IMPORTANT)

        EventSystem.current.SetSelectedGameObject(null); // reset first
        EventSystem.current.SetSelectedGameObject(select);
    }

    

    //-------------------------------------------------------- Settings Panel ------------------------------------------------------------

    public GameObject settingsPanel;
    public Button settingsCloseButton;


    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
        Mainmenu.SetActive(false);
    }

    public void SettingsCloseButton()
    {
        settingsPanel.SetActive(false);
        Mainmenu.SetActive(true);
        UpdateTotalCoins();
    }

  

    // --------------------- settngs Sound ------------------------------------------------
    // --------------------- Settings Sound (SINGLE BUTTON) ---------------------
    [Header("Sound Toggle")]
    public Image soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public AudioSource subAudioSource;


   
    public void Mute()
    {
       
        if (AudioListener.volume == 0)
        {
            soundButton.sprite = soundOnSprite;
            AudioListener.volume = 1;
           
        }
        else
        {
            soundButton.sprite = soundOffSprite;
           
            AudioListener.volume = 0;
        }
    }

    public void CheatCode()
    {
        UnlockAllCharBuy();
    }
}
