#define USE_LOGS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterManager : MonoBehaviour {

	#region Singleton
	public static GameCenterManager Instance { private set; get; }

	void Awake()
	{
		Instance = this;
	}

    #endregion

    public bool loginSuccessful = false;
	string leaderboardID = "01";

	void Start()
	{
		AuthenticateUser ();
	}

	void AuthenticateUser()
	{
        Social.localUser.Authenticate((bool success) => {
            if (success)
			{
				loginSuccessful = true;
			}
		});
	}

	public void PostScoreOnLeaderBoard(int myScore)
	{
        #if !UNITY_EDITOR
        if (loginSuccessful)
		{
            Social.ReportScore(myScore, leaderboardID, (bool success) => {
                if (success)
				{
					Debug.Log("Game Center - Report Score successful!");

				}
				else
				{
                    Debug.Log("Game Center authenticate unsuccessful!");
				}
			});
		}
		else
		{
		Social.localUser.Authenticate((bool success) => {
            // Fix Bug: Crash on Endgame
            // 2019.10.21 - LEVON
            //
            if (success)
            {
                loginSuccessful = true;
                return;
            }
            // End of Fix Bug
            DebugConsole.Log("Game Center authenticate unsuccessful!", "error");
            PostScoreOnLeaderBoard(myScore);
        });
        }
		#endif
	}

	public void OpenGameCenter()
	{
		Social.ShowLeaderboardUI();
	}
}
