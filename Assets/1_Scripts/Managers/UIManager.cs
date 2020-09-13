//#define USE_LOGS

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

    public Canvas mainCanvas;
    [Header("Fields")] public Canvas worldSpaceCanvas;
    [Header("Panels")] public UIPanel mainMenuPanel;
    public UIPanel_PreGame preGamePanel;
    public UIPanel_EndGame endGamePanel;
    public UIPanel inGamePanel;
    public UIPanel_Achievement newAchievementPanel;
    public UIPanel_NewMedal medalPanel;
    public UIPanel_Options optionsPanel;
    public UIPanel_Profile profilePanel;
    public UIPanel_Shop shopPanel;
    public UIPanel_NewHighscore highscorePanel;
    public UIPanel outofcoinsPanel;
    public UIPanel loadingPanel;
    [Space] public List<UIPanel> openPanels = new List<UIPanel>();
    [Space] public List<UIPanel> pendingPopups = new List<UIPanel>();

    [Header("Modules")] public UILeaderBoard mainPanelLeaderBoard;

    //  public UILeaderBoard endgameLeaderBoard;
    public Countdown countDown;
    public MidScreenMessage midScreenMessage;
    public MidScreenMessage timesUpMessage;

    public TipsScreen tipsScreen;
//    public NewTimer newTimer;

    [Header("Ingame")]
    public IngameShaman ingameShaman;
    public GameObject ingameVeil;
    [SerializeField] CanvasGroup vignette;
    [SerializeField] Text scoreText;

    public Text ScoreText
    {
        get { return scoreText; }
    }

    [SerializeField] Text timeText;
    [SerializeField] Text comboText;

    public int fontSize = 208;

    [Space] public RectTransform mainMenuBg;

    [Header("Parameters")] [SerializeField]
    float vignetteMaxAlpha = .8f;

    [SerializeField] Transform leaderboardIPhoneTransform;

    public bool isUIAvailable
    {
        get { return mainCanvas != null; }
    }


    void Start()
    {
        if (!isUIAvailable)
            return;
        // Fix the main menu background position for iPhone
        //      if(Camera.main.aspect > 1.4)
        //      {
        //          mainMenuBg.anchoredPosition = new Vector2 (0, mainMenuBg.anchoredPosition.y);
        //      }

        optionsPanel.Init();

        //      Vector3 lbCachedPos = leaderBoard.gameObject.transform.position;
        //      leaderBoard.gameObject.transform.position = leaderBoard.initialPos.position;

        // Initial Panel
        //      Utility.Instance.DoAfter (.1f, () => {


        //          LeanTween.move(leaderBoard.gameObject, lbCachedPos, GPM.Instance.leaderboardInDuration).setEase(GPM.Instance.leaderboardInEasing);
        //      } );

        // Set leaderboard size/pos for iPhone
        if (Camera.main.aspect > 1.4)
        {
            //            Camera.main.orthographicSize *= GPM.Instance.iPhoneCameraSizeModifier;
            mainPanelLeaderBoard.transform.position = leaderboardIPhoneTransform.position;
            mainPanelLeaderBoard.transform.localScale = leaderboardIPhoneTransform.localScale;
        }

        mainMenuPanel.Open();
        mainPanelLeaderBoard.AnimateIn();

        // Main Menu Music
        AudioManager2.Instance.Play(SoundsManager.Instance.mainMenuMusic, AudioClipExtended.AudioType.Music);


        // Feature: Localization
        // 2019.12.19 - LEVON
        //
        string currentLanguage = "English";

		// if ( Application.systemLanguage == SystemLanguage.Arabic ) {
		// 	currentLanguage = "Arabic"; }
		// else 
		if ( Application.systemLanguage == SystemLanguage.ChineseSimplified ) {
			currentLanguage = "ChineseSimplified"; }
		else if ( Application.systemLanguage == SystemLanguage.ChineseTraditional ) {
			currentLanguage = "ChineseTraditional"; }
		else if ( Application.systemLanguage == SystemLanguage.Dutch ) {
			currentLanguage = "Dutch"; }
		else if ( Application.systemLanguage == SystemLanguage.English ) {
			currentLanguage = "English"; }
		else if ( Application.systemLanguage == SystemLanguage.French ) {
			currentLanguage = "French"; }
		else if ( Application.systemLanguage == SystemLanguage.German ) {
			currentLanguage = "German"; }
		else if ( Application.systemLanguage == SystemLanguage.Italian ) {
			currentLanguage = "Italian"; }
		else if ( Application.systemLanguage == SystemLanguage.Japanese ) {
			currentLanguage = "Japanese"; }
		else if ( Application.systemLanguage == SystemLanguage.Korean ) {
			currentLanguage = "Korean"; }
		else if ( Application.systemLanguage == SystemLanguage.Portuguese ) {
			currentLanguage = "Portuguese"; }
		else if ( Application.systemLanguage == SystemLanguage.Russian ) {
			currentLanguage = "Russian"; }
		else if ( Application.systemLanguage == SystemLanguage.Spanish ) {
			currentLanguage = "Spanish"; }
		else if ( Application.systemLanguage == SystemLanguage.Turkish ) {
			currentLanguage = "Turkish"; }
		else {
			currentLanguage = "English"; }


		// 
		// Demo
		// 
		// currentLanguage = "German";
		//
		// End of Demo
		// 
		
		Lean.Localization.LeanLocalization.CurrentLanguage = currentLanguage;
		//
		// End of Feature

    }

    public void OnPlayClicked()
    {

    	// 
        // Feature: Hide InGameCanvas Veil
        // 2019.12.22 - LEVON
        // 
        // InGameCanvas, including top timer, is displayed for a brief moment before the main menu reveals
        // To make things worse, it can get stuck 
        // if a blocking-event interrupts the Unity engine 
        // such as Push Notification confirmation of iOS
        // So, we've put a veil (same object as the InGame Background Image) to hide everything behind
        // which must be hidden when the game actually starts
        //
        ingameVeil.SetActive( false );
        //
		// End of Feature
		// 


        inGamePanel.Open();
        GameManager.Instance.StartGameCountdown();
    }

    //  public void ShowTutorialPanel()
    //  {
    //      mainMenuPanel.Close ();
    //      tutorialPanel.Open ();
    //  }

    public void DisplayAdThenShowPregame()
    {
    	AppLovinManager.Instance.ShowInter ();

    	// Wait for open panels to closed
        Utility.Instance.WaitForCondition(() => !AppLovinManager.Instance.isInterVisible, ShowPregame);
    }

    public void ShowPregame()
    {
    	AppLovinManager.Instance.setRewardedVideoWatch ( false );

        preGamePanel.Open();
        //      endGamePanel.Close ();
    }

    //  public void ShowEndGame(int[] ballCounts, int score, int coinsEarned)
    //  {
    //
    ////        int score = ScoreManager.Instance.Score;
    //
    ////        inGamePanel.Close ();
    ////        mainMenuPanel.Open ();
    //  }

    public void DisplayAdThenShowMainMenu()
    {
    	AppLovinManager.Instance.ShowInter ();

    	// Wait for open panels to closed
        Utility.Instance.WaitForCondition(() =>
            {
                return !AppLovinManager.Instance.isInterVisible;
            
            }, () =>
            {

				ShowMainMenu();

            });

    }
    
    public void ShowMainMenu()
    {
        AudioManager2.Instance.Play(SoundsManager.Instance.mainMenuMusic, AudioClipExtended.AudioType.Music);
        mainMenuPanel.Open();
        //      preGamePanel.Close ();
        //      endGamePanel.Close ();
    }


    //  public void ShowEndGame(int[] ballCounts, int score, int coinsEarned)
    //  {
    //
    ////        int score = ScoreManager.Instance.Score;
    //
    ////        inGamePanel.Close ();
    ////        mainMenuPanel.Open ();
    //  }
    public void RefreshCurrencyTexts(int coins)
    {
        //      int coinsCount = GameSparksManager.Instance.player.coins;

        preGamePanel.SetCoins(coins);
        endGamePanel.SetCoins(coins);
    }

    public void OnPanelOpening(UIPanel panel)
    {
        //      if(panel.isLonely)
        //      {
        UIPanel[] dupOpenPanels = openPanels.ToArray();

        bool panelAlreadyOpen = false;
        foreach (var openPanel in dupOpenPanels)
        {
            // If new clip is deeper  or same close the panel
            if ((panel.Level <= openPanel.Level && openPanel != panel) // If the panel is the same one don't close
                && !(panel.isPopup || openPanel.isPopup) // if the panel or open panel is popup don't close
            )
            {
                openPanel.Close();
            }

            if (openPanel == panel)
            {
                panelAlreadyOpen = true;
            }
        }

        if (!panelAlreadyOpen)
            openPanels.Add(panel);
    }

    public void OnPanelClosing(UIPanel panel)
    {
        if (!isUIAvailable)
            return;

        openPanels.Remove(panel);
    }

    public void OnLoginFailed()
    {
        if (!isUIAvailable)
            return;

        mainPanelLeaderBoard.OnLoginFailed();
        endGamePanel.leaderboard.OnLoginFailed();
    }

    public void PlayButtonSFX()
    {
        //        if (playsOpenSound)
        //        {
        AudioManager2.Instance.Play(SoundsManager.Instance.genericClick, AudioClipExtended.AudioType.SFX);
        //        }
    }

    public void Vignette(float value)
    {
        Trace.Msg("New Vignette Value is " + value);
        float finalAlpha = vignetteMaxAlpha * value;
        LeanTween.alphaCanvas(vignette, finalAlpha, .2f);
    }

    public void SetIngameTimer(int remainingTime)
    {
        int totalSeconds = remainingTime;
        int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;

        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();

        // Put extra 0s if necessary
        if (seconds < 10)
        {
            secondsString = "0" + secondsString;
        }

        if (minutes < 10)
        {
            minutesString = "0" + minutesString;
        }


        timeText.text = minutesString + ":" + secondsString;
    }

    public void SetIngameScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetInGameComboText(int combo)
    {
        string txt = combo == 1 ? "" : ("x" + combo);

        int comboLvl = ScoreManager.Instance.GetComboLevel();
        float a = (float) comboLvl / 10 * .5f;
        a += .5f;
        a *= fontSize;

        comboText.text = txt;
        comboText.fontSize = Mathf.FloorToInt(a);
        //        print(a);
        //        print(comboText.fontSize);
        //        comboText.text = "x" + combo.ToString();
    }

    public void SetShowLoading(bool isShow)
    {
        if (isShow)
        {
            loadingPanel.Open();
        }
        else
        {
            loadingPanel.Close();
        }
    }

    public void TakeScreenShot(System.Action<Texture2D> callback)
    {
        StartCoroutine(
            TakeScreenShotRoutine(callback)
//			TakeScreenShotRoutine (Camera.main, Camera.main.pixelWidth, Camera.main.pixelHeight, callback)
        );
    }

    private IEnumerator TakeScreenShotRoutine(System.Action<Texture2D> callback)
    {
        yield return new WaitForEndOfFrame();
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        if (callback != null)
            callback(tex);

        Destroy(tex);

//		Sprite tempSprite = Sprite.Create(tex,new Rect(0,0,width,height),new Vector2(0,0));
//		GameObject.Find("SpriteObject").GetComponent<Image>().sprite = tempSprite;
    }

