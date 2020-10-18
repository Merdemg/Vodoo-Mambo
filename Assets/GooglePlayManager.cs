using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    public PlayGamesPlatform platform;

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();


        SignIn();
    }


    void testSignIn()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void SignIn()
    {
        Social.Active.localUser.Authenticate(success =>
        {

            if (success)
            {
                Debug.Log("Logged in successfully");
            }
            else
            {
                Debug.Log("Could not log in");
            }
        });
    }

    public void UnlockAchievement(string id)
    {
        Social.Active.ReportProgress(id, 100, success => { });
        //PlayGamesPlatform.Instance.ReportProgress(id, 100, success => { });
    }

    public void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.Active.ReportScore(score, leaderboardId, succcess => { });
        //PlayGamesPlatform.Instance.ReportScore(score, leaderboardId, succcess => { });
    }

    public void ShowLeaderboard()
    {
        testSignIn();

        Social.Active.ShowLeaderboardUI();
        //PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
