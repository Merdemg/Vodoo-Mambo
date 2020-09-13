//#define USE_LOGS
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;


public class GameManager : MonoBehaviour {

	#region Singleton
	public static GameManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

    public delegate void GameEventDelegate();
    public delegate void GameEventDelegateBool(bool foo);
	public GameEventDelegate OnGameStart;
    public GameEventDelegateBool OnGamePause;
	public GameEventDelegate OnGameStop;

	[Header("InGame Parameters")]
    [SerializeField]float remainingTime = 0;
    [SerializeField]float RemainingTime{
		get {
			return remainingTime;
		}
		set {
			remainingTime = value;
            UIManager.Instance.SetIngameTimer((int)remainingTime);
//			UIManager.Instance.timeText.text = "00:" + ((int)remainingTime).ToString ();
		}
	}
    [SerializeField]bool timerActive = true;
    [SerializeField]public bool TimerActive{
        get{
            return newTimer.IsActive;
        }
    }

    [SerializeField]GameplayState gameplayState = GameplayState.Stopped;
	public GameplayState GameplayState {
		get{
			return gameplayState;
		}
	}

    [SerializeField]bool lastSecondsTriggered = false;

	[Header ("InGame Containers")]
	public List<Ball> balls;
	public List<GameObject> spellObjects;

	public Bounds GameAreaBounds{
		
		get{
			return gameAreaBounds;
		}
	}
	[Header("Game Area")]
	[SerializeField]private Bounds gameAreaBounds;

	[Header("Ball Count")]
//	[SerializeField]int[] ballCounts = new int[]{0,0,0,0};
    [SerializeField]int[] _currentGameBallCounts = new int[]{0,0,0,0};
    [SerializeField]float _currentGameBallNormalVelocity = 0;
    public float CurrentGameBallNormalVelocity{
        get{
            return _currentGameBallNormalVelocity;
        }
    }

    [Header("Timer")]
    public NewTimer newTimer;



	void Start()
	{
        _currentGameBallCounts = new int[]{0,0,0,0};

        // Set Target Frame Rate
        Application.targetFrameRate = 60;

		// Set camera size for iPhone
		if(Camera.main.aspect > 1.4)
		{
			Camera.main.orthographicSize *= GPM.Instance.iPhoneCameraSizeModifier;

		}
		
		// Initialize game area bounds
		RefreshGameArea ();

		LeanTween.init (800);

//		float height = Camera.main.orthographicSize * 2;
//		float width = Camera.main.aspect * height;
//		gameAreaBounds = new Bounds (Vector3.zero, new Vector3 (width, height));

//		if(!Tutorial.Instance.IsTutorialShown(TutorialType.Pinch))
//		{
//			Tutorial.Instance.InitTutorial(TutorialType.Pinch);
//		}
		GameAnalytics.Initialize();
	}

//	bool ShouldShowStartTutorial()
//	{
//		int gamerunned = 0;
//
//		gamerunned = PlayerPrefs.GetInt ("gamerunned");
//
//		if (gamerunned == 0) 
//		{
//			PlayerPrefs.SetInt ("gamerunned", 1);
//			return true;
//		}
//		else 
//		{
//			return false;
//		}
//	}

	private Bounds RefreshGameArea()
	{
		var mainCamera = Camera.main;
		var screenHeight = mainCamera.orthographicSize * 2;
		var width = mainCamera.aspect * screenHeight;

		// var height = (4f / 3) * width;
		// var height = (575f / 352) * width;
		var height = screenHeight * (3 / 4f); 
		if (height > screenHeight) height = screenHeight;
		
		gameAreaBounds = new Bounds (Vector3.zero, new Vector3 (width, height));
		
		return gameAreaBounds;
	}

