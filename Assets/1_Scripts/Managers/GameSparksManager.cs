#define USE_LOGS
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;
using System.Collections.Generic;
using System;
using SimpleJSON;

public class GameSparksPlayer 
{
//	_playerId, _name, _picture, _rank, _score
	public string userId, name;
	public int coins, highscore, bestRank;
	public Sprite picture;
	public List<string> earnedAchievements = new List<string>();
    public List<int> medals;
	public List<int> ballCounts;

	public GameSparksPlayer (string _userId)
	{
		userId = _userId;
	}
}

public class MedalAlert
{

	public MedalAlert(string _medal, int _score = 0)
	{
		medal = _medal;
		score = _score;
	}

	public string medal = "";
	public int score;
}

public class GameSparksManager : MonoBehaviour
{
    public static GameSparksManager Instance { private set; get; }

	public GameSparksPlayer player;
	public bool playerInitialised = false;

    public bool IsLoggedIn{
        get{
            
            return player != null;
        }

    }
//	public List<string> playerEarnedAchievements;

    void Awake()
    {
        Instance = this;
    }

//	void Start()
//	{
//		GS.GameSparksAvailable += HandleGameSparksAvailable;
//	}
//
//	void HandleGameSparksAvailable(bool available)
//	{
//		Debug.Log("available " + available);
//
//	}

//    public void SendDeviceAuthenticationRequestWithFacebookDisplayName(string facebookDisplayName)
//    {
//        new DeviceAuthenticationRequest().SetDisplayName(facebookDisplayName).Send((response) =>
//        {
//            if (response.HasErrors)
//            {
//				Trace.Msg("COULD NOT AUTHENTICATE DEVICE WITH NAME " + facebookDisplayName);
//
//				DebugConsole.Log("COULD NOT AUTHENTICATE DEVICE WITH NAME " + facebookDisplayName, "error");
//            }
//            else
//            {
//                Trace.Msg("AUTHENTICATED DEVICE WITH NAME " + facebookDisplayName);
//				DebugConsole.Log("AUTHENTICATED DEVICE WITH NAME " + facebookDisplayName);
//
//                SendFacebookAuthenticationRequest(AccessToken.CurrentAccessToken.TokenString);
//            }
//        });
//    }

    public void SendFacebookAuthenticationRequest(string accessTokenString)
    {
        new FacebookConnectRequest().SetAccessToken(accessTokenString).Send((response) =>
	        {
	            if (response.HasErrors)
	            {
//	                UIManager.Instance.UpdateDebugText("COULD NOT CONNECT WITH GAMESPARKS");
					DebugConsole.Log("COULD NOT CONNECT WITH GAMESPARKS","warning");

                    UIManager.Instance.OnLoginFailed ();

                    FacebookManager.Instance.InvokeLoginEvents(false);

//                    if(FacebookManager.Instance.LoginFailed != null)
//                    {
//                        FacebookManager.Instance.LoginFailed();
//                    }
	            }
	            else
	            {
//	                UIManager.Instance.UpdateDebugText("CONNECTED WITH GAMESPARKS");
					DebugConsole.Log("CONNECTED WITH GAMESPARKS");
					
					playerInitialised = true;

					GetAccountDetails();
                    FacebookManager.Instance.InvokeLoginEvents(true);

//	                InitializeLeaderboard();
//					InitializeSocialLeaderboard();
	            }
	        });
    }

	/// <summary>
	/// Creates the gamesparks player with details.
	/// </summary>
	public void GetAccountDetails ()
	{
		new GameSparks.Api.Requests.AccountDetailsRequest().Send((response) => {

			if (!response.HasErrors) 
			{
				DebugConsole.Log("GetAccountDetails succeded!");

				// Data to expect
				string userId;
				string playerName;
				int currency;
				List<string> earnedAchievements;

				//  Player details
				userId = response.UserId;
				playerName = response.DisplayName; 

				// Achievements
				earnedAchievements = response.Achievements;

				// Currency
				List < int > currencyList = new List < int > () {
					int.Parse(response.Currency1.ToString()), int.Parse(response.Currency2.ToString()), int.Parse(response.Currency3.ToString()), int.Parse(response.Currency4.ToString()), int.Parse(response.Currency5.ToString()), int.Parse(response.Currency6.ToString()),
				};

				currency = currencyList[0];

				// Then we can Trace.Msg this data out or use it wherever we want 
				//				Debug.Log("Player Name: " + playerName);
				//				foreach(string s in achievementsList) {
				//					Debug.Log("Player Earned: " + s);
				//				}
				//				for (int i = 0; i < currencyList.Count; i++) {
				//					Debug.Log("Currency" + i + "    " + currencyList[i].ToString());
				//				}

				player = new GameSparksPlayer(userId);
				player.name = playerName;
				player.coins = int.Parse(response.Currency1.ToString());

				if(earnedAchievements != null)
				{
					foreach (var item in earnedAchievements) 
					{
						DebugConsole.Log(item);
					}
					player.earnedAchievements = earnedAchievements;
				}

				AchievementManager.Instance.Init();

//				UIManager.Instance.RefreshCurrency();

				// Additinal details
				InitialActions();

			} 
			else 
			{
				DebugConsole.Log("GetAccountDetails failed!", "warning");
			}
		});

	}

