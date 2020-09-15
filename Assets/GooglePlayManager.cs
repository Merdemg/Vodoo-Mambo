using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    public void UnlockAchievement(string id)
    {
        Social.ReportProgress(id, 100, success => { });
    }

    public void AddScoreToLeaderboard(string leaderboardId, long score)
    {
        Social.ReportScore(score, leaderboardId, succcess => { });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
}