    private bool hasShownTipForCurrentGame = false;
	public void StartGameCountdown()
	{
        if(!hasShownTipForCurrentGame) {
            hasShownTipForCurrentGame = true;
			UIManager.Instance.tipsScreen.Show(StartGameCountdown);
			return;
        }

		gameplayState = GameplayState.Playing;
		newTimer.Reset ();

        if(!PitchCinematic.IsCinematicShown) {
            PitchCinematic.Instance.ShowCinematic(StartGameCountdown);
            return;
        }

        // If we have not showed tutorial yet bypass countdown
        if(!Tutorial.Instance.IsTutorialShown(TutorialType.Pinch)) {
            Tutorial.Instance.InitTutorial(TutorialType.Pinch, null, StartGame);

            return;
        }

		// Start countdown to game
		UIManager.Instance.countDown.StartCountown (StartGame);

	}

	void StartGame()
	{
        UIManager.Instance.SetInGameComboText(1);
		newTimer.StartTimer();
        LeanTween.value(gameObject, GPM.Instance.ballNormalVelocity, GPM.Instance.ballNormalEndGameVelocity, GPM.Instance.gameplayTime).setOnUpdate((value)=>{
            _currentGameBallNormalVelocity =  value;
        });

		GoogleAnalyticsManager.Instance.Event_StartedGame ();
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "game");

		if(OnGameStart != null)
		{
			OnGameStart ();
		}

		// Start Timer & Loop
		StartCoroutine (BallSpawnLoop ());