	/// <summary>
	/// Performs the initial actions, finalizes the creation of the player
	/// getting
	/// 	initializing leadersboards
	/// 	remaining seconds to next week
	/// 	medal count
	/// 	friends
	/// 	global rank
	/// 	</summary>
	public void InitialActions()
	{
		InitializeAllLeaderboards();
//		InitializeLeaderboard(asSocial:true);
//		InitializeSocialLeaderboard();

        // Create a new waiter to wait for bulk actions
		Waiter newWaiter = new Waiter (this, () => {
			Trace.Msg(("Done all initial actions!").Colored(Colors.blue));
			AchievementManager.Instance.CheckLoggedInGameAchievements ();

            MetaManager.Instance.SyncAllMeta();

		});

		newWaiter.AddJob ("remaining");
		GetRemainingSeconds ((seconds) => {
			
			UIManager.Instance.mainPanelLeaderBoard.InitRemainingTime ((int)seconds);
			UIManager.Instance.endGamePanel.leaderboard.InitRemainingTime ((int)seconds);
			newWaiter.JobDone ("remaining");

		});

		newWaiter.AddJob ("med");
		CheckNewMedal((medalAlert)=>{

			if(medalAlert.medal != "none")
			{
				UIManager.Instance.medalPanel.LoadWithMedalAlert(medalAlert);
				
			}
			newWaiter.JobDone ("med");

			// TODO: Butun islerin bitmesin bekle
		});

        newWaiter.AddJob ("med1");
		GetMedalsByID (player.userId, (medalArray) => {
			player.medals = new List<int>(medalArray);
			newWaiter.JobDone ("med1");
		});

		newWaiter.AddJob ("ball");
		GetBallCounts ((ballCounts) => {
            player.ballCounts =  new List<int>(ballCounts);
//			UIManager.Instance.endGamePanel.InitBallFillers(ballCounts);
			newWaiter.JobDone ("ball");
		});

		newWaiter.AddJob ("score");

		GetBestRankAndScoreByID (player.userId, (rankAndHighscoreArray) => {
			player.bestRank = rankAndHighscoreArray[0];

//			player.highscore = CheckIfShouldAndMoveScore(rankAndHighscoreArray[1]);
            player.highscore = rankAndHighscoreArray[1];

//			if(!CheckIfShouldAndMoveScore(rankAndHighscoreArray[1]))
//			{
//				player.highscore = rankAndHighscoreArray[1];
			newWaiter.JobDone ("score");
//			}
		});	

	}

    // TODO: Move this somewehere else
    /// <summary>
    /// Waiter. 
    /// Calls the callback function once all jobs are done.
    /// </summary>
	class Waiter
	{
		MonoBehaviour owner;
		Dictionary<string, bool> waitingDict = new Dictionary<string, bool>();
		Action callback;

		public Waiter(MonoBehaviour owner, System.Action callback)
		{
			this.owner = owner;
			this.callback = callback;
		}

		public void AddJob(string id)
		{
			Trace.Msg ("Waiter add job! " + id);
			waitingDict.Add (id, false);

			if(waitingDict.Count == 1)
			{
				owner.StartCoroutine (StartWaiting ());
			}
		}

		public void JobDone(string id)
		{
			Trace.Msg ("Waiter done job! " + id);
			waitingDict.Remove (id);
		}

