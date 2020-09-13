using UnityEngine;

public class GoogleAnalyticsManager : MonoBehaviour
{
	public static GoogleAnalyticsManager Instance { private set; get; }

//	private GoogleAnalyticsV4 _googleAnalytics;

	private float _totalAppDuration = 0L;

	void Awake()
	{
		Instance = this;

//		_googleAnalytics = GetComponent<GoogleAnalyticsV4>();
//
//		_googleAnalytics.StartSession();
	}

	void Start()
	{
//		_googleAnalytics.LogScreen("Initial Menu");
	}

	void Update()
	{
		_totalAppDuration += Time.deltaTime;
	}

	void OnApplicationQuit()
	{
//		LogTiming("App Time", (long)_totalAppDuration, "Session Duration", "User's session duration.");
		Timing_SessionTime ((long)_totalAppDuration);
	}

	void LogEvent(string category, string action, string label, long value)
	{
//		_googleAnalytics.LogEvent(category, action, label, value);
	}

	void LogTiming(string category, long interval, string name, string label)
	{
//		_googleAnalytics.LogTiming(category, interval, name, label);
	}

	public void LogScreen(string title)
	{
//		_googleAnalytics.LogScreen (title);
	}

	// EVENTS

	// TIMINGS
	public void Timing_SessionTime(long seconds)
	{
		LogTiming ("Timing", seconds, "Session time", "User's session duration.");
	}

	public void Screen_MainMenuTime()
	{
//		LogTiming ("Timing", (long)seconds, "Mainmenu time", "User's session main menu  duration.");
		LogScreen("Main Menu");
	}


	// SCREENS
//	public void Screen_PreGameTime()
//	{
                         //		LogScreen("Pregame");

	// BUTTON CLICKS
	public void Event_Button_LeaderBoardGlobal()
	{
		LogEvent ("Button Click", "Leaderboard Global", "Clicked leaderboard global.", 1);
	}

	public void Event_Button_PressedLeaderBoardSocial()
	{
		LogEvent ("Button Click", "Leaderboard Social", "Clicked leaderboard social.", 1);

	}
		
	public void Event_Button_EndGamePlay()
	{
		LogEvent ("Button Click", "End Game Play Button", "Clicked play at endgame panel. (Replay)", 1);
	}

	// GAME EVENTS
	public void Event_StartedGame()
	{
		LogEvent ("Game Event", "Start Game", "Started a new game.", 1);

	}

	public void Event_Scored(int score)
	{
		LogEvent ("Game Event", "Scored", "Scored " + score + " points.", score);
	}


	// META EVENTS

	public void Event_GoldEarned(int amount)
	{
		LogEvent ("Meta Event", "Gold Earned", "Gold earned,  " + amount, amount);
	}

	public void Event_GoldSpent(int amount)
	{
		LogEvent ("Meta Event", "Gold Spent", "Gold spent,  " + amount, amount);
	}

	public void Event_GoldRefund(int amount)
	{
		LogEvent ("Meta Event", "Gold Refund", "Gold refund,  " + amount, amount);
	}

	public void Event_EnablePowerUp(PowerUp powerUp)
	{
		LogEvent ("Meta Event", "Enable Power Up", "Enabled powerup,  " + powerUp.name, 1);
		Event_GoldSpent (powerUp.cost);
	}

	public void Event_DisablePowerUp(PowerUp powerUp)
	{
		LogEvent ("Meta Event", "Disable Power Up", "Disabled powerup,  " + powerUp.name, 1);
		Event_GoldRefund (powerUp.cost);
	}

}