		// Audio
        AudioManager2.Instance.Play (SoundsManager.Instance.ingameMusic,AudioClipExtended.AudioType.Music, SoundsManager.Instance.inGameMusicVolume,false);

	}

    // TODO: ISDEBUG
    public void DEBUG_SimulateStopGame()
    {
        ScoreManager.Instance.AddScore(ScoreType.Pinch);
        _currentGameBallCounts = new int[]{ 20, 15, 10, 5 };
        StopGame();
    }

    public void InitStopGame() {
        StartCoroutine(GameStopBallAndSpellKill());
    }

	private void StopGame()
	{
		Trace.Msg (("Game Manager Stop Game").Colored (Colors.lime));
        hasShownTipForCurrentGame = false;
		lastSecondsTriggered = false;
		gameplayState = GameplayState.Stopped;

        int initialCoins = (int)MetaManager.Instance.Get(MetaManager.MetaType.coins);
        int currentHighscore = (int)MetaManager.Instance.Get(MetaManager.MetaType.hiscore);
        int endGameScore = ScoreManager.Instance.Score;
        Trace.Msg("End game score:" + endGameScore);

		// Calculate earned gold with ball counts
//        var initialBallCounts = (int[])MetaManager.Instance.Get(MetaManager.MetaType.ballCounts);
//        int[] newBallCounts = (int[])initialBallCounts.Clone();
		int[] newBallCounts = new int[]{0,0,0,0};

		int earnedGold = 0;
        for (int level = 0; level < _currentGameBallCounts.Length; level++) {
            newBallCounts [level] += _currentGameBallCounts [level];

			// Give gold
            int gameBallCount = _currentGameBallCounts[level];
//            int loggedBallCount = ((int[])MetaManager.Instance.Get(MetaManager.MetaType.ballCounts))[i];

//            int currentFraction = loggedBallCount % GPM.Instance.ballRecordMaxValues [i];

//            Trace.Msg ("currentFraction: " + currentFraction);
            earnedGold += Mathf.FloorToInt( ( gameBallCount) / (float)GPM.Instance.ballRecordMaxValues[level] )
                * GPM.Instance.goldPerBallCountByLevel[level];
//			print("level :" + level );
//			print("gameBallCount :" + gameBallCount );
//			print("(float)GPM.Instance.ballRecordMaxValues[i] :" + (float)GPM.Instance.ballRecordMaxValues[level] );
//			print("GPM.Instance.goldPerBallCountByLevel[i] :" + GPM.Instance.goldPerBallCountByLevel[level] );
//
//			print("Mathf.FloorToInt( ( gameBallCount) / (float)GPM.Instance.ballRecordMaxValues[i] ) * GPM.Instance.goldPerBallCountByLevel[i] :" + 
//			(Mathf.FloorToInt ((gameBallCount) / (float)GPM.Instance.ballRecordMaxValues [level]) * GPM.Instance.goldPerBallCountByLevel [level]));

//			print("earnedGold: " + earnedGold);
		}
        // Execute registered stop events
		if(OnGameStop != null) {
			OnGameStop ();
		}
        // Analytics
		GoogleAnalyticsManager.Instance.Event_Scored (ScoreManager.Instance.Score);
		GoogleAnalyticsManager.Instance.Event_GoldEarned (earnedGold);
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "score", ScoreManager.Instance.Score);

        // Update Meta
        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, earnedGold, new object[]{ true, false });
        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.ballCounts, newBallCounts);
        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.weeklyScore, ScoreManager.Instance.Score);

		// New Achievements
        Achievement[] newAchievements = AchievementManager.Instance.CheckEndGameAchievements ();

        // Feature: Display ads after viewing game score
        // 2019.12.18 - LEVON
        //
        // Move existing code to line: ???
        // 

		// Show ads
		// AppLovinManager.Instance.ShowInter ();

		//
		// End of Feature

        // TODO WIP -- show panels one after another
        //Utility.Instance.WaitForCondition(() =>
        //{
        //    return !AppLovinManager.Instance.isInterVisible;
        //}, () =>
        //{
        //    UIManager.Instance.newAchievementPanel.OpenIfPending();

        //    // HighScore
        //    if (endGameScore > currentHighscore) {
        //        Trace.Msg(("New highscore, Opening Highscore Panel!").Colored(Colors.green));
        //        UIManager.Instance.highscorePanel.OpenWithScore(endGameScore);
        //    }

        //});

        // Wait for open panels to closed
        Utility.Instance.WaitForCondition(() =>
            {
                return 
                	!UIManager.Instance.newAchievementPanel.IsOpen 
                	&& !UIManager.Instance.newAchievementPanel.IsPending 
                	&& !UIManager.Instance.highscorePanel.IsOpen;
                	// && !AppLovinManager.Instance.isInterVisible; // Commented-out part of feature 2019.12.28 by LEVON
            
            }, () =>
            {

				// Audio
				// Show End Game panel
                UIManager.Instance.endGamePanel.Initialize(newBallCounts,
                    endGameScore,
                    earnedGold,
                    initialCoins, 
					new int[]{0,0,0,0}, 
                    newAchievements);
                
                UIManager.Instance.endGamePanel.Open();

                Trace.Msg("currentHighscore: " + currentHighscore);


            });

		// Clean Up & reset game cache
		SpawnManager.Instance.holdables = new List<IHoldable> ();
		balls = new List<Ball> ();
		spellObjects = new List<GameObject> ();
		_currentGameBallCounts = new int[]{0,0,0,0};
		ScoreManager.Instance.ResetScore ();
        ScoreManager.Instance.ResetComboMultiplier();
		Time.timeScale = 1f;
	}

    public void PauseGame(bool isPaused, bool effectTimeScale = false, bool stopFloating = true)
	{
        
		Trace.Msg (("Game Manager Pause Game").Colored (Colors.lime));
        InputManager.Instance.DestroyHolders();
		if(isPaused)
		{
			gameplayState = GameplayState.Paused;
			if(effectTimeScale) {
				Time.timeScale = 0;
			}
			// Stop floating objects
            if(stopFloating) {
                foreach(Floating floating in SpawnManager.Instance.floatings) {
                    if(floating)
                        floating.StopMoving();
                }
            }
		}
		else
		{
			gameplayState = GameplayState.Playing;
			if(effectTimeScale) {
				Time.timeScale = 1;
			}

            foreach(Floating floating in SpawnManager.Instance.floatings) {
                if(floating)
                    floating.StartMoving();
            }
		}
	}

    public void SuspendGame(bool isSuspended)
    {
        Trace.Msg (("Game Manager Pause Game").Colored (Colors.lime));
        if(isSuspended)
        {
            gameplayState = GameplayState.Suspended;
        }
        else
        {
            gameplayState = GameplayState.Playing;
        }
        
    }

	public void PauseGameToggle()
	{
		if(gameplayState == GameplayState.Paused)
		{
            PauseGame(false, true, false);
		}
		else
		{
            PauseGame(true, true, false);
		}

		if(OnGamePause != null)
            OnGamePause (gameplayState == GameplayState.Paused);
	}

	IEnumerator BallSpawnLoop()
	{
		Trace.Msg (("BallSpawnLoop Started").Colored (Colors.lime));
		float timePassedSinceLastSpawn = GPM.Instance.ballSpawnInterval;

		while(true)
		{
			// Stop if game is stopped
			if (gameplayState == GameplayState.Stopped)
			{
				yield break;
			}
            else if(gameplayState != GameplayState.Playing)
			{
				yield return false;
				continue;
			}
            else if(!newTimer.IsActive)
            {
                yield return false;
                continue;
            }

            // Reduce spawn interval according to ball count min = x4 
            float ballFillness = (float)balls.Count / (float)GPM.Instance.ballCountLimit;

            float newBallSpawnInterval = 
                ballFillness * GPM.Instance.ballSpawnIntervalMultiplierMax
                * GPM.Instance.ballSpawnInterval;

            // Can't me less than 1/4 of normal interval
            if(newBallSpawnInterval < GPM.Instance.ballSpawnInterval / 2) { newBallSpawnInterval = GPM.Instance.ballSpawnInterval / 2; }

			// Spawn if less than max && interval passed
//			if(balls.Count < GPM.Instance.ballCountLimit && 
//				timePassedSinceLastSpawn > GPM.Instance.ballSpawnInterval)
			if(balls.Count < GPM.Instance.ballCountLimit && 
                timePassedSinceLastSpawn > newBallSpawnInterval)
            {
                Trace.Msg("newBallSpawnInterval: " + newBallSpawnInterval);

				SpawnManager.Instance.SpawnBall ();

				timePassedSinceLastSpawn = 0;
			}

			// Increase interval time if not paused
            if(gameplayState == GameplayState.Playing)
				timePassedSinceLastSpawn += Time.deltaTime;

			yield return false;
		}
  	}

	IEnumerator GameStopBallAndSpellKill()
	{
		Trace.Msg (("GameStopBallKill Started").Colored (Colors.lime));
        PauseGame (true, false, false);

		// Kill all Spells
		foreach (var spellObject in spellObjects)
		{
			Destroy (spellObject.gameObject);
		}

		float ballKillInterval = .2f;
		float ballKillIntervalDropdownFactor = .9f;

		float currentElapsedTime = 0;

		bool continueKilling = true;

		int ballIndex = 0;

		while(continueKilling)
		{
			currentElapsedTime += Time.deltaTime;

			// Check time for the next ball kill
			if(currentElapsedTime >= ballKillInterval)
			{
				//Kill a ball
				Ball ball = balls [ballIndex];

                // Check if ball still exists
                if(ball != null)
                {
                    ScoreManager.Instance.AddScore (ScoreType.GameEndExplode, ball.transform.position, ball.level, false, false);

                    LeanTween.scale(ball.gameObject, Vector3.zero, .2f).setOnComplete(()=>{
						Destroy(ball.gameObject);
                    });
                    
                    currentElapsedTime -= ballKillInterval;
                    ballKillInterval *= ballKillIntervalDropdownFactor;
                }

                ballIndex++;

            }

			if (ballIndex > balls.Count - 1)
				continueKilling = false;

			yield return 0;
		}

		StopGame ();
	}

//	public void IncreaseRemainingTime(float amount)
//	{
//		RemainingTime += amount;
//	}

	public void IncrementBallCount(int level)
	{
		
		if (_currentGameBallCounts == null)
		{
			Debug.LogWarning ("Ball Counts is not initialized!");
			return;
		}

		_currentGameBallCounts [level] += 2;
	}

	public bool IsSpellObjectExists<T>() {
		foreach (var spellObject in spellObjects) {
			if (spellObject == null)
				continue;
			
			if(spellObject.GetComponent<T>() != null) {
				return true;
			}
		}
		return false;
	}

	public void PauseTimer(bool isPause) {
		newTimer.SetPaused (isPause);
	}

    public Ball GetRandomBall()
    {
        while(true) {
            var randomBall = balls[Random.Range(0, balls.Count)];
            if(randomBall.IsActive) {
                return randomBall;
            }
        }
    }
}