		System.Collections.IEnumerator StartWaiting()
		{
			//			bool done = false;
			Trace.Msg ("Waiter started!");
			while(waitingDict.Count > 0)
			{
				yield return true;
			}
			// Done all jobs
			Trace.Msg ("Waiter done!");
			callback ();
		}

	}
	//====//

	// Local Player

    public void AddCoins(int amount)
	{
        if (!IsLoggedIn)
        {
            DebugConsole.Log ("Can't AddCoins, not logged in!", "warning");
            return;
        }

        player.coins += amount;

		new LogEventRequest_EVT_ADDCOINS ().Set_AMOUNT(amount).Send ((response) => {

			if(response.HasErrors)
			{

				DebugConsole.Log("Event addcoins failed!", "warning");
			}
			else
			{
				DebugConsole.Log("Event addcoins succeded!, amount: " + amount);
//				player.coins += amount;

//                UIManager.Instance.RefreshCurrency();
//				if(callback != null)
//				{
//					callback();
//				}

			}
		});
	}

    // TODO: SetCoins
    public void SetCoins(int amount)
    {
        GameSparksManager.Instance.player.coins = amount;

		new LogEventRequest_EVT_SETCOINS ().Set_AMOUNT (amount).Send ((response) => {

			if(response.HasErrors)
			{

				DebugConsole.Log("Event RemoveCoins failed!", "warning");
			}
			else
			{
				var resultCoins = response.BaseData.GetInt("resultCoins");


				DebugConsole.Log("Event RemoveCoins succeded! Result: " + resultCoins);
			}

		});
    }

	public void RemoveCoins(int amount)
	{
        if (!IsLoggedIn)
        {
            DebugConsole.Log ("Can't RemoveCoins, not logged in!", "warning");
            return;
        }

        player.coins -= amount;

		new LogEventRequest_EVT_RMVCOINS ().Set_AMOUNT(amount).Send ((response) => {

			if(response.HasErrors)
			{

				DebugConsole.Log("Event RemoveCoins failed!", "warning");
			}
			else
			{
//                UIManager.Instance.RefreshCurrency();
//				player.coins -= amount;
//				UIManager.Instance.RefreshCurrency(player.coins);
				//				int newamount = 
				//					foreach (var item in response_) 
				//					{
				//						Trace.Msg(item);
				//					}


				DebugConsole.Log("Event RemoveCoins succeded!");
			}
		});
	}

    public void PostScore(int score)
	{
		if (player == null)
		{
			DebugConsole.Log ("Can't PostScore, not logged in!", "warning");
			return;
		}
		if(score > player.highscore)
			player.highscore = score;

		DebugConsole.Log("GameSparks post score: " + score);

        new LogEventRequest_SCORE_EVT().Set_SCORE_ATTR(score).Send((response) =>
	        {
	            if (response.HasErrors)
	            {
//	                UIManager.Instance.UpdateDebugText("Leaderboard score post failed!");
					DebugConsole.Log("Leaderboard score post failed!", "warning");
					DebugConsole.Log("Leaderboard score post failed!", response.Errors.JSON);
	            }
	            else
	            {
//	                UIManager.Instance.UpdateDebugText("Leaderboard score post succeeded!");
					DebugConsole.Log("Leaderboard score post succeeded!, score: " + score);


					UIManager.Instance.mainPanelLeaderBoard.ResetEntries();
					UIManager.Instance.endGamePanel.leaderboard.ResetEntries();

	                InitializeAllLeaderboards();
//					InitializeLeaderboard(asSocial: true);
//					UIManager.Instance.mainPanelLeaderBoard.InitLocalPlayer (player.userId);
//					InitializeSocialLeaderboard();
	            }
	        });
    }

    // TODO: PostHiscore
    public void SetHiscore(int score)
    {
        GameSparksManager.Instance.player.highscore = score;

        Debug.LogError("Not yet implemented!");
    }