//	private IEnumerator TakeScreenShotRoutine(Camera _camera, int resWidth, int resHeight, System.Action<Texture2D> callback)
//	{
//		yield return new WaitForEndOfFrame();
//		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
//		_camera.targetTexture = rt;
//		var screenShotTexture= new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
//		_camera.Render();
//		RenderTexture.active = rt;
//		screenShotTexture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
//		screenShotTexture.Apply ();
//		_camera.targetTexture = null;
//		RenderTexture.active = null;
//		Destroy(rt);
//
//		if(callback != null) {
//			callback (screenShotTexture);
//		}
//		//		string filename = ScreenShotName(resWidth, resHeight);
//
//		//byte[] bytes = _screenShot.EncodeToPNG();
//		//System.IO.File.WriteAllBytes(filename, bytes);
//
//		//		Debug.Log(string.Format("Took screenshot to: {0}", filename));
//
//		Sprite tempSprite = Sprite.Create(screenShotTexture,new Rect(0,0,resWidth,resHeight),new Vector2(0,0));
//		GameObject.Find("SpriteObject").GetComponent<Image>().sprite = tempSprite;
//	}

    //  public void OnGameStart()
    //  {
    //      foreach (var item in openPanels) 
    //      {
    //          item.Close ();
    //      }
    //  }

    //  public void OnGameStop()
    //  {
    //      
    //  }
    //
    //  public void OnGamePause()
    //  {
    //      
    //  }
}