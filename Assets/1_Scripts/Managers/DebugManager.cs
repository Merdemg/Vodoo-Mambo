#define USE_LOGS
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugManager : MonoBehaviour
{
	#region Singleton
	public static DebugManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

	#endregion

	public bool autoStart = false;
	public bool dontStopGame = false;

	public Spell comet;

	public Canvas debugCanvas;

    public Text currenComboText;
	public Text comboLeftText;

	[Header("Test Elements")]
	public UIBallFiller testFiller;
    public int[] testInitialBallCounts;
	public int[] testFinalBallCounts;

	public Achievement testAchievement;
    public Achievement[] endgameTestAchievements;

	[Header("Test Spells")]
    public Spell bombSpell;
    public Spell blackholeSpell;
    public Spell levelUpSpell;

	public void Start()
	{
		if(autoStart)
		{
			Invoke ("AutoStart", .5f);
		}
	}

	void Update()
	{
		UpdateComboLeft();
	}

	void UpdateComboLeft()
	{
        comboLeftText.text = ScoreManager.Instance.activeComboTimeRemaining.ToString("0.00");
        currenComboText.text = ScoreManager.Instance.ComboMultiplier.ToString();
	}

	private void AutoStart()
	{
		UIManager.Instance.OnPlayClicked ();
	}

	public void Reset()
	{
		Application.LoadLevel (0);
	}

    public void CastSpell()
    {
        SpellManager.Instance.GetRandomSpell ().Cast (SpawnManager.Instance.GetRandomPositionInGameArea(4));
    }
	
	public void CastBombSpell()
	{
		bombSpell.Cast(SpawnManager.Instance.GetRandomPositionInGameArea(4));
	}

	public void CastBlackholeSpell()
	{
		blackholeSpell.Cast(SpawnManager.Instance.GetRandomPositionInGameArea(4));
	}

	public void CastLevelUpSpell()
	{
		levelUpSpell.Cast(SpawnManager.Instance.GetRandomPositionInGameArea(4));
	}

	public void CastComet()
	{
		comet.Cast (SpawnManager.Instance.GetRandomPositionInGameArea (.4f));
	}
		
	public void ResetTutorial()
	{
		PlayerPrefs.SetInt (TutorialType.Pinch + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellBomb + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellLevelup + "isshown", 0);
		PlayerPrefs.SetInt (TutorialType.SpellBlackhole + "isshown", 0);
	}

	public void DeleteLocalPrefs()
	{
		PlayerPrefs.DeleteAll ();
	}

	public void GetBallCounts()
	{
		Trace.Msg("BallCounts Debug started!");
		GameSparksManager.Instance.GetBallCounts ((ballCounts) => {
			Trace.Msg("BallCounts Debug finished!");
		});
	}

	public void GetMedalCounts()
	{
		Trace.Msg("BallCounts Debug started!");
		GameSparksManager.Instance.GetMedalsByID (GameSparksManager.Instance.player.userId, (medals) => {
			Trace.Msg("BallCounts Debug finished!");
		});
	}

	public void MoveCoin()
	{
		UIManager.Instance.endGamePanel.MoveCoins (testFiller.transform.position, 40);
	}

	public void BallFill()
	{
//		UIManager.Instance.endGamePanel.BallFill2 (testBallCounts);
	}

	public void Earn()
	{
		testAchievement.Achieve();
	}

	public void NewMedal()
	{
		int randomScore = ( 100 * UnityEngine.Random.Range( 1, 10 ) );
		UIManager.Instance.medalPanel.LoadWithMedalAlert (new MedalAlert ("gold", randomScore ) );
	}

	public void OpenEndGame()
	{
		UIManager.Instance.endGamePanel.Open ();
	}

    public void TestEndGame()
    {
        UIManager.Instance.endGamePanel.Initialize(testFinalBallCounts, 100, 10, 10, testInitialBallCounts,endgameTestAchievements );
        UIManager.Instance.endGamePanel.Open();
    }
    
    public void TestHighscore()
    {
        UIManager.Instance.highscorePanel.OpenWithScore(1000);
    }

    public void PauseGame()
    {
        GameManager.Instance.PauseGameToggle();
    }

    public void StopGame() {
        GameManager.Instance.InitStopGame();
    }

    public void ShowInter() {
        AppLovinManager.Instance.TestInter();
    }

	public void PinchTwoBalls(int startIndex = 0) {
		if(startIndex > GameManager.Instance.balls.Count - 1) {
			Debug.LogWarning ("No balls found to pinch!");
			return;
		}
		Ball randomBallOne = GameManager.Instance.balls[startIndex];
        Ball randomBallTwo = null;
        // Get another ball with same level
        foreach (var ball in GameManager.Instance.balls) {
            if(ball.level == randomBallOne.level && ball != randomBallOne) {
                randomBallTwo = ball;
                break;
            }
        }
		if(randomBallOne != null && randomBallTwo != null) {
			GameControlManager.Instance.Pinch(new Ball[]{randomBallOne, randomBallTwo});
		}
		else {
			PinchTwoBalls (startIndex + 1);
		}
    }

	public void Suck() {
		// Search for a blackhole object
		foreach (var spellObject in GameManager.Instance.spellObjects) {
			BlackholeObject blackholeObject = spellObject.GetComponent<BlackholeObject> ();
			if( blackholeObject != null) {
				blackholeObject.DebugSuck (GameManager.Instance.balls [0]);
				break;
			}
		}
	}

	public void ToggleLoading(){
		UIManager.Instance.SetShowLoading (!UIManager.Instance.loadingPanel.gameObject.activeSelf);
	}
//	public void TestLeaderboardLocalPlayerMarking()  {
//		UIManager.Instance.mainPanelLeaderBoard.InitLocalPlayer ();
//	}

	public void Share() {
		UIManager.Instance.endGamePanel.Share (100);
	}

    public void AddCoins() {
        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, 100, new object[]{true, false});
    }
}