	public void InitializeAllLeaderboards ()
	{
//		new AroundMeLeaderboardRequest_HIGH_SCORE_LB


		new AroundMeLeaderboardRequest_HIGH_SCORE_LB ().SetIncludeFirst (7).SetEntryCount (50).SetSocial (false).Send ((response) => {
			// TEST FOR LONG LIST
//			var duplicatedList = new List<AroundMeLeaderboardResponse._LeaderboardData> ();
//			for (int i = 0; i < 10; i++) {
//				foreach (var entry in response.Data) {
//					duplicatedList.Add (entry);
//				}
//			}
			//			foreach (var entry in duplicatedList) {

			// Add all entries
			foreach (var entry in response.Data) {
				string playerId = entry.UserId;
				string facebookId = entry.ExternalIds.GetString ("FB");
				int rank = (int)entry.Rank;
				string name = entry.UserName;
				int score = (int)entry.GetNumberValue ("SCORE_ATTR");
				
//				if (!asSocial) {
					UIManager.Instance.mainPanelLeaderBoard.AddEntry (playerId, facebookId, rank, name, score, false);
					UIManager.Instance.endGamePanel.leaderboard.AddEntry (playerId, facebookId, rank, name, score, false);
//					UIManager.Instance.mainPanelLeaderBoard.AddEntry (playerId, facebookId, rank, name, score, true);
//					UIManager.Instance.endGamePanel.leaderboard.AddEntry (playerId, facebookId, rank, name, score, true);
//				}
			}


			//		var allLeaderboards = new UILeaderBoard[] {
			//			UIManager.Instance.mainPanelLeaderBoard,
			//			UIManager.Instance.endGamePanel.leaderboard
			//		};
			// Scroll to & Mark current player

//			UIManager.Instance.endGamePanel.leaderboard.InitLocalPlayer ();

		});

		// Social
		new AroundMeLeaderboardRequest_HIGH_SCORE_LB ().SetIncludeFirst (7).SetEntryCount (50).SetSocial (true).Send ((response) => {
			// TEST FOR LONG LIST
			//			var duplicatedList = new List<AroundMeLeaderboardResponse._LeaderboardData> ();
			//			for (int i = 0; i < 10; i++) {
			//				foreach (var entry in response.Data) {
			//					duplicatedList.Add (entry);
			//				}
			//			}
			//			foreach (var entry in duplicatedList) {

			// Add all entries
			foreach (var entry in response.Data) {
				string playerId = entry.UserId;
				string facebookId = entry.ExternalIds.GetString ("FB");
				int rank = (int)entry.Rank;
				string name = entry.UserName;
				int score = (int)entry.GetNumberValue ("SCORE_ATTR");

				//				if (!asSocial) {
//				UIManager.Instance.mainPanelLeaderBoard.AddEntry (playerId, facebookId, rank, name, score, false);
//				UIManager.Instance.endGamePanel.leaderboard.AddEntry (playerId, facebookId, rank, name, score, false);
				UIManager.Instance.mainPanelLeaderBoard.AddEntry (playerId, facebookId, rank, name, score, true);
				UIManager.Instance.endGamePanel.leaderboard.AddEntry (playerId, facebookId, rank, name, score, true);
				//				}
			}


			//		var allLeaderboards = new UILeaderBoard[] {
			//			UIManager.Instance.mainPanelLeaderBoard,
			//			UIManager.Instance.endGamePanel.leaderboard
			//		};
			// Scroll to & Mark current player

			//			UIManager.Instance.endGamePanel.leaderboard.InitLocalPlayer ();

		});

		

	}

//	public void InitializeSocialLeaderboard()
//	{
////		UIManager.Instance.mainPanelLeaderBoard.ResetEntries();
////		UIManager.Instance.mainPanelLeaderBoard.ResetEntries();
//
//		new LeaderboardDataRequest_HIGH_SCORE_LB().SetSocial(true).Send((LeaderboardDataResponse_HIGH_SCORE_LB response) =>
//			{
//				foreach (var entry in response.Data)
//				{
//					string playerId = entry.UserId;
//					string facebookId = entry.ExternalIds.GetString("FB");
//					int rank = (int)entry.Rank;
//					string name = entry.UserName;
//					int score = (int)entry.GetNumberValue("SCORE_ATTR");
//
//					UIManager.Instance.mainPanelLeaderBoard.AddEntry(playerId, facebookId, rank, name, score, true);
//					UIManager.Instance.endGamePanel.leaderboard.AddEntry(playerId, facebookId, rank, name, score, true);
//				}
//			});
//
//
//
//	}

