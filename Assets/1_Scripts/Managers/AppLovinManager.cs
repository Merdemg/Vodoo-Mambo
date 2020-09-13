#define USE_LOGS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Time = Org.BouncyCastle.Asn1.X509.Time;

public class AppLovinManager : MonoBehaviour {

    #region Singleton
    public static AppLovinManager Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }

    #endregion

	public bool isInterVisible{private set; get;}

	public bool isRewardedVideoWatched{private set; get;}

	System.Action<bool> nextRewardCallback = null;

	private float _lastInterTime;
	private int _interTryShowCount;
	
	// Use this for initialization
	void Start () {
		// Feature: AppLovin SDK Key Update
        // 2019.11.12 - LEVON
        //
		// AppLovin.SetSdkKey("bk5rpwDg0PCmQpEZJ6xlT68-pWsGaI0B161r0EV0ein4V_dq6ZZffidTDTkpnk_pq0dqF7ruqbyKba9h8iLoP-");
		AppLovin.SetSdkKey("pTBBfH62Vzpt9ivdcDuMoOJaBXuWsb9NVSKKV2ECSTViHfCtl89iEOcOQuPgwKicHZCvgNAGr_hACaRxc-2N82");
		//
		// End of Feature


        AppLovin.InitializeSdk();

        AppLovin.SetUnityAdListener("AppLovinManager");

        AppLovin.SetHasUserConsent ("false");

		InitialiseAds ();
	}

    void InitialiseAds() {
        AppLovin.PreloadInterstitial();
		AppLovin.LoadRewardedInterstitial();
    }

	void onAppLovinEventReceived(string ev){
		DebugConsole.Log ("Applovin event: " + ev);
		
		// IOSNativePopUpManager.showMessage( "Applovin Event", ev );
		
		if(ev.Contains("DISPLAYEDINTER")) {
			// An ad was shown.  Pause the game.
			isInterVisible = true;
		}
		else if(ev.Contains("HIDDENINTER")) {
			// Ad ad was closed.  Resume the game.
			// If you're using PreloadInterstitial/HasPreloadedInterstitial, make a preload call here.
			AppLovin.PreloadInterstitial();
			isInterVisible = false;
		}
		else if(ev.Contains("LOADEDINTER")) {
			// An interstitial ad was successfully loaded.
		}
		else if(string.Equals(ev, "LOADINTERFAILED")) {
			// An interstitial ad failed to load.
		}


		// The format would be "REWARDAPPROVEDINFO|AMOUNT|CURRENCY"
		if (ev.Contains("REWARDAPPROVEDINFO")) {
			// For this example, assume the event is "REWARDAPPROVEDINFO|10|Coins"
//			string delimeter = "|";

			// Split the string based on the delimeter
//			string[] split = ev.Split(delimeter);

			// Pull out the currency amount
//			double amount = double.Parse(split[1]);

			// Pull out the currency name
//			string currencyName = split[2];

			// Do something with the values from above.  For example, grant the coins to the user.
			if(nextRewardCallback != null) {
				nextRewardCallback (true);
				nextRewardCallback = null;
			}
//			updateBalance(amount, currencyName);
		}
		else if(ev.Contains("LOADEDREWARDED")) {
			// A rewarded video was successfully loaded.
		}
		else if(ev.Contains("LOADREWARDEDFAILED")) {
			// A rewarded video failed to load.
		}
		else if(ev.Contains("HIDDENREWARDED")) {
			// A rewarded video has been closed.  Preload the next rewarded video.
			AppLovin.LoadRewardedInterstitial();
		}
	}

	public void setRewardedVideoWatch( bool flag ) {
		
		isRewardedVideoWatched = flag;

		// 
		// Feature: Don't show standard ad if rewarded video has been watched
		// 2020.02.26 - LEVON
		//
		//
		// UIPanel_EndGame.ShowRewardedVideo() calls this very function to set isRewardedVideoWatched flag to true
		// ShowInter() simply returns (does nothing) if the flag is set
		// UIManager.ShowPregame() calls this very function to set isRewardedVideoWatched flag to false
		//

	}

	public void ShowInter()
	{
		if ( isRewardedVideoWatched ) {
			return; }
		
		// Max Ad interval
		if(UnityEngine.Time.realtimeSinceStartup - _lastInterTime < 50) return;
		_lastInterTime = UnityEngine.Time.realtimeSinceStartup;
		
		// Don't show inter on specific 
		_interTryShowCount++;
		if(_interTryShowCount == 1 || _interTryShowCount == 3 || _interTryShowCount == 7) return;
		
		// Showing utilizing PreloadInterstitial and HasPreloadedInterstitial
		if(AppLovin.HasPreloadedInterstitial()){
			// An ad is currently available, so show the interstitial.
			AppLovin.ShowInterstitial ();
			isInterVisible = true;
		}
		else{
			// No ad is available.  Perform failover logic...
			DebugConsole.Log("Not preloaded inter yet");
		}
	}

	public void ShowAwardedInter(System.Action<bool> callback) {
		// Check to see if a rewarded video is ready before attempting to show.
		if(AppLovin.IsIncentInterstitialReady()) {
//			AppLovin.Showre
			AppLovin.ShowRewardedInterstitial();
			nextRewardCallback = callback;
		}
		else{
			callback (false);
			// No rewarded ad is available.  Perform failover logic...
		}
	}

    public void TestInter() {
        // Showing utilizing PreloadInterstitial and HasPreloadedInterstitial
        if(AppLovin.HasPreloadedInterstitial()){
            // An ad is currently available, so show the interstitial.
            AppLovin.ShowInterstitial();
        }
        else{
            // No ad is available.  Perform failover logic...
            DebugConsole.Log("Not preloaded inter yet");
        }
    }
}
