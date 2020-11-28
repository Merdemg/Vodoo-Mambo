using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour
{
    private bool mAuthenticating = false;
    
    public bool Authenticating
    {
        get { return mAuthenticating; }
    }

    public bool Authenticated
    {
        get { return Social.Active.localUser.authenticated; }
    }

    //public PlayGamesPlatform platform;

    // Start is called before the first frame update
    void Start()
    {
        Authenticate();

        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();


        //SignIn();
    }

    public void Authenticate()
    {
        if (Authenticated || mAuthenticating)
        {
            Debug.LogWarning("Ignoring repeated call to Authenticate().");
            return;
        }

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                //.EnableSavedGames()
                .Build();
        PlayGamesPlatform.InitializeInstance(config);

        // Activate the Play Games platform. This will make it the default
        // implementation of Social.Active
        PlayGamesPlatform.Activate();

        // Set the default leaderboard for the leaderboards UI
        //((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(GameIds.LeaderboardId);

        // Sign in to Google Play Games
        mAuthenticating = true;
        Social.localUser.Authenticate((bool success) =>
        {
            mAuthenticating = false;
            if (success)
            {
                // if we signed in successfully, load data from cloud
                Debug.Log("Login successful!");
            }
            else
            {
                // no need to show error message (error messages are shown automatically
                // by plugin)
                Debug.LogWarning("Failed to sign in with Google Play Games.");
            }
        });
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
        if (Authenticated)
        {
            Social.ShowLeaderboardUI();
        }

        //testSignIn();

        //Social.Active.ShowLeaderboardUI();
        //PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }
}