	public void CheckNewMedal(Action<MedalAlert> Callback)
	{
		new LogEventRequest_EVT_CHCKMEDAL ().Send ((response) => {

			if (response.HasErrors) 
			{
				DebugConsole.Log("CheckNewMedal failed!", "warning");

			}
			else
			{
				DebugConsole.Log("CheckNewMedal succeeded!");

				// If no script data exist, no new medal exists
				if(response.ScriptData == null)
				{
					DebugConsole.Log("No new medal!");

					Callback(new MedalAlert("none"));
				}
				else
				{
					string medal = response.ScriptData.GetString("alert");
					int? score = response.ScriptData.GetInt("score");

					MedalAlert medalAlert = new MedalAlert(medal, (int)score);

					Callback(medalAlert);

					DebugConsole.Log(medalAlert.medal + ", " + medalAlert.score);

				}
			}

		});
	}

	public void GetPlayerFriendIDs(Action<List<string>> Callback)
	{
		
		new LogEventRequest_EVT_GETFRIENDIDS ().Send ((response) => 
			{
				if(response.HasErrors)
				{

					DebugConsole.Log("GetBestGlobalRankById failed!", "warning");
				}
				else
				{
					List<string> friendIDList = response.ScriptData.GetStringList("friendIDArray");

					Callback(friendIDList);

					DebugConsole.Log("GetBestGlobalRankById succeeded!");
				}
			});
	}

	public void GetBallCounts(Action<int[]> Callback)
	{
		if (player == null)
		{
			DebugConsole.Log ("Can't GetBallCounts, not logged in!", "warning");
			return;
		}

		new LogEventRequest_EVT_GETBALLS ().Send ((response) => {
			if (response.HasErrors) {

				DebugConsole.Log ("GetBallCounts failed!", "warning");
				return;
			} else {

				List<int> ballCounts = response.ScriptData.GetIntList("ballCounts");

				DebugConsole.Log("GetBallCounts succeeded!");

				for (int i = 0; i < ballCounts.Count; i++) {
					
					DebugConsole.Log(("Ball Count Lvl " + i + ":")  + ballCounts[i]);
									
				}

				Callback(ballCounts.ToArray());	
			}
		});

	}

	public void SetBallCounts(int[] ballCounts ,Action<int[]> Callback = null)
	{
        if (player == null)
        {
            DebugConsole.Log ("Can't SetBallCounts, not logged in!", "warning");
            return;
        }

        player.ballCounts = new List<int>( ballCounts);

		new LogEventRequest_EVT_SETBALLS ().Set_LVL0(ballCounts[0]).Set_LVL1(ballCounts[1]).Set_LVL2(ballCounts[2]).Set_LVL3(ballCounts[3]).Send ((response) => {
			if (response.HasErrors) {

				DebugConsole.Log ("SetBallCounts failed!", "warning");

			} else {
				DebugConsole.Log("SetBallCounts succeeded!");
			}
		});

	}

	public void Achieve(string shortcode)
	{

		if (player == null)
		{
			DebugConsole.Log ("Can't Achieve achievement, not logged in! Achievement:" + shortcode, "warning");
			return;
		}

		player.earnedAchievements.Add (shortcode);
		new LogEventRequest_EVT_ACHVMNT ().Set_ACHVMNT (shortcode).Send ((response) => {
			if (response.HasErrors) {

				DebugConsole.Log ("Achieve failed!, achivement: " + shortcode, "warning");

			} else {

				//				List<int> ballCounts = response.ScriptData.GetIntList("ballCounts");

				DebugConsole.Log("Achieve succeeded!, achivement: " + shortcode);

				//				for (int i = 0; i < ballCounts.Count; i++) {
				//					
				//					DebugConsole.Log("Ball Count Lvl 0 :" + ballCounts[0]);
				//									
				//				}
				//
				//				Callback(ballCounts.ToArray());	
			}
		});
	}

	// General Player 

	public void GetBestRankAndScoreByID(string playerID, Action<int[]> Callback)
	{
		new LogEventRequest_EVT_GETBESTGLBRANK ().Set_PLAYERID(playerID).Send ((response) => 
			{
				if(response.HasErrors)
				{
					DebugConsole.Log("GetBestGlobalRankById failed!", "warning");
				}
				else
				{
					int[] rankAndScore = new int[]{0, 0};

					try {
						int? bestGlobalScore = response.ScriptData.GetInt("playerBestGlobalScore");
						int? bestGlobalRank = response.ScriptData.GetInt("playerBestGlobalRank");


						rankAndScore = new int[]{(int)bestGlobalRank, (int)bestGlobalScore};

						DebugConsole.Log(rankAndScore[0] + ", " + rankAndScore[1]);
						
						DebugConsole.Log("GetBestGlobalRankById succeded!");
					} catch (Exception ex) {

						DebugConsole.Log("GetBestGlobalRankById returned empty!", "warning");
						Trace.Msg("GetBestGlobalRankById returned empty, exception: " + ex);

						rankAndScore = new int[]{0, 0};

					}

					Callback(rankAndScore);
				}
			}
		);

	}

	public void GetMedalsByID(string playerID, Action<int[]> Callback)
	{
		new LogEventRequest_EVT_GETMEDALCTID ().Set_PLAYERID(playerID).Send ((response) => 
			{
				if(response.HasErrors)
				{
					DebugConsole.Log("GetMedalsByID failed!", "warning");
				}
				else
				{
					DebugConsole.Log("GetMedalsByID succeeded!");

					int? goldCt = response.ScriptData.GetInt("gold");
					int? silverCt = response.ScriptData.GetInt("silver");
					int? bronzeCt = response.ScriptData.GetInt("bronze");

					int[] returnVal = new int[]{(int)goldCt, (int)silverCt, (int)bronzeCt};

					Callback(returnVal);

					DebugConsole.Log(returnVal[0] + ", " + returnVal[1] + ", " + returnVal[2]);

				}
			}
		);

	}

	public void GetAchivementsByID(string playerID, Action<List<string>> Callback)
	{

		new LogEventRequest_EVT_GETACHVMNTSID ().Set_player_id(playerID).Send ((response) => 
			{
				if(response.HasErrors)
				{

					DebugConsole.Log("GetAchivementsById failed!", "warning");
				}
				else
				{
					List<string> earnedAchievements = response.ScriptData.GetStringList("achievements");
//					foreach (var item in response_) 
//					{
//						Trace.Msg(item);
//					}

					Callback(earnedAchievements);

					DebugConsole.Log("GetAchivementsById succeded!");
					Trace.Msg(earnedAchievements.Count + " achivement earned in total!");
					foreach (var ach in earnedAchievements) {
						Trace.Msg(ach);
					}
				}
			}
		);

	}

	public void notusing_GetLeaderBoardData(string playerID, Action<int[]> callback)
	{
		List<string> leaderboards = new List<string> ();
		leaderboards.Add ("HIGH_SCORE_LB");

		new GetLeaderboardEntriesRequest ().SetPlayer (playerID).SetLeaderboards (leaderboards).Send ((GetLeaderboardEntriesResponse response) => {
		
//			GSData results = response.Results;

			string json = response.JSONString;
			var N = SimpleJSON.JSON.Parse(json);
			var rankValue = N["HIGH_SCORE_LB"]["rank"].AsInt;
			var highScoreValue = N["HIGH_SCORE_LB"]["SCORE_ATTR"].AsInt;
			Debug.Log("Player rank: " + rankValue);

			callback(new int[]{rankValue, highScoreValue});

		});
	}

	// Other

	/// <summary>
	/// Gets the remaining seconds to next week from server
	/// </summary>
	/// <param name="Callback">Callback.</param>
	public void GetRemainingSeconds(Action<int> Callback)
	{
		new LogEventRequest_EVT_GETTIME ().Send ((response) => 
			{

			if(response.HasErrors)
			{

					DebugConsole.Log("GetRemainingSecods failed!", "warning");
			}
			else
			{
					int? remainingTimeInSeconds = response.ScriptData.GetInt("remaining");
				//					foreach (var item in response_) 
				//					{
				//						Trace.Msg(item);
				//					}

					if(remainingTimeInSeconds != null)
					{
						Callback((int)remainingTimeInSeconds);
					}

					DebugConsole.Log("GetRemainingSecods succeded!");
					DebugConsole.Log(((int)remainingTimeInSeconds).ToString());
			}


			}
		);
	}
	
    public void RegisterForPushNotifications(string pushId) {

		DebugConsole.Log("RegisterForPushNotifications called on GameSparks Manager with id: " + pushId);

        Utility.Instance.WaitForCondition(() =>
            {
                return player != null;
            }, () =>
            {
                new PushRegistrationRequest()
                .SetDeviceOS("ios")
                .SetPushId(pushId)
                .Send((response) =>
                    {
                        string registrationId = response.RegistrationId; 
                            DebugConsole.Log("RegistrationId: " + registrationId);
                        GSData scriptData = response.ScriptData; 
                        DebugConsole.Log("Succesfully registered for push notifications on GameSparks!");
                    });
            });

    }

}
